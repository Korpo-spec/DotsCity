using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public float Speed;
    public Vector3 TargetPosition;
}

