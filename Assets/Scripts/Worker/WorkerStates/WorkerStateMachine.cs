using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorkerStates
{
    public class WorkerStateMachine : IDisposable
    {
        private readonly Dictionary<Type, IWorkerState> _states;
        private IWorkerState _currentState;

        public WorkerStateMachine( Mover mover, Transform storageUnloadZone,Transform storagePutTarget, Worker worker)
        {
            _states = new Dictionary<Type, IWorkerState>()
            {
                { typeof(IdleState), new IdleState() },
                { typeof(MoveState), new MoveState(this, mover, storageUnloadZone, worker) },
                { typeof(KeepState), new KeepState(this, worker) },
                { typeof(PutState), new PutState(this, worker, storagePutTarget) },
                
            };
            
            SetState(typeof(IdleState));
        }

        public void SetState(Type type, Transform target = null)
        {
            if (_states.TryGetValue(type,  out var state))
            {
                if (_currentState == state || state == null)
                    return;
                
                _currentState?.Exit();
                _currentState = state;
                _currentState.Enter();
            }
        }
        
        public void Dispose()
        {
            _currentState?.Exit();
            _states.Clear();
        }
    }
}