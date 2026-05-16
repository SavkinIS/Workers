namespace WorkerStates
{
    public class PutState : IWorkerState
    {
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private readonly ReplaceAnimation _replaceAnimation;
        private readonly Storage _storage;

        public PutState(WorkerStateMachine stateMachine, Worker worker, Storage storage)
        {
            _stateMachine = stateMachine;
            _worker = worker;
            _replaceAnimation = new ReplaceAnimation(storage.PutTarget);
            _storage = storage;
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
            
            _storage.Claim(_worker.Resource);
            _worker.PutResource();
            _stateMachine.SetState(typeof(IdleState));
            _worker.CompleteWork();
        }

        public void Exit()
        {
        }
    }
}