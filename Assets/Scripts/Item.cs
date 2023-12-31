using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using UnityEngine;

[CreateAssetMenu()]
public class BuildingCost : ScriptableObject
{
    public string name;
    public int stackSize;
    public Sprite icon;
    public FixedString32Bytes itemID => "baseGame:" + name.Trim().Replace(' ', '_');

    public Building building;
    

    private void OnValidate()
    {
        if (building)
        {
            building.buildingID = itemID;
        }
        
    }
}
