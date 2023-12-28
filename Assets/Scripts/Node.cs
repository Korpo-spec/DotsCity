using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Korpo.Movement
{
    
    public struct Node
    {
        
        public int2 position;
        public int index;
        
        public float gCost;
        public float hCost;
        public float fCost;
        
        public bool isWalkable;
        
        public int cameFromNodeIndex;


        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}