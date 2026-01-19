using UnityEngine;

public static class VectorIntExtensions
{
    public static Vector3Int ToVector3Int(this Vector3 vector)
    {
        var vectorInt = new Vector3Int
        {
            x = (int)vector.x,
            y = (int)vector.y,
            z = (int)vector.z
        };
        return vectorInt;
    }
    
    public static Vector3 ToVector3(this Vector3Int vectorInt)
    {
        var vector = new Vector3
        {
            x = vectorInt.x,
            y = vectorInt.y,
            z = vectorInt.z
        };
        return vector;
    }
}
