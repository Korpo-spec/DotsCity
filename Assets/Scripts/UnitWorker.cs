using System;
using Korpo.Movement;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public class UnitWorker : MonoBehaviour
    {
        // public Entity WorkerEntity { get; private set; }
        // private EntityManager entityManager;
        // [SerializeField] private FetchTask OriginalTask;
        // public IUnitTask currentTask;
        // public Map map;
        //
        // private void Awake()
        // {
        //     //throw new NotImplementedException();
        //     Debug.Log("Awake called");
        // }
        //
        // void Start()
        // {
        //     Debug.Log("Starting worker init");
        //     currentTask = Instantiate(OriginalTask);
        //     OriginalTask = currentTask as FetchTask;
        //     entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        //
        //     // Create the worker entity in DOTS
        //     WorkerEntity = entityManager.CreateEntity();
        //     entityManager.AddComponentData(WorkerEntity, new MovementComponent());
        //     
        //     map = FindObjectOfType<Map>();
        //     
        //     DynamicBuffer<NodeBufferElement> nodes = entityManager.AddBuffer<NodeBufferElement>(WorkerEntity);;
        //         
        //     //DynamicBuffer<Node> nodes = new DynamicBuffer<Node>();
        //     for (int i = 0; i < map.map.m_Grid.GetLength(1); i++)
        //     {
        //         for (int j = 0; j < map.map.m_Grid.GetLength(0); j++)
        //         {
        //                 
        //             nodes.Add(new Node
        //             {
        //                 position = new int2(j, i),
        //                 index = i * (int)map.map.m_Grid.GetLength(1) + j,
        //                 gCost = float.MaxValue,
        //                 hCost = 0,
        //                 cameFromNodeIndex = 0,
        //                 isWalkable = !map.map.GetValue(j,i)
        //             });
        //         }
        //     }
        //     
        //     entityManager.AddComponentData(WorkerEntity, new PathFindingComponent
        //     {
        //         //nodes = nodes,
        //         size = new int2(map.map.m_Grid.GetLength(0), map.map.m_Grid.GetLength(1)),
        //         offset = map.map.m_Origin
        //     });
        //
        //     entityManager.AddComponentData(WorkerEntity, new PathfindingResult()
        //     {
        //         IsComplete = false
        //     });
        //     entityManager.AddBuffer<PathBufferElement>(WorkerEntity);
        //     Debug.Log("Setting up worker");
        //
        //
        // }
        //
        // public void AssignTask(IUnitTask task)
        // {
        //     currentTask = task;
        // }
        //
        // public void RequestPathfinding(Vector3 targetPosition)
        // {
        //     Debug.Log("Req pathfinding");
        //
        //     var ecb = new EntityCommandBuffer(Allocator.TempJob);
        //
        //     ecb.SetComponent(WorkerEntity, new MovementComponent
        //     {
        //         StartPosition = transform.position,
        //         TargetPosition = targetPosition,
        //         RequestingEntity = WorkerEntity
        //     });
        //
        //     ecb.AddComponent(WorkerEntity, new PathfindingResult
        //     {
        //         IsComplete = false
        //     });
        //
        //     ecb.Playback(entityManager);
        //     ecb.Dispose();
        // }
        //
        // public bool HasPathfindingResult
        // {
        //     get
        //     {
        //         return entityManager.HasComponent<PathfindingResult>(WorkerEntity) &&
        //                entityManager.GetComponentData<PathfindingResult>(WorkerEntity).IsComplete;
        //     }
        // }
        //
        // private int index = 0;
        // private float time = 0;
        // private bool runFirstTime = true;
        // private NativeArray<PathBufferElement> result;
        // public bool  FollowPath()
        // {
        //     
        //     if (runFirstTime)
        //     {
        //         result = entityManager.GetBuffer<PathBufferElement>(WorkerEntity).ToNativeArray(Allocator.Persistent);
        //         index = result.Length - 1;
        //         runFirstTime = false;
        //     }
        //     
        //     
        //     
        //     if (time > 1)
        //     {
        //         time = 0;
        //         index--;
        //         if (index < 0)
        //         {
        //             index = 10;
        //             time = 0;
        //             Debug.Log("movement done");
        //             entityManager.RemoveComponent<PathfindingResult>(WorkerEntity);
        //             runFirstTime = true;
        //             result.Dispose();
        //             return true;
        //         
        //         }
        //     }
        //     else
        //     {
        //         time += Time.deltaTime* 6;
        //         transform.position = Vector3.Lerp(transform.position, new Vector3(result[index].Value.x, 0, result[index].Value.y), time);
        //     }
        //     
        //     for (int i = 0; i < result.Length-1; i++)
        //     {
        //         Debug.DrawLine(new Vector3(result[i].Value.x,0,  result[i].Value.y), new Vector3(result[i+1].Value.x,0,  result[i+1].Value.y), Color.red);
        //     }
        //     
        //     
        //     // Cleanup the pathfinding result after use
        //     
        //     
        //     return false;
        // }
        //
        // void Update()
        // {
        //     // Execute the current task
        //     if (currentTask != null && !currentTask.IsComplete)
        //     {
        //         currentTask.Execute(this);
        //     }
        // }
    }
}