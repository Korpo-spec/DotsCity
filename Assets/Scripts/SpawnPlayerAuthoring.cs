using Korpo.Spawning;
using Unity.Entities;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpawnPlayerAuthoring : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        
        class Baker: Baker<SpawnPlayerAuthoring>
        {
            public override void Bake(SpawnPlayerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SpawnPlayerComponent
                {
                    
                    EntityPrefab = GetEntity(authoring.PlayerPrefab)
                });
            
            }
        }
    }
}