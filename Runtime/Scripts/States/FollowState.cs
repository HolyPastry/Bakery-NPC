using System;
using System.Collections;
using UnityEngine;

namespace Bakery.NPC
{
    internal class FollowState : NpcState
    {

        private Transform _transform;
        private Coroutine _routine;
        private NpcController npcController3;
        private NpcController _followedNpc;

        public const float WALK_DISTANCE = 5f;
        public const float PERSONAL_SPACE = 1.5f;

        public FollowState(NpcStateMachine stateMachine, NpcController npcController, Transform transform)
            : base(stateMachine, npcController)
        {
            this._transform = transform;
        }

        public FollowState(NpcStateMachine machine, NpcController controller, NpcController followedNpc) : base(machine, controller)
        {
            _followedNpc = followedNpc;
        }

        internal override void Enter()
        {
            if (!_controller.IsActivated)
                _controller.Activate();
            if (_followedNpc != null)
                _routine = _controller.StartCoroutine(FollowNpcRoutine(_followedNpc));
            else
                _routine = _controller.StartCoroutine(FollowRoutine());
        }

        private IEnumerator FollowNpcRoutine(NpcController followedNpc)
        {

            while (true)
            {


                if (!_controller.FindPath(followedNpc.transform.position, out var path))
                {
                    yield return null;
                    continue;
                }

                if (!_controller.IsPathFound)
                {
                    yield return null;
                    continue;
                }

                _controller.SetPath(path);

                if (_controller.DistanceToDestination <= PERSONAL_SPACE)
                {
                    _controller.Idle();
                }
                else if (_controller.DistanceToDestination <= WALK_DISTANCE)
                {
                    _controller.WalkTo(path);
                }
                else
                {
                    _controller.RunTo(path);
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator FollowRoutine()
        {
            while (true)
            {
                if (!_controller.FindPath(_transform.position, out var path))
                    yield return null;

                if (!_controller.IsPathFound)
                    yield return null;

                _controller.WalkTo(path);
                yield return _controller.WaitUntilReachDestination;
            }
        }

        internal override void Exit()
        {
            if (_routine != null)
                _controller.StopCoroutine(_routine);

        }

    }
}