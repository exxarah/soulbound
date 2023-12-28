using System;

namespace Core.Maths
{

    public struct Vector3Int : IEquatable<Vector3Int>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public int Z { get; private set; }

        public Vector3Int(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static readonly Vector3Int Zero = new Vector3Int(0, 0);
        public static readonly Vector3Int Left = new Vector3Int(-1, 0);
        public static readonly Vector3Int Right = new Vector3Int(1, 0);
        public static readonly Vector3Int Up = new Vector3Int(0, -1);
        public static readonly Vector3Int Down = new Vector3Int(0, 1);

        public bool Equals(Vector3Int other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3Int other && Equals(other);
        }

        public override int GetHashCode()
        {
            return new { X, Y, Z }.GetHashCode();
        }

        public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
        {
            return !(lhs == rhs);
        }

        public static Vector3Int operator -(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        public static Vector3Int operator +(Vector3Int lhs, Vector3Int rhs)
        {
            return new Vector3Int(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }
    }
}