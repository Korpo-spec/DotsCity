using System.Linq;
using Korpo.Movement;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace DefaultNamespace
{
    public readonly partial struct PathFindingAspect : IAspect
    {
        private readonly Entity entity;
        public readonly DynamicBuffer<NodeBufferElement> nodesBuffer;
        private readonly DynamicBuffer<PathBufferElement> path;

        public readonly RefRW<PathFindingComponent> pathfindingComponent;
        public readonly MovementAspect movementAspect;
        public void FindPath(int2 startPosition, int2 endPosition)
        {

            DynamicBuffer<Node> nodes = nodesBuffer.Reinterpret<Node>();

            for (int i = 0; i < nodes.Length; i++)
            {
                Node prevNode = nodes[i];
                
                prevNode.gCost = float.MaxValue;
                prevNode.hCost = 0;
                prevNode.cameFromNodeIndex = 0;
                nodes[i] = prevNode;
            }
            
            var startnodeIndex = pathfindingComponent.ValueRO.GetNodeIndex(startPosition);
            var endNodeIndex = pathfindingComponent.ValueRO.GetNodeIndex(endPosition);
            ////Debug.Log(endNodeIndex + " " + endPosition);
            var startNode = nodes[startnodeIndex];
            var endNode = nodes[endNodeIndex];
            var openList = new NativeList<int>(Allocator.Temp);
            var closedList = new NativeList<int>(Allocator.Temp);
            bool foundPath = false;
            nodes.RemoveAt(startnodeIndex);
            startNode.gCost = 0;
            nodes.Insert(startnodeIndex, startNode);
            openList.Add(startNode.index);
            while (openList.Length > 0)
            {
                var currentNodeIndex = GetLowestCostFNodeIndex(openList);
                var currentNode = nodes[currentNodeIndex];
                //Debug.Log(currentNode.cameFromNodeIndex);
                if (currentNode.index == endNode.index)
                {
                    //Debug.Log(currentNode.index + " " + endNode.index);
                    //Debug.Log("FoundPath");
                    foundPath = true;
                    break;
                }
                for (int i = 0; i < openList.Length; i++)
                {
                    if (openList[i] == currentNodeIndex)
                    {
                        openList.RemoveAtSwapBack(i);
                        break;
                    }
                }
                closedList.Add(currentNodeIndex);
                //Debug.Log("GettingNeighbours");
                for (int i = 0; i < 8; i++)
                {
                    var neighbourPos = GetNeighbourPos(currentNode.position, i);
                    if (!IsPositionInsideGrid(neighbourPos, pathfindingComponent.ValueRO.size))
                    {
                        continue;
                    }
                    //Debug.Log("IsInsideGrid");
                    var neighbourNode = nodes[pathfindingComponent.ValueRO.GetNodeIndex(neighbourPos)];
                    if (!neighbourNode.isWalkable || closedList.Contains(neighbourNode.index))
                    {
                        //Debug.Log(neighbourNode.isWalkable + " " + closedList.Contains(neighbourNode.index));
                        continue;
                    }
                    //Debug.Log("IsWalkableAndNotInClosedList");
                    var tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode.position, neighbourNode.position);
                    //Debug.Log(tentativeGCost +" gCost" + currentNode.gCost + " " + CalculateDistanceCost(currentNode.position, neighbourNode.position));
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNodeIndex = currentNodeIndex;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode.position, endNode.position);
                        neighbourNode.CalculateFCost();
                        //Debug.Log("Adding to openList");
                        nodes.RemoveAt(neighbourNode.index);
                        nodes.Insert(neighbourNode.index, neighbourNode);
                        if (!openList.Contains(neighbourNode.index))
                        {
                            openList.Add(neighbourNode.index);
                        }
                    }
                }
            }

            if (foundPath)
            {
                
            }
            
            var calcPath = CalculatePath(nodes[endNodeIndex], startNode, nodes);
            path.Clear();
            path.AddRange(calcPath.AsArray());
            movementAspect.movementComponent.ValueRW.reachedPosition = false;
            calcPath.Dispose();
            //Debug.Log(calcPath.Length);
            
            openList.Dispose();
            closedList.Dispose();
            
        }

        private NativeList<PathBufferElement> CalculatePath(Node endNode, Node startNode, DynamicBuffer<Node> nodes)
        {
            
            //Debug.Log(endNode.index + " " + startNode.index);
            //Debug.Log(endNode.cameFromNodeIndex + " " + startNode.index);
            if (endNode.cameFromNodeIndex == startNode.index)
            {
                //Debug.Log("EndEarly");
                return new NativeList<PathBufferElement>(Allocator.Temp);
            }
            var path = new NativeList<PathBufferElement>(Allocator.Temp);
            int2 offset = new int2((int)pathfindingComponent.ValueRO.offset.x, (int)pathfindingComponent.ValueRO.offset.y);
            path.Add(endNode.position+ offset);
            var currentNode = endNode;
            int safety = 0;
            while (currentNode.cameFromNodeIndex != startNode.index && safety < 2500)
            {
                //Debug.Log(currentNode.position);
                var cameFromNode = nodes[currentNode.cameFromNodeIndex];
                path.Add(cameFromNode.position+ offset);
                currentNode = cameFromNode;
                safety++;
            }

            if (safety >= 2499)
            {
                Debug.LogError("InfiniteLoop for startNode: " + startNode.position + " endNode: " + endNode.position + " " + endNode.cameFromNodeIndex + " " + startNode.index);
                return new NativeList<PathBufferElement>(Allocator.Temp);
            }

            

            
            return path;
        }

        private float CalculateDistanceCost(int2 currentNodePosition, int2 neighbourNodePosition)
        {
            var xDistance = math.abs(currentNodePosition.x - neighbourNodePosition.x);
            var yDistance = math.abs(currentNodePosition.y - neighbourNodePosition.y);
            var remaining = math.abs(xDistance - yDistance);
            return 13 * math.min(xDistance, yDistance) + 10 * remaining;
        }

        private bool IsPositionInsideGrid(int2 neighbourPos, int2 pathfindingComponentSize)
        {
            if (neighbourPos.x >= 0 && neighbourPos.y >= 0 && neighbourPos.x < pathfindingComponentSize.x && neighbourPos.y < pathfindingComponentSize.y)
            {
                return true;
            }
            
            return false;
        }

        private int2 GetNeighbourPos(int2 currentNodePosition, int i)
        {
            switch (i)
            {
                case 0:
                    return new int2(currentNodePosition.x - 1, currentNodePosition.y - 1);
                case 1:
                    return new int2(currentNodePosition.x, currentNodePosition.y - 1);
                case 2:
                    return new int2(currentNodePosition.x + 1, currentNodePosition.y - 1);
                case 3:
                    return new int2(currentNodePosition.x - 1, currentNodePosition.y);
                case 4:
                    return new int2(currentNodePosition.x + 1, currentNodePosition.y);
                case 5:
                    return new int2(currentNodePosition.x - 1, currentNodePosition.y + 1);
                case 6:
                    return new int2(currentNodePosition.x, currentNodePosition.y + 1);
                case 7:
                    return new int2(currentNodePosition.x + 1, currentNodePosition.y + 1);
                default:
                    return new int2();
            }
        }

        

        private int GetLowestCostFNodeIndex(NativeList<int> openList)
        {
            var lowestCostFNodeIndex = openList[0];
            for (int i = 0; i < openList.Length; i++)
            {
                if (nodesBuffer[openList[i]].Value.fCost < nodesBuffer[lowestCostFNodeIndex].Value.fCost)
                {
                    lowestCostFNodeIndex = openList[i];
                }
            }
            return lowestCostFNodeIndex;
        }
    }
}