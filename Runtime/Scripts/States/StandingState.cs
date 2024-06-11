namespace Bakery.NPC
{
    internal class StandingState : NpcState
    {
        public StandingState(NpcStateMachine machine, NpcController controller) : base(machine, controller)
        { }

        internal override void Enter()
        {
            if (!_controller.IsActivated)
                _controller.Activate();
            _controller.Idle();
        }
    }
}
