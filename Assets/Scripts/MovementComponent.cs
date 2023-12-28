using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public float Speed;
    public float3 TargetPosition;
    public bool reachedPosition;
}

