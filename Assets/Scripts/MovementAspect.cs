using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Korpo.Movement
{
    

public readonly partial struct MovementAspect : IAspect
{
    readonly RefRW<MovementComponent> movementComponent;
    readonly RefRW<LocalTransform> transform;

    public void Move(float deltaTime)
    {
        transform.ValueRW.Position = Vector3.MoveTowards(transform.ValueRW.Position,
            movementComponent.ValueRW.TargetPosition, movementComponent.ValueRW.Speed * deltaTime);

        
    }
    
    public void TestReachedPosition(RefRW<RandomComponent> component)
    {
        if (math.distance(transform.ValueRW.Position, movementComponent.ValueRW.TargetPosition) < 0.1f)
        {
            movementComponent.ValueRW.TargetPosition = GetRandomPosition(component);
        }
    }

    private float3 GetRandomPosition(RefRW<RandomComponent> random)
    {
        return new float3(random.ValueRW.Random.NextFloat(-10, 10), 0, random.ValueRW.Random.NextFloat(-10, 10));
    }

}
}

