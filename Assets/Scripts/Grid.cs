using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Serialization.Json;
using UnityEngine;
using Unity.VisualScripting;
using Object = UnityEngine.Object;

[Serializable]
public class Grid<T> : ISerializationCallbackReceiver
{
    public T[,] m_Grid;
    private TextMesh[,] m_TextMeshes;
    public Plane m_Plane;

    [SerializeField]private int m_Width;
    [SerializeField]private int m_Height;
    [SerializeField]private float m_CellSize;
    [SerializeField]public Vector3 m_Origin;
    [SerializeField]private Vector3 m_Normal;
    [SerializeField]private Quaternion m_RotationQuat;
    private Action<Vector2, T[,]> m_ONSetup;

    public event Action<T> OnGridValueChanged;
    [SerializeField]public readonly Vector2 size;
    public Grid(int width, int height, float cellSize, Vector3 origin, Vector3 normal, Action<Vector2, T[,]> onSetup)
    {
        m_Grid = new T[width, height];
        m_TextMeshes = new TextMesh[width, height];
        this.m_Width = width;
        this.m_Height = height;
        size.x = width;
        size.y = height;
        this.m_CellSize = cellSize;
        this.m_Origin = origin;
        this.m_Normal = normal;
        m_Normal.Normalize();
        m_ONSetup = onSetup;
        
        m_Plane = new Plane(normal, Vector3.zero);
        float angle = Vector3.Angle(normal, Vector3.forward);
        Vector3 rotationAxis = Vector3.Cross(normal, Vector3.forward);
        m_RotationQuat = Quaternion.AngleAxis(angle, rotationAxis);
        Matrix4x4 mat = Matrix4x4.Rotate(m_RotationQuat);
        
        VisualizeGrid();
    }

    public void VisualizeGrid()
    {
        //Debug.Log(m_Grid.GetLength(1));
        //Debug.Log(m_Grid.GetLength(0));
        
        for (int i = 0; i < m_Grid.GetLength(1); i++)
        {
            for (int j = 0; j < m_Grid.GetLength(0); j++)
            {
                /*_textMeshes[j,i] = UtilsClass.CreateWorldText(grid[j, i].ToString(), null, new Vector3(cellSize * (j +0.5f), cellSize * (i+0.5f), 0), 7,
                    Color.white, TextAnchor.MiddleCenter);
                    */
                
                m_ONSetup?.Invoke(new Vector2(j,i), m_Grid);
                Debug.DrawLine(m_RotationQuat * (m_Origin+new Vector3(m_CellSize * j, m_CellSize * i, 0)) ,m_RotationQuat *(m_Origin+new Vector3(m_CellSize*(j+1), m_CellSize*(i), 0)), Color.white);
                Debug.DrawLine(m_RotationQuat * (m_Origin+new Vector3(m_CellSize * j, m_CellSize * i, 0)),m_RotationQuat *(m_Origin+new Vector3(m_CellSize*(j), m_CellSize*(i+1), 0)), Color.white);
                if (j == m_Grid.GetLength(0)-1)
                {
                    Debug.DrawLine(m_RotationQuat *(m_Origin+new Vector3(m_CellSize * (j+1), m_CellSize * i, 0)),m_RotationQuat *(m_Origin+new Vector3(m_CellSize*(j+1), m_CellSize*(i+1), 0)), Color.white);
                }
                
                if (i == m_Grid.GetLength(1)-1)
                {
                    Debug.DrawLine(m_RotationQuat *(m_Origin+new Vector3(m_CellSize * j, m_CellSize * (i+1), 0)),m_RotationQuat *(m_Origin+new Vector3(m_CellSize*(j+1), m_CellSize*(i+1), 0)), Color.white);
                }
            }
        }
    }

    /*
    public static void VisualizeGizmoGrid(int height, int width)
    {
        for (int i = 0; i < grid.GetLength(1); i++)
        {
            for (int j = 0; j < grid.GetLength(0); j++)
            {
                Debug.DrawLine(_rotationQuat * (origin+new Vector3(cellSize * j, cellSize * i, 0)) ,_rotationQuat *(origin+new Vector3(cellSize*(j+1), cellSize*(i), 0)), Color.white, 100f);
                Debug.DrawLine(_rotationQuat * (origin+new Vector3(cellSize * j, cellSize * i, 0)),_rotationQuat *(origin+new Vector3(cellSize*(j), cellSize*(i+1), 0)), Color.white, 100f);
                if (j == grid.GetLength(0)-1)
                {
                    Debug.DrawLine(_rotationQuat *(origin+new Vector3(cellSize * (j+1), cellSize * i, 0)),_rotationQuat *(origin+new Vector3(cellSize*(j+1), cellSize*(i+1), 0)), Color.white, 100f);
                }
                
                if (i == grid.GetLength(1)-1)
                {
                    Debug.DrawLine(_rotationQuat *(origin+new Vector3(cellSize * j, cellSize * (i+1), 0)),_rotationQuat *(origin+new Vector3(cellSize*(j+1), cellSize*(i+1), 0)), Color.white, 100f);
                }
            }
        }
    }
    */
    
    

    private void UpdateVisualizer(int x, int y)
    {
        //_textMeshes[x, y].text = grid[x, y].ToString();
    }

    public T GetValue(int x, int y)
    {
        //Debug.Log("X: " + x + " Y: " +y);
        if (x < 0 || x > m_Width-1 || y < 0 || y > m_Height-1)
        {
            return default(T);
        }

        return m_Grid[x, y];
    }

    public T GetValue(Vector2 vector2)
    {
        return GetValue(Mathf.FloorToInt(vector2.x), Mathf.FloorToInt(vector2.y));
    }
    
