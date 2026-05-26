namespace WorkerStates
{
    public class KeepState : IWorkerState
    {
        private readonly WorkerStateMachine _stateMachine;
        private readonly Worker _worker;
        private readonly ReplaceAnimation _replaceAnimation;

        public KeepState(WorkerStateMachine stateMachine, Worker worker)
        {
            _stateMachine = stateMachine;
            _worker = worker;
            _replaceAnimation = new ReplaceAnimation(worker.HandPlace);
        }
        
        public void Enter()
        {
            _worker.TargetResource.DisablePhysics();
            _replaceAnimation.Replace(_worker.TargetResource.Transform, ResourceKeeped);
        }

        private void ResourceKeeped()
        {
            _worker.KeepResource();
            _stateMachine.SetState(typeof(MoveState));
        }

        public void Exit()
        {
        }
    }
}