using Korpo.Movement;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

partial struct ConstructionSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (constructionTask,movementComponent,unitInventory) in 
                 SystemAPI.Query<RefRW<ConstructionTask>, RefRW<MovementComponent>, RefRW<UnitInventory>>()) 
        {
            if (movementComponent.ValueRO.reachedPosition == false)
            {
                continue;
            }
            Debug.Log("Unit is constructing...");
            //TODO: FIX THIS
            BuildingMaterial material = constructionTask.ValueRO.Progress.MaterialsNeeded[0];

            if (unitInventory.ValueRO.items.Length > 0)
            {
                Debug.Log("Unit has materials");
                for (int i = 0; i < unitInventory.ValueRO.items.Length; i++)
                {
                    if (unitInventory.ValueRO.items[i].ID == material.ID)
                    {
                        Debug.Log("Unit has the right materials");
                        movementComponent.ValueRW.TargetPosition = SystemAPI.GetComponentRO<LocalTransform>(constructionTask.ValueRO.Progress.Building).ValueRO.Position.ToInt2();
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Unit has no materials");
                
            }
            
            
            //var storage = SystemAPI.Query<RefRW<StorageTag>>();

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
