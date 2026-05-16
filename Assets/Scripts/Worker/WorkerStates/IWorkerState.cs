using System.Diagnostics;
using UnityEngine;

namespace WorkerStates
{
    public interface IWorkerState
    {
        void Enter();
        void Exit();
    }
}