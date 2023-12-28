using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Utils
{
    public static partial class Utils
    {
        public static IEnumerable<Vector3Int> Map3D(Vector3Int size)
        {
            return Map3D(size.x, size.y, size.z);
        }

        public static IEnumerable<Vector3Int> Map3D(int xSize, int ySize, int zSize)
        {
            for (int x = 0; x < xSize; x++)
            for (int y = 0; y < ySize; y++)
            for (int z = 0; z < zSize; z++)
                yield return new Vector3Int(x, y, z);
        }
    }
}