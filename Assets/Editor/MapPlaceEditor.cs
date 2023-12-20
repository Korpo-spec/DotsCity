using System;
using DefaultNamespace;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;


[CustomEditor(typeof(Map))]
public class MapPlaceEditor : Editor
{
    private bool useTool = false;
    public override void OnInspectorGUI()
    {
        Map map = target as Map;
        if (map == null)
        {
            return;
        }
        if (map.map == null)
        {
            map.map = new Grid<Building>(50, 50, 1f, new Vector3(-25, -25, 0), Vector3.up, null);
        }
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Place buildings by clicking on the map. Press 'R' to rotate.", MessageType.Info);
        useTool = EditorGUILayout.Toggle(useTool);
        
        //Tools.current = useTool ? Tool.Custom : Tool.Move;


       

        
        
        

    }

    private void OnSceneGUI()
    {
        
    }
}

[EditorTool("Map Place Tool", typeof(Map))]
public class MapPlaceTool : EditorTool, IDrawSelectedHandles
{
    public override void OnActivated()
    {
        SceneView.lastActiveSceneView.ShowNotification(new GUIContent("Entering Map Tool"), .1f);
        SceneView.duringSceneGui += OnToolGUI;
    }

    public override void OnWillBeDeactivated()
    {
        //GUIUtility.hotControl = 0;
        SceneView.duringSceneGui -= OnToolGUI;
    }

    private bool eraserMode = false;
    public override void OnToolGUI(EditorWindow window)
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        if (!(window is SceneView sceneView))
            return;
        

        Handles.BeginGUI();
        GUI.color = !eraserMode? Color.red: Color.gray;
        if (GUI.Button(new Rect(50,10,100,30), "Place"))
        {
            eraserMode = false;
        }
        GUI.color = eraserMode? Color.red: Color.gray;
        if (GUI.Button(new Rect(150,10,100,30), "Eraser"))
        {
            eraserMode = true;
        }
        
        Handles.EndGUI();

        window.wantsMouseMove = true;
        Map map = target as Map;
        if(eraserMode)
        {
            EraseEditorBuilding();
        }
        else
        {
            PlaceEditorBuilding();
        }
        
        
        
    }

    private void EraseEditorBuilding()
    {
        Map map = target as Map;
        Building building = map.placeBuilding.GetComponent<Building>();
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Event currentEvent = Event.current;

        if (currentEvent.type == EventType.MouseDrag && currentEvent.button == 0 &&
            map.map.m_Plane.Raycast(ray, out float enter))
        {

            Debug.Log("Trying to remove building");
            Vector3 pos = ray.GetPoint(enter);
            pos.x = Mathf.Floor(pos.x);
            pos.y = 0;
            pos.z = Mathf.Floor(pos.z);
            
            Building buildingToRemove = map.map.GetValueWorldPosition(pos.ConvertToXZVector2());
            if( buildingToRemove == null)
                return;
            map.Remove(pos.ConvertToXZVector2());
            DestroyImmediate(buildingToRemove.gameObject);
        }
    }
    private void PlaceEditorBuilding()
    {
        Map map = target as Map;
        Building building = map.placeBuilding.GetComponent<Building>();
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Event currentEvent = Event.current;
        
        if ( currentEvent.type == EventType.MouseDrag && currentEvent.button == 0 && map.map.m_Plane.Raycast(ray, out float enter))
        {
            
            Vector3 pos = ray.GetPoint(enter);
            pos.x = Mathf.Floor(pos.x);
            pos.y = 0;
            pos.z = Mathf.Floor(pos.z);
            pos.x += building.size.x / 2;
            pos.z += building.size.y / 2;
            Debug.Log("Placing building");
            
            Handles.DrawWireCube(pos, building.size);
            Handles.color = Color.red;
            Handles.DrawWireCube(pos, building.size);
            Handles.color = Color.white;
            Handles.Label(pos, building.buildingID.ToString());
            map.placeBuilding.transform.Rotate(Vector3.forward, 90);
            if (!map.CheckIfPossiblePlacement(building, pos.ConvertToXZVector2()))
            {
                Debug.Log(pos);
                Debug.Log("Not possible placement" + map.map.GetValueWorldPosition(pos.ConvertToXZVector2()));
                return;
            }
            GameObject g = PrefabUtility.InstantiatePrefab(map.placeBuilding, map.transform) as GameObject;
            if (g == null)
            {
                Debug.Log(g + " is null");
                return;
            }
            g.transform.position = pos;
            g.transform.rotation = Quaternion.identity;
            
            map.Place(g.GetComponent<Building>(), pos.ConvertToXZVector2(), Quaternion.identity);
            
            EditorUtility.SetDirty(g.gameObject);
            AssetDatabase.SaveAssetIfDirty(g.gameObject);
            EditorUtility.SetDirty(map.gameObject);
            AssetDatabase.SaveAssetIfDirty(map.gameObject);
            ToolManager.SetActiveTool(this);
            
        }
    }

    public void OnDrawHandles()
    {
        // Map map = target as Map;
        // Vector2 vec = Event.current.mousePosition;
        // Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        // if (map.map.m_Plane.Raycast(ray, out float enter))
        // {
        //     Vector3 pos = ray.GetPoint(enter);
        //     pos.x = Mathf.Floor(pos.x);
        //     pos.y = 0;
        //     pos.z = Mathf.Floor(pos.z);
        //     // Gizmos.DrawMesh(map.placeBuilding.GetComponent<MeshFilter>().sharedMesh, pos, map.placeBuilding.transform.rotation);
        //     Handles.DrawWireCube(pos, map.placeBuilding.GetComponent<Building>().size);
        // }
        
        
    }
}


