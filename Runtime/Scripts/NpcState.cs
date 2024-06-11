

namespace Bakery.NPC
{
    internal abstract class NpcState
    {

        protected NpcController _controller;
        protected NpcStateMachine _machine;
        internal NpcState(NpcStateMachine machine, NpcController controller)
        {
            _controller = controller;
            _machine = machine;
        }

        internal virtual void Enter() { }

        internal virtual void Exit() { }

        internal virtual void FixedUpdate() { }

        internal virtual void Update() { }

    }
}
