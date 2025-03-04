using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public float Speed;
    public int2 TargetPosition;
    public float3 CurrentTargetPosition;
    public bool reachedPosition;
}

