using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Collections;
using UnityEngine;

[ExecuteAlways]
public class Map : MonoBehaviour
{
    [SerializeField] private bool visualizeGrid = true;
    [SerializeField] public Grid<Building> map;
    [SerializeField] public GameObject placeBuilding;

    public static Map mapObject;
    // Start is called before the first frame update
    void Start()
    {
        
        if (map == null)
        {
            map = new Grid<Building>(50, 50, 1f, new Vector3(-25, -25, 0), Vector3.up, null);
        }
        mapObject = this;
    }

    private void OnValidate()
    {
        if (map == null)
        {
            map = new Grid<Building>(50, 50, 1f, new Vector3(-25, -25, 0), Vector3.up, null);
        }
    }

    private void OnEnable()
    {
        if (map == null)
        {
            map = new Grid<Building>(50, 50, 1f, new Vector3(-25, -25, 0), Vector3.up, null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (visualizeGrid)
        {
            map.VisualizeGrid();
            
        }
       
    }

    public GameObject CameraRaycast(Camera camera)
    {
        GameObject result = null;
        Vector2 vec;
        if (map.CameraRaycast(camera, out vec))
        {
            result = map.GetValue(vec)?.gameObject;
        }
        return result;
    }

    public Vector2 CameraRaycastVec2(Camera camera)
    {
        Vector2 vec;
        map.CameraRaycast(camera, out vec);
        
        return vec;
    }

    public bool CheckIfPossiblePlacement(Building building, Vector2 pos)
    {
        for (int i = 0; i < building.size.x; i++)
        {
            for (int j = 0; j < building.size.y; j++)
            {
                Vector2 pos2 = pos + new Vector2(25, 25);
                pos2.x -= i;
                pos2.y -= j;
                Building temp = map.GetValue(pos2);
                if (temp != null)
                {
                    return false;
                }
            }
            
        }
        return true;
    }
    
    public bool CheckIfPossiblePlacement(Vector2 building, Vector2 pos)
    {
        for (int i = 0; i < building.x; i++)
        {
            for (int j = 0; j < building.y; j++)
            {
                Vector2 pos2 = pos + new Vector2(25, 25);
                pos2.x -= i;
                pos2.y -= j;
                Building temp = map.GetValue(pos2);
                if (temp != null)
                {
                    return false;
                }
            }
            
        }
        return true;
    }

    public bool CheckIfSpecificPlacement(Building building, Vector2 pos)
    {
        //Debug.Log(tilemap.GetTile(new Vector3Int((int)vec.x, (int)vec.y, 0)));
        //Debug.Log(buildingToPlace.tileset.GetTypeName(tilemap.GetTile(new Vector3Int((int)vec.x, (int)vec.y, 0))));
        return true;
    }

    public void Place(Building building, Vector2 pos, Quaternion rotation)
    {
        Debug.Log("Place building: " +building.name);
        PlaceBuilding(building, pos, rotation);
    }

    public void Remove(Vector2 pos)
    {
        RemoveBuilding(pos);
    }

    public void ReplaceBuilding(Building building, Vector2 pos, Quaternion rotation)
    {
        Debug.Log("Replacing");
        RemoveBuilding(pos);
        PlaceBuilding(building, pos, rotation);
    }

    
    
    
    
    private void RemoveBuilding(Vector2 pos)
    {
        pos = pos + new Vector2(25, 25);
        Building building = map.GetValue(pos);
        pos = (Vector2) building.transform.position + new Vector2(25, 25) -building.size / 2;
        for (int i = 0; i < building.size.y; i++)
        {
            for (int j = 0; j < building.size.y; j++)
            {
                map.SetValue(pos + new Vector2(j,i), null);
            }
        }
        
    }

    
    private void PlaceBuilding(Building building, Vector2 pos, Quaternion rotation)
    {
        Vector2 pos2 =  pos + new Vector2(25, 25);
        
        for (int i = 0; i < building.size.x; i++)
        {
            for (int j = 0; j < building.size.y; j++)
            {
                pos2 = pos + new Vector2(25, 25);
                pos2.x -= i;
                pos2.y -= j;
                map.SetValue(pos2, building);
            }
            
        }
        building.OnPlace(this, pos2);

        /*
         //Used for updating the tiles around the building
        for (int i = -1; i < building.size.y +1; i++)
        {
            for (int j = -1; j < building.size.x + 1; j++)
            {
                pos.x = j;
                pos.y = i;
                pos += pos2;
                if (i == -1 || i == (int)building.size.y)
                {
                    //Debug.Log(map.GetValue(pos) + " " + pos);
                    map.GetValue(pos)?.OnPlace(this, pos);
                }
                else
                {
                    //Debug.Log(map.GetValue(pos) + " " + pos);
                    map.GetValue(pos)?.OnPlace(this, pos);
                    j += (int)building.size.x + 1;
                }
            }
        }
        */
    }
    
    
    
    
}
