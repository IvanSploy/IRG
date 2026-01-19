using UnityEngine;

namespace IRG.Utils
{
    public static class MathExtensions
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
    }
}