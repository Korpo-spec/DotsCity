using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    // [CreateAssetMenu(fileName = "FetchTask", menuName = "Tasks/FetchTask")]
    // public class FetchTask : ScriptableObject, IUnitTask
    // {
    //     // [SerializeField] private Vector3 targetPosition;
    //     // public bool pathRequested = false;
    //     // private bool m_IsComplete;
    //     //
    //     // public FetchTask(Vector3 targetPosition)
    //     // {
    //     //     this.targetPosition = targetPosition;
    //     // }
    //     //
    //     // public void Execute(UnitWorker worker)
    //     // {
    //     //     
    //     //     if (!pathRequested)
    //     //     {
    //     //         Debug.Log("Requesting pathfinding");
    //     //         // Request pathfinding in DOTS
    //     //         worker.RequestPathfinding(targetPosition);
    //     //         pathRequested = true;
    //     //     }
    //     //     else if (worker.HasPathfindingResult)
    //     //     {
    //     //         Debug.Log("Has pathfinding result");
    //     //         // Use the pathfinding result to move the worker
    //     //         if (worker.FollowPath())
    //     //         {
    //     //             m_IsComplete = false; // Mark the task as complete once movement is done
    //     //             pathRequested = false;
    //     //             targetPosition = new Vector3().RandomVec3()*10 + Vector3.one;
    //     //         }
    //     //         
    //     //        
    //     //     }
    //     // }
    //     //
    //     // bool IUnitTask.IsComplete
    //     // {
    //     //     get => m_IsComplete;
    //     //     set => m_IsComplete = value;
    //     // }
    // }
}