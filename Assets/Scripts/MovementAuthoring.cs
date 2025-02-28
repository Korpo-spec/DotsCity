using System.Collections;
using System.Collections.Generic;
using Korpo.Movement;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public float Speed;
    public Vector3 TargetPosition;

    class Baker: Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            DynamicBuffer<PathBufferElement> path = AddBuffer<PathBufferElement>(entity);
            AddComponent(entity, new MovementComponent
            {
                Speed = authoring.Speed,
                CurrentTargetPosition = authoring.TargetPosition
            });
        
        }
    }
}
