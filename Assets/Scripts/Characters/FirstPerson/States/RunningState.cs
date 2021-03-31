using UnityEngine;

namespace Characters.FirstPerson.States
{
    public class RunningState : BaseState
    {
        public override void OnStateEnter(FirstPersonBrain firstPerson)
        {
        }

        public override void OnStateExit(FirstPersonBrain firstPerson)
        {
        }

        public override void OnStateUpdate(FirstPersonBrain firstPerson)
        {
            var groundCheck = firstPerson.groundCheck;
            groundCheck.CheckGround();

            if (groundCheck.IsGrounded && !groundCheck.IsOnSteepSlope())
            {
                firstPerson.motor.MoveOnGround(groundCheck.GroundHit.normal, firstPerson.Movement);

                if (firstPerson.IsJumped)
                    firstPerson.TransitionToState(firstPerson.JumpingState);
                else if (firstPerson.Movement == Vector2.zero)
                    firstPerson.TransitionToState(firstPerson.IdleState);
            }
            else
            {
                firstPerson.motor.SnapToGround(
                    groundCheck.GroundDistance,
                    groundCheck.groundCheckDistance,
                    firstPerson.motor.snapToGroundDistance);

                firstPerson.motor.ApplyGravity();
            }
        }
    }
}