using UnityEngine;

namespace Core.Unity.Utils
{
    public static partial class Utils
    {
        public static int Sum(this Vector3Int vec)
        {
            return vec.x * vec.y * vec.z;
        }

        public static int Sum(this Vector2Int vec)
        {
            return vec.x * vec.y;
        }

        public static Vector3 Rotate(this Vector3 vec, Vector3Int axis, float amount)
        {
            vec.Normalize();

            Vector3 cross = Vector3.Cross(vec, axis);
            if (cross == Vector3.zero) cross = Vector3.right;

            vec = Quaternion.AngleAxis(amount, cross) * vec;
            return vec;
        }

        public static Vector3Int Rotate(this Vector3Int vec, Vector3Int axis, float amount)
        {
            Vector3 floatVec = vec;
            floatVec = floatVec.Rotate(axis, amount);
            vec = Vector3Int.FloorToInt(floatVec);
            return vec;
        }
    }
}