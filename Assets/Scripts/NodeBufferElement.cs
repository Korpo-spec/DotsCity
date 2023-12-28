using Korpo.Movement;
using Unity.Entities;

namespace DefaultNamespace
{
    [InternalBufferCapacity(8)]
    public struct NodeBufferElement: IBufferElementData
    {
        public Node Value;
        
        public static implicit operator Node(NodeBufferElement e) { return e.Value; }
        public static implicit operator NodeBufferElement(Node e) { return new NodeBufferElement() { Value = e }; }
    }
}