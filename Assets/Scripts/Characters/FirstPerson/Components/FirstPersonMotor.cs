using UnityEngine;

namespace Characters.FirstPerson.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonMotor : MonoBehaviour
    {
        public float accelerationSpeedInAir = 5f;
        public float gravity = 9.81f;
        public float jumpHeight = 1.5f;
        public float maxSpeedOnGround = 5f;
        public float snapToGroundDistance = 0.2f;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void ApplyGravity()
        {
            var velocity = Vector3.down * (gravity * _rigidbody.mass);
            _rigidbody.AddForce(velocity);
        }

        public void Jump()
        {
            var jumpPower = Mathf.Sqrt(2f * jumpHeight * gravity);
            var velocity = _rigidbody.velocity;
            _rigidbody.velocity = new Vector3(velocity.x, jumpPower, velocity.z);
        }

        public void MoveInAir(Vector2 moveInput)
        {
            var move = MoveInputToWorldSpace(moveInput);
            var dotProduct = Vector3.Dot(_rigidbody.velocity, move);
            var velocity = Vector3.zero;

            if (dotProduct < 0f)
                velocity = move * accelerationSpeedInAir;

            _rigidbody.AddForce(velocity, ForceMode.Acceleration);
        }

        public void MoveOnGround(Vector3 normal, Vector2 moveInput)
        {
            var move = MoveInputToWorldSpace(moveInput) * maxSpeedOnGround;
            move = Vector3.ProjectOnPlane(move, normal);
            var velocity = _rigidbody.velocity;
            velocity = move - velocity;
            _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
        }

        public void SnapToGround(float distance, float min, float max, float snapStrength = 1f)
        {
            if (distance <= min || distance >= max)
                return;

            snapStrength = Mathf.Clamp01(snapStrength);
            var velocity = _rigidbody.velocity;
            var snapVelocity = velocity + Vector3.down * snapStrength;
            velocity.y = snapVelocity.normalized.y * velocity.magnitude;
            _rigidbody.velocity = velocity;
        }

        private Vector3 MoveInputToWorldSpace(Vector2 moveInput)
        {
            var move = new Vector3(moveInput.x, 0f, moveInput.y);
            move = Vector3.ClampMagnitude(move, 1f);
            var worldSpace = transform.TransformVector(move);
            return worldSpace;
        }
    }
}