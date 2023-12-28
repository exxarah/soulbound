using Core.Maths;

namespace Core.Noise
{

    public class HashNoise : ANoise
    {
        public HashNoise(Vector2Int size, Settings settings) : base(size, settings)
        {
        }

        public override void Generate()
        {
            // TODO: This would be pretty easy to thread. Probably worth doing
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Results[y * Size.X + x] = Execute(SmallXXHash.Seed(NoiseSettings.Seed), x, y);
                }
            }
        }

        private byte Execute(SmallXXHash hash, int x, int y)
        {
            return hash.Eat(x).Eat(y).AsByte;
        }
    }
}