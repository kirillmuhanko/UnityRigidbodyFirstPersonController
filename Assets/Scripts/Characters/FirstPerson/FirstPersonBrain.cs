using Characters.FirstPerson.Components;
using Characters.FirstPerson.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.FirstPerson
{
    [RequireComponent(typeof(FirstPersonMotor))]
    [RequireComponent(typeof(FirstPersonOrientation))]
    [RequireComponent(typeof(FirstPersonGroundCheck))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody))]
    public class FirstPersonBrain : MonoBehaviour
    {
        [HideInInspector] public FirstPersonGroundCheck groundCheck;
        [HideInInspector] public FirstPersonMotor motor;
        public readonly IdleState IdleState = new IdleState();
        public readonly JumpingState JumpingState = new JumpingState();
        public readonly RunningState RunningState = new RunningState();
        private BaseState _currentState;
        private Rigidbody _rigidbody;
        public bool IsJumped { get; private set; }
        public Vector2 Movement { get; private set; }

        private void Awake()
        {
            motor = GetComponent<FirstPersonMotor>();
            groundCheck = GetComponent<FirstPersonGroundCheck>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.useGravity = false;
            _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
            _rigidbody.freezeRotation = true;
            _currentState = IdleState;
        }

        private void FixedUpdate()
        {
            _currentState.OnStateUpdate(this);
            IsJumped = false;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            IsJumped = context.performed;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void TransitionToState(BaseState state)
        {
            _currentState.OnStateExit(this);
            _currentState = state;
            _currentState.OnStateEnter(this);
        }
    }
}