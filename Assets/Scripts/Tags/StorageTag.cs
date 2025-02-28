using Unity.Collections;
using Unity.Entities;

public struct StorageTag : IComponentData
{
    public NativeArray<BuildingMaterial> MaterialsInStorage;
}
