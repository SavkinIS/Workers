using System.Collections;
using UnityEngine;

namespace WorkerStates
{
    public class MoveState : IWorkerState
    {
        private readonly Mover _mover;
        private readonly Worker _worker;
        private Coroutine _moveCoroutine;
        private bool _isActive = true;

        public MoveState(Mover mover, Worker worker)
        {
            _mover = mover;
            _worker = worker;
        }
        
        public void Enter()
        {
            _mover.SetTarget(_worker.Task.Target, _worker.HasResource);
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
            _worker.Task.ExecuteCompletion();
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