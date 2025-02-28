using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TaskManager : MonoBehaviour
    {
        public List<UnitWorker> workers = new List<UnitWorker>();
        private Queue<IUnitTask> taskQueue = new Queue<IUnitTask>();

        public void AddTask(IUnitTask task)
        {
            taskQueue.Enqueue(task);
        }

        void Update()
        {
            // // Assign tasks to idle workers
            // foreach (var worker in workers)
            // {
            //     if (worker.HasPathfindingResult) continue;
            //
            //     if (taskQueue.Count > 0 && worker.currentTask == null)
            //     {
            //         var task = taskQueue.Dequeue();
            //         worker.AssignTask(task);
            //     }
            // }
        }
    }
}