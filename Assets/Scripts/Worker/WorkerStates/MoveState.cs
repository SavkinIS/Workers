using System.Collections;
using UnityEngine;

namespace WorkerStates
{
    public class MoveState : IWorkerState
    {
        private readonly Mover _mover;
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private Coroutine _moveCoroutine;
        private bool _moveToBase;
        private bool _isActive = true;
        private readonly Transform _storageUnloadZone;

        public MoveState(WorkerStateMachine stateMachine, Mover mover, Transform storageUnloadZone, Worker worker)
        {
            _mover = mover;
            _storageUnloadZone = storageUnloadZone;
            _stateMachine = stateMachine;
            _worker = worker;
        }
        
        public void Enter()
        {
            Transform target;

            _moveToBase = false;
            
            if (_worker.HasResource)
            {
                target = _storageUnloadZone;
                _moveToBase =  true;
            }
            else
            {
                target = _worker.TargetResource.Transform;
            }
            
            _mover.SetTarget(target, _worker.HasResource);
            _mover.DestinationReached += DestinationReached;
            _moveCoroutine = _worker.StartCoroutine(MoveCoroutine());
        }
        
        private IEnumerator MoveCoroutine()
        {
            while (_isActive)
            {
                _mover.Move();
                yield return null;
            }
        }

        public void Exit()
        {
            if (_moveCoroutine  != null)
                _mover.StopCoroutine(_moveCoroutine);
            
            _mover.DestinationReached -= DestinationReached;
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