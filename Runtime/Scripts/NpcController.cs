
using System;
using System.Collections.Generic;
using KBCore.Refs;

using UnityEngine;
using UnityEngine.AI;

namespace Bakery.NPC
{

    public class NpcController : ValidatedMonoBehaviour
    {
        public enum NpcPose
        {
            Idle,
            Walk,
            Run,
            Talk,
            Sprint,
            Sit,
            Stand,
            SitOnGround
        }
        [SerializeField] private GameObject _selectedIndicator;

        internal static Dictionary<NpcPose, int> PoseHashes = new()
        {
            { NpcPose.Idle, Animator.StringToHash("Idle") },
            { NpcPose.Walk,Animator.StringToHash("Walk")},
            { NpcPose.Run, Animator.StringToHash("Run") },
            { NpcPose.Talk, Animator.StringToHash("Talk") },
            { NpcPose.Sprint, Animator.StringToHash("Sprint") },
            { NpcPose.Sit, Animator.StringToHash("Sit") },
            { NpcPose.Stand, Animator.StringToHash("Stand") },
            { NpcPose.SitOnGround, Animator.StringToHash("SitOnGround") }
        };
        [SerializeField, Self] private NavMeshAgent _navMeshAgent;
        [SerializeField] private NpcStateMachine _stateMachine;
        [SerializeField, Child] private Animator _animator;
        [SerializeField, Self] private Collider _collision;

        [SerializeField] private GameObject _visuals;

        protected Vector3 _startPosition;
        protected bool _activated = false;
        internal NpcData NpcData => _data;
        [SerializeField] protected NpcData _data;
        protected Npc _npc;

        [SerializeField] private bool _initOnStart = false;
        [SerializeField] private float _searchRadius = 0.5f;
        private NpcPose _currentPose;

        public virtual void Select() => _selectedIndicator.SetActive(true);


        public virtual void Deselect() => _selectedIndicator.SetActive(false);

        protected virtual void Awake()
        {
            _stateMachine = new();
            _selectedIndicator.SetActive(false);
        }

        protected virtual void Start()
        {
            if (_initOnStart)
            {
                Init(transform.position, _data);
            }
        }

        public void Init(Vector3 startPosition, NpcData data)
        {
            _data = data;
            _npc = new Npc(_data);
            _startPosition = startPosition;
            _navMeshAgent.Warp(_startPosition);
        }

        void Update() => _stateMachine.Update();


        void FixedUpdate() => _stateMachine.FixedUpdate();

        protected void SetAnimation(NpcPose pose)
        {
            if (_currentPose == pose) return;
            _currentPose = pose;
            _animator.CrossFade(PoseHashes[pose], 0.1f);
        }

        public float DistanceToDestination => _navMeshAgent.remainingDistance;

        public WaitUntil WaitUntilReachDestination => new(() => _navMeshAgent.enabled && _navMeshAgent.remainingDistance < 0.2f);
        public bool IsPathFound => _navMeshAgent.pathPending == false && _navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete;

        public string CurrentStateText => _stateMachine.CurrentState.ToString();

        public bool IsActivated => _activated;

        public bool IsRunning => _navMeshAgent.speed == _data.RunSpeed;

        internal void WalkTo(NavMeshPath path)
        {
            _navMeshAgent.speed = _data.WalkSpeed;
            SetAnimation(NpcPose.Walk);
        }

        internal void SetPath(NavMeshPath path)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetPath(path);
        }

        internal void RunTo(NavMeshPath path)
        {
            _navMeshAgent.speed = _data.RunSpeed;
            SetAnimation(NpcPose.Run);
        }


        public void Deactivate()
        {
            _activated = false;
            _navMeshAgent.enabled = false;
            _collision.enabled = false;
            _animator.enabled = false;
            _visuals.SetActive(false);

        }

        public void Activate()
        {
            if (_activated) return;
            _activated = true;
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _collision.enabled = true;

            _visuals.SetActive(true);
            _animator.enabled = true;
        }


        internal void Idle()
        {
            _navMeshAgent.isStopped = true;
            SetAnimation(NpcPose.Idle);
        }

        public void GoTo(Vector3 position, bool isRunning)
        {
            _stateMachine.SetState(new GotoState(_stateMachine, this, position, isRunning));
        }

        public void Wander(Vector3 position, float range, float rate = 2f)
        {
            _stateMachine.SetState(new WanderState(_stateMachine, this, position, range, rate));
        }

        internal bool FindPath(Vector3 destination, out NavMeshPath path)
        {
            path = new NavMeshPath();
            if (!NavMesh.SamplePosition(destination, out NavMeshHit hit, _searchRadius, NavMesh.AllAreas))
                return false;
            if (!_navMeshAgent.CalculatePath(hit.position, path)) return false;
            return true;
        }

        internal void Follow(Transform transform)
        {
            _stateMachine.SetState(new FollowState(_stateMachine, this, transform));
        }

        internal void Follow(NpcController npcController3)
        {
            _stateMachine.SetState(new FollowState(_stateMachine, this, npcController3));
        }
    }
}