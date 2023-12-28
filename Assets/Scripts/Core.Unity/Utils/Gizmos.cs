using UnityEngine;

namespace Core.Unity.Utils
{
    public static partial class Utils
    {
        public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f,
                                     float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawLine(from, to);
            Vector3 direction = to - from;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawLine(to, to + right * arrowHeadLength);
            Gizmos.DrawLine(to, to + left * arrowHeadLength);
        }

        public static void DrawDoubleArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.25f,
                                           float arrowHeadAngle = 20.0f)
        {
            Gizmos.DrawLine(from, to);

            Vector3 direction = to - from;
            Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                            new Vector3(0, 0, 1);
            Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                           new Vector3(0, 0, 1);
            Gizmos.DrawLine(to, to + right * arrowHeadLength);
            Gizmos.DrawLine(to, to + left * arrowHeadLength);

            direction = from - to;
            right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 + arrowHeadAngle, 0) *
                    new Vector3(0, 0, 1);
            left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180 - arrowHeadAngle, 0) *
                   new Vector3(0, 0, 1);

            Gizmos.DrawLine(from, from + right * arrowHeadLength);
            Gizmos.DrawLine(from, from + left * arrowHeadLength);
        }

        public static void DrawSquare(Vector2 min, Vector2 max)
        {
            Gizmos.DrawLine(min, new Vector3(min.x, max.y));
            Gizmos.DrawLine(new Vector3(min.x, max.y), max);
            Gizmos.DrawLine(max, new Vector3(max.x, min.y));
            Gizmos.DrawLine(new Vector3(max.x, min.y), min);
        }

        public static void DrawSquare(Vector3 min, Vector3 max)
        {
            Gizmos.DrawLine(min, new Vector3(min.x, max.y));
            Gizmos.DrawLine(new Vector3(min.x, max.y), max);
            Gizmos.DrawLine(max, new Vector3(max.x, min.y));
            Gizmos.DrawLine(new Vector3(max.x, min.y), min);
        }

        public static void DrawShape(Vector3[] corners)
        {
            for (int i = 0; i < corners.Length - 1; i++) Gizmos.DrawLine(corners[i], corners[i + 1]);
            Gizmos.DrawLine(corners[corners.Length - 1], corners[0]);
        }
    }
}