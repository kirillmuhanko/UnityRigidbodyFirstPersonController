namespace Characters.FirstPerson.States
{
    public abstract class BaseState
    {
        public abstract void OnStateEnter(FirstPersonBrain firstPerson);

        public abstract void OnStateUpdate(FirstPersonBrain firstPerson);

        public abstract void OnStateExit(FirstPersonBrain firstPerson);
    }
}