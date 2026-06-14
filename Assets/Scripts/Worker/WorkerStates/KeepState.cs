using System.Collections;
using UnityEngine;

namespace WorkerStates
{
    public class KeepState : IWorkerState
    {
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private readonly ReplaceAnimation _replaceAnimation;
        private readonly float _waitingDelay = 0.5f;
        private readonly WaitForSeconds _waitingTime;

        public KeepState(WorkerStateMachine stateMachine, Worker worker)
        {
            _stateMachine = stateMachine;
            _worker = worker;
            _replaceAnimation = new ReplaceAnimation(worker.HandPlace);
            _waitingTime = new WaitForSeconds(_waitingDelay);
        }
        
        public void Enter()
        {
            _worker.TargetResource.DisablePhysics();
            _worker.StartCoroutine(KeepCoroutine());
        }

        private IEnumerator KeepCoroutine()
        {
            yield return _waitingTime;
            _replaceAnimation.Replace(_worker.TargetResource.Transform, ResourceKeeped);
        }

        private void ResourceKeeped()
        {
            _worker.KeepResource();
        }

        public void Exit()
        {
        }
    }
}