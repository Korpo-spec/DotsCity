using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;


public class RandomAuthoring : MonoBehaviour
{
    
}

public class RandomBaker: Baker<RandomAuthoring>
{
    public override void Bake(RandomAuthoring authoring)
    {
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new RandomComponent
        {
            Random = new Unity.Mathematics.Random((uint)Random.Range(0, int.MaxValue))
        });
    
    }
}
