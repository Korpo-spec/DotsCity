using Unity.Collections;
using Unity.Entities;

public struct ConstructionProgress : IComponentData
{
    public Entity Building;
    public float Progress => MaterialsNeeded.Length == 0 ? 1 : 0;
    public NativeArray<BuildingMaterial> MaterialsNeeded;
    
    ConstructionProgress(NativeArray<BuildingMaterial> materialsNeeded)
    {
        Building = Entity.Null;
        MaterialsNeeded = materialsNeeded;
    }
}
