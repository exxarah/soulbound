using System;

namespace Core.Maths
{

    public static class Distance
    {
        public static int Manhattan(Vector2Int pointA, Vector2Int pointB)
        {
            int x = Math.Abs(pointA.X - pointB.X);
            int y = Math.Abs(pointA.Y - pointB.Y);
            return x + y;
        }

        public static float Manhattan(float x, float y)
        {
            return Math.Abs(x) + Math.Abs(y);
        }

        public static int Euclidean(Vector2Int pointA, Vector2Int pointB)
        {
            float x = (pointB.X - pointA.X);
            float y = (pointB.Y - pointA.Y);
            return (int)Euclidean(x, y);
        }

        public static float Euclidean(float x, float y)
        {
            return x * x + y * y;
        }
    }
}