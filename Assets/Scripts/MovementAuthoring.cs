using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
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
            AddComponent(entity, new MovementComponent
            {
                Speed = authoring.Speed,
                TargetPosition = authoring.TargetPosition
            });
        
        }
    }
}
