using System;

namespace Core.Maths
{

    public struct Rect
    {
        public int MinX;
        public int MinY;
        public int MaxX;
        public int MaxY;
        public int Width => Math.Abs(MaxX - MinX);
        public int Height => Math.Abs(MaxY - MinY);

        public static readonly Rect Empty = new Rect(0, 0, 0, 0);

        public Rect(int x, int y, int width, int height)
        {
            MinX = x;
            MinY = y;
            MaxX = x + width;
            MaxY = y + height;
        }

        public bool Intersects(Rect other)
        {
            return MinX <= other.MaxX && MaxX >= other.MinX && MinY <= other.MaxY && MaxY >= other.MinY;
        }

        public bool Contains(Rect other)
        {
            return MinX <= other.MinX && MaxX >= other.MaxX && MinY <= other.MinY && MaxY >= other.MaxY;
        }

        public Vector2Int Center()
        {
            return new Vector2Int((int)(MinX + (MaxX - MinX) / 2.0f), (int)(MinY + (MaxY - MinY) / 2.0f));
        }

        /// <summary>
        /// Get the rectangle that represents the intersection between the two given rectangles
        /// </summary>
        public Rect GetIntersection(Rect other)
        {
            if (!Intersects(other))
            {
                return Rect.Empty;
            }

            int x = Math.Max(other.MinX, MinX);
            int y = Math.Max(other.MinY, MinY);

            int maxX = Math.Min(other.MaxX, MaxX);
            int maxY = Math.Min(other.MaxY, MaxY);

            return new Rect(x, y, maxX - x, maxY - y);
        }
    }
}