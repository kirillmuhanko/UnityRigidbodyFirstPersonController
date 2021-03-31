using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.FirstPerson.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonOrientation : MonoBehaviour
    {
        [SerializeField] private float cameraSmoothTime = 0.05f;
        [SerializeField] private float mouseSensitivity = 4f;
        [SerializeField] [Range(0f, 0.5f)] private float mouseSmoothTime = 0.05f;
        [SerializeField] [Range(1, 90)] private int pitchLimit = 90;
        [SerializeField] private Transform playerCamera;
        [SerializeField] private Vector3 cameraOffset = new Vector3(0f, 0.5f, 0f);
        private Rigidbody _rigidbody;
        private Vector2 _currentMouseDelta;
        private Vector2 _lookAngle;
        private Vector2 _mouseVelocity;
        private Vector2 _targetMouseDelta;
        private Vector3 _cameraVelocity;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _lookAngle.x = transform.eulerAngles.y;
        }

        private void FixedUpdate()
        {
            RotatePlayer();
        }

        private void LateUpdate()
        {
            UpdateMouseLook();
            RotateCamera();
            FollowCamera();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            _targetMouseDelta = context.ReadValue<Vector2>();
        }

        private void FollowCamera()
        {
            var target = transform.TransformPoint(cameraOffset);

            playerCamera.position = Vector3.SmoothDamp(
                playerCamera.position,
                target,
                ref _cameraVelocity,
                cameraSmoothTime);
        }

        private void RotateCamera()
        {
            playerCamera.localRotation = Quaternion.Euler(-_lookAngle.y, _lookAngle.x, 0f);
        }

        private void RotatePlayer()
        {
            _rigidbody.MoveRotation(Quaternion.Euler(0f, _lookAngle.x, 0f));
        }

        private void UpdateMouseLook()
        {
            _currentMouseDelta = Vector2.SmoothDamp(
                _currentMouseDelta,
                _targetMouseDelta,
                ref _mouseVelocity,
                mouseSmoothTime);

            _lookAngle += _currentMouseDelta * mouseSensitivity;
            _lookAngle.y = Mathf.Clamp(_lookAngle.y, -pitchLimit, pitchLimit);
        }
    }
}