using UnityEngine;

namespace Characters.FirstPerson.States
{
    public class IdleState : BaseState
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
                firstPerson.motor.MoveOnGround(groundCheck.GroundHit.normal, Vector2.zero);

                if (firstPerson.IsJumped)
                    firstPerson.TransitionToState(firstPerson.JumpingState);
                else if (firstPerson.Movement != Vector2.zero)
                    firstPerson.TransitionToState(firstPerson.RunningState);
            }
            else
            {
                firstPerson.motor.ApplyGravity();
            }
        }
    }
}