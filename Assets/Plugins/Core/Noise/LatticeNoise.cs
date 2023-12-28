using Core.Maths;

namespace Core.Noise{

    public class LatticeNoise : ANoise
    {
        protected readonly int LatticeSize;

        public LatticeNoise(Vector2Int size, Settings settings, int latticeSize = 10) : base(size, settings)
        {
            LatticeSize = latticeSize;
        }

        public override void Generate()
        {
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Results[y * Size.X + x] = Execute(x, y);
                }
            }
        }

        private byte Execute(int x, int y)
        {
            SmallXXHash hash = SmallXXHash.Seed(NoiseSettings.Seed);
            int frequency = NoiseSettings.Frequency;
            float amplitude = 1.0f;

            float amplitudeSum = 0.0f;
            float sum = 0.0f;

            for (int o = 0; o < NoiseSettings.Octaves; o++)
            {
                sum += amplitude * GetNoise(NoiseSettings.ModifySeed ? hash + o : hash, x * frequency, y * frequency,
                                            frequency);
                amplitudeSum += amplitude;
                frequency *= NoiseSettings.Lacunarity;
                amplitude *= NoiseSettings.Persistence;
            }

            return (byte)(255 * sum / amplitudeSum);
        }

        protected virtual float GetNoise(SmallXXHash hash, int x, int y, int frequency)
        {
            int xMin = x - x % (LatticeSize * frequency);
            int yMin = y - y % (LatticeSize * frequency);

            float gX = (xMin - x) / (float)(LatticeSize * frequency);
            float gY = (yMin - y) / (float)(LatticeSize * frequency);

            float corner00 = Gradient(hash.Eat(xMin).Eat(yMin), gX, gY);
            float corner01 = Gradient(hash.Eat(xMin).Eat(yMin + (LatticeSize * frequency)), gX, gY + 1);
            float corner10 = Gradient(hash.Eat(xMin + (LatticeSize * frequency)).Eat(yMin), gX + 1, gY);
            float corner11 = Gradient(hash.Eat(xMin + (LatticeSize * frequency)).Eat(yMin + (LatticeSize * frequency)),
                                      gX + 1, gY + 1);

            float tX = (x - xMin) / (float)(LatticeSize * frequency);
            tX = Interpolation.SmoothStep(0.0f, 1.0f, tX);
            float tY = (y - yMin) / (float)(LatticeSize * frequency);
            tY = Interpolation.SmoothStep(0.0f, 1.0f, tY);

            float lerp = Interpolation.Linear(
                                              Interpolation.Linear(corner00, corner01, tY),
                                              Interpolation.Linear(corner10, corner11, tY),
                                              tX
                                             );
            return PostProcess(lerp);
        }

        protected virtual float Gradient(SmallXXHash hash, float x, float y)
        {
            return hash.AsNormFloat;
        }

        protected virtual float PostProcess(float value)
        {
            return value;
        }
    }
}