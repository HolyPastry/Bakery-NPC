using KBCore.Refs;
using UnityEngine;

namespace Bakery.NPC
{
    internal class NpcStateMachine
    {
        private NpcState _currentState;

        internal object CurrentState => _currentState;

        internal void SetState(NpcState state)
        {
            _currentState?.Exit();
            _currentState = state;
            _currentState.Enter();
        }
        internal void FixedUpdate() => _currentState?.FixedUpdate();

        internal void Update() => _currentState?.Update();

    }
}
