using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Korpo.Spawning;
using Unity.Entities;
using UnityEngine;

public partial struct SpawnPlayerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        query = state.GetEntityQuery(typeof(MovementComponent));
        state.RequireForUpdate<SpawnPlayerComponent>();
    }

    private EntityQuery query;
    
    public void OnUpdate(ref SystemState state)
    {
        
        int amount = 1000;
        
        if (query.CalculateEntityCount() > 0)
        {
            return;
        }
        SpawnPlayerComponent spawnPlayerComponent = SystemAPI.GetSingleton<SpawnPlayerComponent>();
        EntityCommandBuffer entityCommandBuffer = SystemAPI
            .GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        for (int i = 0; i < amount; i++)
        {
            entityCommandBuffer.Instantiate(spawnPlayerComponent.EntityPrefab);

        }
    }
}
