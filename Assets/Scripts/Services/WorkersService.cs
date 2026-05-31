using System.Collections.Generic;

public class WorkersService
{
    private readonly Queue<Worker> _freeWorker = new Queue<Worker>();

    public WorkersService(List<Worker> workers)
    {
        foreach (var worker in workers)
        {
            _freeWorker.Enqueue(worker);
        }
    }

    public bool HasFreeWorkers  => _freeWorker.Count > 0;
    public int FreeWorkers => _freeWorker.Count;

    public Worker GetFreeWorker()
    {
        if (_freeWorker.Count > 0)
        {
            return _freeWorker.Dequeue();
        }
        
        return null;
    }

    public void AddFreeWorker(Worker worker)
    {
        if (_freeWorker.Contains(worker) == false)
            _freeWorker.Enqueue(worker);
    }
}