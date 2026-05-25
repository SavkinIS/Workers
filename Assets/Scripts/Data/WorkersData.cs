using System.Collections.Generic;

public class WorkersData
{
    private readonly List<Worker> _totalWorkers = new List<Worker>();
    private readonly Queue<Worker> _freeWorker = new Queue<Worker>();

    public WorkersData(List<Worker> workers)
    {
        _totalWorkers.AddRange(workers);

        foreach (var worker in workers)
        {
            _freeWorker.Enqueue(worker);
        }
    }

    public bool TryGetFreeWorker(out Worker worker)
    {
        worker = null;

        if (_freeWorker.Count > 0)
        {
            worker = _freeWorker.Dequeue();
            return true;
        }

        return false;
    }

    public void AddFreeWorker(Worker worker)
    {
        if (_freeWorker.Contains(worker) == false)
            _freeWorker.Enqueue(worker);
    }
}