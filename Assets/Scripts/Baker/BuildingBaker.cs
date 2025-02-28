using Unity.Collections;
using Unity.Entities;
using UnityEngine;

class BuildingBaker : MonoBehaviour
{
    [SerializeField] public bool underConstruction;
}

class BuildingBakerBaker : Baker<BuildingBaker>
{
    public override void Bake(BuildingBaker authoring)
    {
        if (authoring.underConstruction)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            var materials = new NativeArray<BuildingMaterial>(1, Allocator.Temp);
            materials[0] = new BuildingMaterial
            {
                ID = 0,
                Quantity = 10
            };
            AddComponent(entity, new ConstructionProgress()
            {
                Building = entity,
                MaterialsNeeded = materials
            });
        }
        else
        {
            var materials = new NativeArray<BuildingMaterial>(1, Allocator.Temp);
            materials[0] = new BuildingMaterial
            {
                ID = 0,
                Quantity = 10
            };
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new StorageTag()
            {
                MaterialsInStorage = materials
            });
        }
        
    }
}