    public T GetValueWorldPosition(Vector3 worldPosition)
    {
        Vector2 gridPosition = worldPosition - m_Origin;
        Debug.Log(gridPosition);
        return GetValue(gridPosition);
    }

    public void SetValue(int x, int y, T value)
    {
        if (x < 0 || x > m_Width-1 || y < 0 || y > m_Height-1)
        {
            return;
        }
        m_Grid[x, y] = value;
        UpdateVisualizer(x,y);
        OnGridValueChanged?.Invoke(value);
    }

    public void SetValue(Vector2 vector2, T value)
    {
        SetValue(Mathf.FloorToInt(vector2.x),Mathf.FloorToInt(vector2.y), value);
    }
    
    public void SetValueWorldPosition(Vector3 worldPosition, T value)
    {
        Vector2 gridPosition = worldPosition - m_Origin;
        SetValue(gridPosition, value);
    }

    public bool CameraRaycast(Camera camera, out Vector2 vec)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter = 0;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Debug.Log(ray.GetPoint(enter));
            Vector3 rayHit = ray.GetPoint(enter);
            vec.x = rayHit.x;
            vec.y = rayHit.y;
            
            if (vec.x < 0 || vec.x > m_Width || vec.y < 0 || vec.y > m_Height)
            {
                return false;
            }
            /*
            Vector3 cameraX = Vector3.zero;
            cameraX = camera.transform.right;
            Vector3 normalX = Vector3.zero;
            normalX = _normal;
            
            Vector3 cameraY = Vector3.zero;
            cameraY = camera.transform.up;
            Vector3 normalY = Vector3.zero;
            normalY = _normal;
            
            Vector3 scaleVec = Vector3.zero;
            vec.x *= Vector3.Angle(cameraX, normalX) / 90 + 1;
            vec.y *= Vector3.Angle(cameraY, normalY) / 90 + 1;
            Debug.Log(Vector3.Angle(cameraX, normalX) + " " + cameraX + " " + normalX);
            Debug.Log(vec);
            */
            return true;
        }
        vec = Vector2.zero;
        return false;
    }
    
    public Vector2 CameraRaycast(Camera camera, Vector2 vec, out bool hit)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        float enter = 0;
        if (m_Plane.Raycast(ray, out enter))
        {
            //Debug.Log(ray.GetPoint(enter));
            Vector3 rayHit = ray.GetPoint(enter);
            vec.x = rayHit.x;
            vec.y = rayHit.y;
            
            if (vec.x < 0 || vec.x > m_Width || vec.y < 0 || vec.y > m_Height)
            {
                hit = false;
                return vec;
            }
            /*
            Vector3 cameraX = Vector3.zero;
            cameraX = camera.transform.right;
            Vector3 normalX = Vector3.zero;
            normalX = _normal;
            
            Vector3 cameraY = Vector3.zero;
            cameraY = camera.transform.up;
            Vector3 normalY = Vector3.zero;
            normalY = _normal;
            
            Vector3 scaleVec = Vector3.zero;
            vec.x *= Vector3.Angle(cameraX, normalX) / 90 + 1;
            vec.y *= Vector3.Angle(cameraY, normalY) / 90 + 1;
            Debug.Log(Vector3.Angle(cameraX, normalX) + " " + cameraX + " " + normalX);
            Debug.Log(vec);
            */
            hit= true;
            return vec;
        }
        vec = Vector2.zero;
        hit = false;
        return vec;
    }


    [SerializeField] private List<SerializeDataMap<T>> _serializeDataMaps; 
    public void OnBeforeSerialize()
    {
        if (m_Grid == null)
        {
            return;
        }
        if (_serializeDataMaps== null)
        {
            _serializeDataMaps = new List<SerializeDataMap<T>>();
        }
        /*
        else if (_serializeDataMaps.Count > 0)
        {
            foreach(var data in _serializeDataMaps)
            {
                if (data.value == null)
                {
                    _serializeDataMaps.Remove(data);
                }
                
            }
        }
        */
        else
        {
            _serializeDataMaps.Clear();
        }
        
        for (int i = 0; i < m_Grid.GetLength(1); i++)
        {
            for (int j = 0; j < m_Grid.GetLength(0); j++)
            {
                if (GetValue(j,i) != null)
                {
                    T value = GetValue(j,i);
                    if (value is Object && !(value as Object))
                    {
                        continue;
                    }
                    _serializeDataMaps.Add(new SerializeDataMap<T>()
                        {positionX = j, positionY = i, value = GetValue(j,i)});
                }
                
            }
        }
        
        //String json = JsonSerialization.ToJson( _serializeDataMaps);
        //Debug.Log(json);
    }

    public void OnAfterDeserialize()
    {
        if (m_Grid == null || m_Grid.Length == 0)
        {
            //Debug.Log("Create new grid");
            //Debug.Log(m_Width + " " + m_Height);
            
            
            
        }
        m_Grid = new T[m_Width, m_Height];
        m_Plane = new Plane(m_Normal, Vector3.zero);
        float angle = Vector3.Angle(m_Normal, Vector3.forward);
        Vector3 rotationAxis = Vector3.Cross(m_Normal, Vector3.forward);
        m_RotationQuat = Quaternion.AngleAxis(angle, rotationAxis);
        Matrix4x4 mat = Matrix4x4.Rotate(m_RotationQuat);

        foreach (var data in _serializeDataMaps)
        {
            SetValue(data.positionX, data.positionY, data.value);
        }

        
    }
}
[Serializable]
public struct SerializeDataMap<T>
{
    public int positionX;
    public int positionY;
    public T value;
}
