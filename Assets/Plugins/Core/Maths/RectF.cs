using System;
using System.Numerics;

namespace Core.Maths
{

    public struct RectF
    {
        public float MinX;
        public float MinY;
        public float MaxX;
        public float MaxY;
        public float Width => Math.Abs(MaxX - MinX);
        public float Height => Math.Abs(MaxY - MinY);

        public static readonly RectF Empty = new RectF(0, 0, 0, 0);

        public RectF(float x, float y, float width, float height)
        {
            MinX = x;
            MinY = y;
            MaxX = x + width;
            MaxY = y + height;
        }

        public bool Intersects(RectF other)
        {
            return MinX <= other.MaxX && MaxX >= other.MinX && MinY <= other.MaxY && MaxY >= other.MinY;
        }

        public bool Contains(RectF other)
        {
            return MinX <= other.MinX && MaxX >= other.MaxX && MinY <= other.MinY && MaxY >= other.MaxY;
        }

        public Vector2 Center()
        {
            return new Vector2(MinX + (MaxX - MinX) / 2.0f, MinY + (MaxY - MinY) / 2.0f);
        }

        /// <summary>
        /// Get the rectangle that represents the intersection between the two given rectangles
        /// </summary>
        public RectF GetIntersection(RectF other)
        {
            if (!Intersects(other))
            {
                return RectF.Empty;
            }

            float x = Math.Max(other.MinX, MinX);
            float y = Math.Max(other.MinY, MinY);

            float maxX = Math.Min(other.MaxX, MaxX);
            float maxY = Math.Min(other.MaxY, MaxY);

            return new RectF(x, y, maxX - x, maxY - y);
        }
    }
}