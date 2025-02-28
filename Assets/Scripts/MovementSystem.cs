using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Korpo.Movement;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
using Unity.Mathematics;

[BurstCompile]
public partial struct MovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        
        //Debug.Log(buffer[23*50+7].Value.isWalkable);
        RefRW<RandomComponent> random = SystemAPI.GetSingletonRW<RandomComponent>();

        float deltaTime = SystemAPI.Time.DeltaTime;
        JobHandle jobHandle = new MovementJob()
        {
            deltaTime = deltaTime
        }.ScheduleParallel(state.Dependency);
        
        jobHandle.Complete();
        
        new TestReachedPositionJob()
        {
            randomComponent = random
        }.Run();

        
        new PathFind()
        {
            
        }.ScheduleParallel();
        
        
    }
}

[BurstCompile]
public partial struct MovementJob : IJobEntity
{
    public float deltaTime;
    [BurstCompile]
    public void Execute(MovementAspect movementAspect)
    {
        
        movementAspect.Move(deltaTime);
    }
    
}
[BurstCompile]
public partial struct TestReachedPositionJob : IJobEntity
{
    [NativeDisableUnsafePtrRestriction]
    public RefRW<RandomComponent> randomComponent;
    [BurstCompile]
    public void Execute(PathFindingAspect pathFindingAspect)
    {

        if (pathFindingAspect.movementAspect.TestReachedPosition(randomComponent))
        {
            bool validPos = false;
            while (!validPos)
            {
                pathFindingAspect.movementAspect.movementComponent.ValueRW.TargetPosition = new int2(randomComponent.ValueRW.Random.NextInt(2, 48), randomComponent.ValueRW.Random.NextInt(2, 48));
                validPos = pathFindingAspect.nodesBuffer[pathFindingAspect.pathfindingComponent.ValueRW.GetNodeIndex(pathFindingAspect.movementAspect.movementComponent.ValueRW.TargetPosition)].Value.isWalkable;
            }
        }

        
    }
    
}


public partial struct PathFind : IJobEntity
{
    
    
    public void Execute(PathFindingAspect pathFindingAspect)
    {
        if (!pathFindingAspect.movementAspect.movementComponent.ValueRO.reachedPosition)
        {
            return;
        }

        
        int2 offset = new int2((int)pathFindingAspect.pathfindingComponent.ValueRO.offset.x, (int)pathFindingAspect.pathfindingComponent.ValueRO.offset.y);
        int2 startpos = new int2((int)pathFindingAspect.movementAspect.transform.ValueRO.Position.x, (int)pathFindingAspect.movementAspect.transform.ValueRO.Position.z);
        pathFindingAspect.movementAspect.movementComponent.ValueRW.reachedPosition = false;
        pathFindingAspect.FindPath(startpos- offset, pathFindingAspect.movementAspect.movementComponent.ValueRO.TargetPosition);
    }
}


