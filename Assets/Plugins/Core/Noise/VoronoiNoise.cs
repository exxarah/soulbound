using System;
using Core.Maths;

namespace Core.Noise
{

    public class VoronoiNoise : LatticeNoise
    {
        public VoronoiNoise(Vector2Int size, Settings settings, int latticeSize = 10) : base(size, settings,
            latticeSize)
        {
        }

        protected override float GetNoise(SmallXXHash hash, int x, int y, int frequency)
        {
            int xMin = x - x % (LatticeSize * frequency);
            int yMin = y - y % (LatticeSize * frequency);

            float gX = (xMin - x) / (float)(LatticeSize * frequency);
            float gY = (yMin - y) / (float)(LatticeSize * frequency);

            float minima = 2.0f;
            for (int u = -1; u <= 1; u++)
            {
                for (int v = -1; v <= 1; v++)
                {
                    SmallXXHash neighbour = hash.Eat(xMin + (LatticeSize * frequency) * u)
                                                .Eat(yMin + (LatticeSize * frequency) * v);
                    minima = Math.Min(minima,
                                       GetDistance(neighbour.AsNormFloat + gX + u, neighbour.AsNormFloat + gY + v));
                }
            }

            return minima;
        }

        private static float GetDistance(float x, float y) => (float)Math.Sqrt(x * x + y * y);
    }
}