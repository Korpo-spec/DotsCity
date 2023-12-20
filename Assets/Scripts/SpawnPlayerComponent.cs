using Unity.Entities;

namespace Korpo.Spawning
{
    public struct SpawnPlayerComponent :IComponentData
    {
        public Entity EntityPrefab;
    }
}