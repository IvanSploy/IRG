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

        public static int MaxComponent(this Vector3 v)
        {
            int max = 0;
            if (v.y > v.x)
            {
                if (v.y > v.z)
                {
                    max = 1;
                }
                else
                {
                    max = 2;
                }
            }
            else if (v.z > v.x)
            {
                if (v.z > v.y)
                {
                    max = 2;
                }
                else
                {
                    max = 1;
                }
            }

            return max;
        }

        public static int MinComponent(this Vector3 v)
        {
            return MaxComponent(-v);
        }

        public static Vector3 Abs(this Vector3 v)
        {
            return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
        }
    }
}