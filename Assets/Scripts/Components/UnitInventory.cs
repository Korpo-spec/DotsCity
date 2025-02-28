using Unity.Collections;
using Unity.Entities;

public struct UnitInventory : IComponentData
{
    public int maxCapacity;
    public int currentCapacity => items.Length;
    public NativeArray<BuildingMaterial> items;
}
