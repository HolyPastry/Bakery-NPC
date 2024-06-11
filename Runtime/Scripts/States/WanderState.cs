

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Bakery.NPC
{
    internal class WanderState : NpcState
    {
        private Vector3 _centerPoint;
        private readonly float _wanderRadius;
        private readonly float _wanderPauseInSeconds;
        private Coroutine _wanderRoutine;

        public WanderState(NpcStateMachine machine,
                           NpcController controller,
                           Vector3 centerPoint,
                           float wanderRadius,
                           float wanderRate)
             : base(machine, controller)
        {
            _centerPoint = centerPoint;
            _wanderRadius = wanderRadius;
            _wanderPauseInSeconds = wanderRate;
        }

        internal override void Enter()
        {
            if (!_controller.IsActivated)
                _controller.Activate();
            _wanderRoutine = _controller.StartCoroutine(WanderRoutine());
        }

        internal override void Exit()
        {
            _controller.StopCoroutine(_wanderRoutine);
        }

        private IEnumerator WanderRoutine()
        {
            while (true)
            {
                Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _wanderRadius;
                randomDirection.y = 0.06f;
                randomDirection += _centerPoint;
                if (!_controller.FindPath(randomDirection, out NavMeshPath path))
                    yield return null;

                if (!_controller.IsPathFound) yield return null;
                _controller.SetPath(path);

                if (Random.Range(0, 2) == 0)
                    _controller.RunTo(path);
                else _controller.WalkTo(path);

                yield return _controller.WaitUntilReachDestination;
                _controller.Idle();
                yield return new WaitForSeconds(_wanderPauseInSeconds);
            }

        }
    }
}
