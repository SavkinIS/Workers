using System.Collections;
using UnityEngine;

namespace WorkerStates
{
    public class MoveState : IWorkerState
    {
        private readonly TargetMover _targetMover;
        private readonly Storage _storage;
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private Coroutine _moveCoroutine;
        private bool _moveToBase;

        public MoveState(WorkerStateMachine stateMachine, TargetMover targetMover, Storage storage, Worker worker)
        {
            _targetMover = targetMover;
            _storage = storage;
            _stateMachine = stateMachine;
            _worker = worker;
        }
        
        public void Enter()
        {
            Transform target;

            _moveToBase = false;
            
            if (_worker.HasResource)
            {
                target = _storage.InputZone;
                _moveToBase =  true;
            }
            else
            {
                target = _worker.TargetResource.Transform;
            }
            
            _targetMover.SetTarget(target);
            _targetMover.DestinationReached += DestinationReached;
            _moveCoroutine = _worker.StartCoroutine(MoveCoroutine());
        }
        
        private IEnumerator MoveCoroutine()
        {
            while (true)
            {
                _targetMover.Move();
                yield return null;
            }
        }

        public void Exit()
        {
            if (_moveCoroutine  != null)
                _targetMover.StopCoroutine(_moveCoroutine);
            
            _targetMover.DestinationReached -= DestinationReached;
        }

        private void DestinationReached()
        {
            if (_moveToBase)
                _stateMachine.SetState(typeof(PutState));
            else
                _stateMachine.SetState(typeof(KeepState));
        }
    }
}