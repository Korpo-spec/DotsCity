using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Korpo.Movement
{
    

public readonly partial struct MovementAspect : IAspect
{
    public readonly RefRW<MovementComponent> movementComponent;
    public readonly RefRW<LocalTransform> transform;
    private readonly DynamicBuffer<PathBufferElement> path;


    public void Move(float deltaTime)
    {
        transform.ValueRW.Position = Vector3.MoveTowards(transform.ValueRW.Position,
            movementComponent.ValueRW.TargetPosition, movementComponent.ValueRW.Speed * deltaTime);

        
    }
    
    public bool TestReachedPosition(RefRW<RandomComponent> component)
    {
        /*
        for (int i = 0; i < path.Length-1; i++)
        {
            Debug.DrawLine(new Vector3(path[i].Value.x,0,  path[i].Value.y), new Vector3(path[i+1].Value.x,0,  path[i+1].Value.y), Color.red);
        }'*/
        
        if (math.distance(transform.ValueRW.Position, movementComponent.ValueRW.TargetPosition) < 0.1f)
        {
            //movementComponent.ValueRW.TargetPosition = GetRandomPosition(component);
            if (path.Length > 0)
            {
                var nextPosition = path[path.Length - 1].Value;
                movementComponent.ValueRW.TargetPosition = new float3(nextPosition.x, 0, nextPosition.y);
                path.RemoveAt(path.Length - 1);
            }
            else
            {
                movementComponent.ValueRW.reachedPosition = true;
                return true;
            }

            return false;
        }

        return false;
    }

    private float3 GetRandomPosition(RefRW<RandomComponent> random)
    {
        return new float3(random.ValueRW.Random.NextFloat(-10, 10), 0, random.ValueRW.Random.NextFloat(-10, 10));
    }

}
}

