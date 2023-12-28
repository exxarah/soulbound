using System;

namespace Core.Maths
{

    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly Vector2Int Zero = new Vector2Int(0, 0);
        public static readonly Vector2Int Left = new Vector2Int(-1, 0);
        public static readonly Vector2Int Right = new Vector2Int(1, 0);
        public static readonly Vector2Int Up = new Vector2Int(0, -1);
        public static readonly Vector2Int Down = new Vector2Int(0, 1);

        public bool Equals(Vector2Int other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2Int other && Equals(other);
        }

        public override int GetHashCode()
        {
            // .NET 4.7.2 HashCode.Combine replacement
            return new { X, Y }.GetHashCode();
        }

        public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
        {
            return !(lhs == rhs);
        }

        public static Vector2Int operator -(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Vector2Int operator +(Vector2Int lhs, Vector2Int rhs)
        {
            return new Vector2Int(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }
    }
}