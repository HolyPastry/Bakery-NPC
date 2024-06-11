using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Bakery.NPC
{
    internal class GotoState : NpcState
    {
        private Vector3 _position;
        private Coroutine _routine;
        private readonly bool _isRunning;

        public GotoState(NpcStateMachine machine,
             NpcController controller,
              UnityEngine.Vector3 position,
               bool isRunning) : base(machine, controller)
        {
            _position = position;
            _isRunning = isRunning;
        }

        internal override void Enter()
        {
            if (!_controller.IsActivated)
                _controller.Activate();

            _routine = _controller.StartCoroutine(GotoRoutine());
        }

        internal override void Exit()
        {
            base.Exit();
            if (_routine != null)
                _controller.StopCoroutine(_routine);
        }

        private IEnumerator GotoRoutine()
        {
            bool pathFound = false;
            NavMeshPath path = new();
            while (!pathFound)
            {
                pathFound = _controller.FindPath(_position, out path);
                pathFound &= _controller.IsPathFound;
                yield return null;
            }

            _controller.SetPath(path);

            if (_isRunning)
                _controller.RunTo(path);
            else
                _controller.WalkTo(path);

            yield return _controller.WaitUntilReachDestination;

            _machine.SetState(new StandingState(_machine, _controller));

        }


    }
}
