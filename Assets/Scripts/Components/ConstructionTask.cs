using UnityEngine;
using Unity.Entities;

public struct ConstructionTask : IComponentData
{
    public ConstructionProgress Progress;
}
