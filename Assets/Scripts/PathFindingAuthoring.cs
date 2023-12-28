using System;
using Korpo.Movement;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using Korpo.Movement;
using Unity.Mathematics;

namespace DefaultNamespace
{
    public class PathFindingAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject GameObject;
        public Map map;

        

        class Baker: Baker<PathFindingAuthoring>
        {
            public override void Bake(PathFindingAuthoring authoring)
            {
                authoring.map = FindObjectOfType<Map>();
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                DynamicBuffer<NodeBufferElement> nodes = AddBuffer<NodeBufferElement>(entity);;
                
                //DynamicBuffer<Node> nodes = new DynamicBuffer<Node>();
                for (int i = 0; i < authoring.map.map.m_Grid.GetLength(1); i++)
                {
                    for (int j = 0; j < authoring.map.map.m_Grid.GetLength(0); j++)
                    {
                        
                        nodes.Add(new Node
                        {
                            position = new int2(j, i),
                            index = i * (int)authoring.map.map.m_Grid.GetLength(1) + j,
                            gCost = float.MaxValue,
                            hCost = 0,
                            cameFromNodeIndex = 0,
                            isWalkable = !authoring.map.map.GetValue(j,i)
                        });
                    }
                }
                //Debug.Log(nodes.Length);
                
                
                AddComponent(entity, new PathFindingComponent
                {
                    //nodes = nodes,
                    size = new int2(authoring.map.map.m_Grid.GetLength(0), authoring.map.map.m_Grid.GetLength(1)),
                    offset = authoring.map.map.m_Origin
                });
                
            }
        }
    }
}