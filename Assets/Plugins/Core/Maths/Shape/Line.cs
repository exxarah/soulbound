using System;
using System.Collections.Generic;

namespace Core.Maths.Shape
{
    public class Line
    {
        public static List<Vector2Int> Bresenham(Vector2Int start, Vector2Int end) =>
            Bresenham(start.X, start.Y, end.X, end.Y);

        public static List<Vector2Int> Bresenham(int startX, int startY, int endX, int endY)
        {
            List<Vector2Int> path = new List<Vector2Int>();
            bool swapped = false;
            bool steep = Math.Abs(endY - startY) > Math.Abs(endX - startX);
            if (steep)
            {
                Utils.Swap(ref startX, ref startY);
                Utils.Swap(ref endX, ref endY);
            }

            if (startX > endX)
            {
                Utils.Swap(ref startX, ref endX);
                Utils.Swap(ref startY, ref endY);
                swapped = true;
            }

            int dX = (endX - startX),
                dY = Math.Abs(endY - startY),
                err = (dX / 2),
                ystep = (startY < endY ? 1 : -1),
                y = startY;

            for (int x = startX; x <= endX; ++x)
            {
                Vector2Int coords = steep ? new Vector2Int(y, x) : new Vector2Int(x, y);
                path.Add(coords);
                err -= dY;
                if (err < 0)
                {
                    y += ystep;
                    err += dX;
                }
            }

            if (swapped)
            {
                path.Reverse();
            }

            return path;
        }
    }
}