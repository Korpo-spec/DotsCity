using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Korpo.Movement
{
    public struct PathFindingComponent : IComponentData
    {
        public int2 size;

        public float3 offset;
        
        public int2 targetpos;

        //[NonSerialized]public DynamicBuffer<Node> nodes;
        

        public readonly int GetNodeIndex(float2 pos)
        {
            int intPos = (int)(pos.y * size.x);
            intPos += (int)pos.x;
            return intPos;
        }
    }
}