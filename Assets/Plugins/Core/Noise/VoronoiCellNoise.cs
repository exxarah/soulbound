using System;
using Core.Maths;

namespace Core.Noise
{

    public class VoronoiCellNoise : LatticeNoise
    {
        private readonly Func<float, float, float> GetDistance;

        public VoronoiCellNoise(Vector2Int size, Settings settings, Func<float, float, float> getDistance,
                                int latticeSize = 10) : base(size, settings, latticeSize)
        {
            GetDistance = getDistance;
        }

        protected override float GetNoise(SmallXXHash hash, int x, int y, int frequency)
        {
            int xMin = x - x % (LatticeSize * frequency);
            int yMin = y - y % (LatticeSize * frequency);

            float gX = (xMin - x) / (float)(LatticeSize * frequency);
            float gY = (yMin - y) / (float)(LatticeSize * frequency);

            float minimaValue = 0.0f;
            float minDistance = 2.0f;
            for (int u = -1; u <= 1; u++)
            {
                for (int v = -1; v <= 1; v++)
                {
                    SmallXXHash neighbour = hash.Eat(xMin + (LatticeSize * frequency) * u)
                                                .Eat(yMin + (LatticeSize * frequency) * v);
                    float neighbourDistance =
                        GetDistance.Invoke(neighbour.AsNormFloat + gX + u, neighbour.AsNormFloat + gY + v);
                    if (neighbourDistance < minDistance)
                    {
                        minDistance = neighbourDistance;
                        minimaValue = neighbour.AsNormFloat;
                    }
                }
            }

            return minimaValue;
        }
    }
}