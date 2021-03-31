using UnityEngine;

namespace Characters.FirstPerson.States
{
    public class JumpingState : BaseState
    {
        private float _nextJumpTime;

        public override void OnStateEnter(FirstPersonBrain firstPerson)
        {
            firstPerson.motor.Jump();
            _nextJumpTime = Time.fixedTime + 0.2f;
        }

        public override void OnStateExit(FirstPersonBrain firstPerson)
        {
        }

        public override void OnStateUpdate(FirstPersonBrain firstPerson)
        {
            var groundCheck = firstPerson.groundCheck;
            groundCheck.CheckGround();

            if (groundCheck.IsGrounded && Time.fixedTime > _nextJumpTime)
            {
                if (firstPerson.Movement == Vector2.zero)
                    firstPerson.TransitionToState(firstPerson.IdleState);
                else
                    firstPerson.TransitionToState(firstPerson.RunningState);
            }
            else
            {
                firstPerson.motor.MoveInAir(firstPerson.Movement);
                firstPerson.motor.ApplyGravity();
            }
        }
    }
}