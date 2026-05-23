using UnityEngine;

namespace WorkerStates
{
    public class PutState : IWorkerState
    {
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private readonly ReplaceAnimation _replaceAnimation;

        public PutState(WorkerStateMachine stateMachine, Worker worker, Transform storagePutTarget)
        {
            _stateMachine = stateMachine;
            _worker = worker;
            _replaceAnimation = new ReplaceAnimation(storagePutTarget);
        }
        
        public void Enter()
        {
            if (_worker.Resource == null) 
                return;
            
            _replaceAnimation.Raplace(_worker.Resource.Transform, ResourcePutted);
        }

        private void ResourcePutted()
        {
            if (_worker.Resource == null)
                return;
            
            _worker.PutResource();
            _stateMachine.SetState(typeof(IdleState));
        }

        public void Exit()
        {
        }
    }
}