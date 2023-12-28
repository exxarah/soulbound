using System;
using Core.Maths;

namespace Core.Noise
{

    public class PerlinNoise : LatticeNoise
    {
        public PerlinNoise(Vector2Int size, Settings settings, int latticeSize = 10) : base(size, settings, latticeSize)
        {
        }

        protected override float Gradient(SmallXXHash hash, float x, float y)
        {
            float gX = hash.AsNormFloat * 2.0f - 1.0f;
            float gY = 0.5f - Math.Abs(gX);
            gX -= (float)Math.Floor(gX + 0.5f);
            return (float)Helpers.Clamp((gX * x + gY * y) * (2.0f / 0.53528f) + 0.5f, 0.0f, 1.0f);
        }

        protected override float PostProcess(float value)
        {
            return Math.Abs(value);
        }
    }
}