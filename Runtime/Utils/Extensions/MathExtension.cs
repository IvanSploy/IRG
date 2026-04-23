using UnityEngine;

namespace IRG
{
    public static class MathExtension
    {
        public static Vector4 ToVector4(this Vector3 vector3, float w)
        {
            return new Vector4(vector3.x, vector3.y, vector3.z, w);
        }
        
        public static Vector3 Mul(this Vector3 numerator, Vector3 denominator)
        {
            return new Vector3(
                numerator.x * denominator.x,
                numerator.y * denominator.y,
                numerator.z * denominator.z
            );
        }
        
        public static Vector3 Div(this Vector3 numerator, Vector3 denominator)
        {
            return new Vector3(
                numerator.x / denominator.x,
                numerator.y / denominator.y,
                numerator.z / denominator.z
            );
        }
        
        public static Vector3Int ToVector3Int(this Vector3 vector)
        {
            return new Vector3Int
            {
                x = (int)vector.x,
                y = (int)vector.y,
                z = (int)vector.z
            };
        }
        
        public static Vector3Int RoundToVector3Int(this Vector3 vector)
        {
            return new Vector3Int
            {
                x = Mathf.RoundToInt(vector.x),
                y = Mathf.RoundToInt(vector.y),
                z = Mathf.RoundToInt(vector.z)
            };
        }
        
        public static Vector3Int FloorToVector3Int(this Vector3 vector)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(vector.x),
                y = Mathf.FloorToInt(vector.y),
                z = Mathf.FloorToInt(vector.z)
            };
        }
        
        public static Vector3Int CeilToVector3Int(this Vector3 vector)
        {
            return new Vector3Int
            {
                x = Mathf.CeilToInt(vector.x),
                y = Mathf.CeilToInt(vector.y),
                z = Mathf.CeilToInt(vector.z)
            };
        }

        public static Vector3 ToVector3(this Vector3Int vectorInt)
        {
            return new Vector3
            {
                x = vectorInt.x,
                y = vectorInt.y,
                z = vectorInt.z
            };
        }

        public static int MaxComponent(this Vector3 v)
        {
            v = Abs(v);
            if (v.x >= v.y && v.x >= v.z) return 0;
            if (v.y >= v.x && v.y >= v.z) return 1;
            return 2;
        }

        public static int MinComponent(this Vector3 v)
        {
            v = Abs(v);
            if (v.x <= v.y && v.x <= v.z) return 0;
            if (v.y <= v.x && v.y <= v.z) return 1;
            return 2;
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }

        public static Vector3Int Sign(this Vector3Int v)
        {
            return new Vector3Int(v.x > 0 ? 1 : -1, v.y > 0 ? 1 : -1, v.z > 0 ? 1 : -1);
        }
    }
}