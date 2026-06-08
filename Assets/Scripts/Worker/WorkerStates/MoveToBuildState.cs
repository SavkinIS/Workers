using System;
using System.Collections;
using UnityEngine;

namespace WorkerStates
{
    public class MoveToBuildState : IWorkerState
    {
        private readonly Mover _mover;
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private Coroutine _moveCoroutine;
        private bool _isActive = true;
        private readonly Transform _flagTransform;
        private readonly Action<Worker> _newStoragePositionReached;

        public MoveToBuildState(WorkerStateMachine stateMachine, Mover mover, Transform flagTransform, Worker worker, Action<Worker> newStoragePositionReached)
        {
            _mover = mover;
            _flagTransform = flagTransform;
            _stateMachine = stateMachine;
            _worker = worker;
            _newStoragePositionReached = newStoragePositionReached;
        }
        
        public void Enter()
        {
            _mover.SetTarget(_flagTransform, _worker.HasResource);
            _mover.DestinationReached += DestinationReached;
            _moveCoroutine = _worker.StartCoroutine(MoveCoroutine());
        }
      
        public void Exit()
        {
            if (_moveCoroutine  != null)
                _worker.StopCoroutine(_moveCoroutine);
            
            _mover.DestinationReached -= DestinationReached;
        }

        private void DestinationReached()
        {
            _stateMachine.SetState(typeof(IdleState));
            _newStoragePositionReached?.Invoke( _worker);
        }
          
        private IEnumerator MoveCoroutine()
        {
            while (_isActive)
            {
                _mover.Move();
                yield return null;
            }
        }
    }
}