using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Exstentions 
{
    public static Vector2 RandomVec2(this Vector2 vec)
    {
        vec = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        return vec.normalized;
    }
    
    public static Vector3 RandomVec3(this Vector3 vec)
    {
        vec = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        return vec.normalized;
    }

    public static Vector2 FloorToInt(this Vector2 vec)
    {
        vec.x = Mathf.FloorToInt(vec.x);
        vec.y = Mathf.FloorToInt(vec.y);
        return vec;
    }
    
    public static Vector2 ConvertToXZVector2(this Vector3 vec)
    {
        return new Vector2(vec.x, vec.z);
    }
    
    public static T GetRandom<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }
    
    public static int2 ToInt2(this float3 vec)
    {
        return new int2((int)vec.x, (int)vec.z);
    }
    
    public static int2 ToInt2(this Vector3 vec)
    {
        return new int2((int)vec.x, (int)vec.z);
    }

    
    
    public static Vector3 StringToVector3(this string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith ("(") && sVector.EndsWith (")")) {
            sVector = sVector.Substring(1, sVector.Length-2);
        }

        
        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0], CultureInfo.InvariantCulture),
            float.Parse(sArray[1], CultureInfo.InvariantCulture),
            float.Parse(sArray[2], CultureInfo.InvariantCulture));

        return result;
    }

    
}
