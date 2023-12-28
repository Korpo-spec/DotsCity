using Unity.Entities;
using Unity.Mathematics;

namespace Korpo.Movement
{
    public struct PathBufferElement : IBufferElementData
    {
        public int2 Value;
        
        public static implicit operator int2(PathBufferElement e) { return e.Value; }
        public static implicit operator PathBufferElement(int2 e) { return new PathBufferElement() { Value = e }; }
        
    }
}