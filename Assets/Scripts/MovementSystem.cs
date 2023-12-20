using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Korpo.Movement;
using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Jobs;
[BurstCompile]
public partial struct MovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        RefRW<RandomComponent> random = SystemAPI.GetSingletonRW<RandomComponent>();
        /*foreach (var movementAspect in SystemAPI.Query<MovementAspect>())
        {
            
            movementAspect.Move(SystemAPI.Time.DeltaTime);
        }
        */

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
    public void Execute(MovementAspect movementAspect)
    {
        
        movementAspect.TestReachedPosition(randomComponent);
    }
    
}
