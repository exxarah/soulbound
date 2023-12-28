using System;
using Core.Maths;
using Core.Extensions;
using Core.Extensions;

namespace Core.Noise
{

    public abstract class ANoise
    {
        protected Settings NoiseSettings;

        protected byte[] Results;
        protected Vector2Int Size;

        protected ANoise(Vector2Int size, Settings settings)
        {
            Results = new Byte[size.X * size.Y];
            Size = size;
            Results.Fill(Byte.MinValue);
            NoiseSettings = settings;
        }

        // public void WriteToFile(string filePath)
        // {
        //     Image<Rgb24> img = new Image<Rgb24>(Size.X, Size.Y);
        //     for (int x = 0; x < Size.X; x++)
        //     {
        //         for (int y = 0; y < Size.Y; y++)
        //         {
        //             byte noiseVal = (byte)GetNoise(x, y);
        //             img[x, y] = new Rgb24(noiseVal, noiseVal, noiseVal);
        //         }
        //     }
        //
        //     img.SaveAsBmp(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Delve",
        //                                filePath));
        // }

        public abstract void Generate();

        public byte GetNoise(int idx)
        {
            return Results[idx];
        }

        public byte GetNoise(int x, int y)
        {
            return Results[y * Size.X + x];
        }

        public readonly struct Settings
        {
            public readonly int Seed;
            public readonly int Frequency; // Fractal scale
            public readonly int Octaves; // How many fractal passes to do
            public readonly int Lacunarity; // How much to increase the frequency by per octave
            public readonly float Persistence; // How much to scale the amplitude by per octave

            public readonly bool ModifySeed;

            public Settings(int seed, int frequency = 1, int octaves = 1, int lacunarity = 2, float persistence = 0.5f,
                            bool modifySeed = true)
            {
                Seed = seed;
                Frequency = frequency;
                Octaves = octaves;
                Lacunarity = lacunarity;
                Persistence = persistence;
                ModifySeed = modifySeed;
            }
        }

        public readonly struct SmallXXHash
        {
            private readonly uint accumulator;

            const uint primeA = 0b10011110001101110111100110110001;
            const uint primeB = 0b10000101111010111100101001110111;
            const uint primeC = 0b11000010101100101010111000111101;
            const uint primeD = 0b00100111110101001110101100101111;
            const uint primeE = 0b00010110010101100110011110110001;

            public SmallXXHash(uint accumulator)
            {
                this.accumulator = accumulator;
            }

            public SmallXXHash Eat(int data) => RotateLeft(accumulator + (uint)data * primeC, 17) * primeD;

            public SmallXXHash Eat(byte data) => RotateLeft(accumulator + data * primeE, 11) * primeA;

            public byte AsByte => (byte)(this & 255);
            public float AsNormFloat => AsByte * (1.0f / 255.0f);

            private static uint RotateLeft(uint data, int steps) => (data << steps) | (data >> 32 - steps);

            public static SmallXXHash Seed(int seed) => (uint)seed + primeE;

            public static SmallXXHash operator +(SmallXXHash hash, int v) => hash.accumulator + (uint)v;
            public static implicit operator SmallXXHash(uint accumulator) => new SmallXXHash(accumulator);

            public static implicit operator uint(SmallXXHash hash)
            {
                uint avalanche = hash.accumulator;
                avalanche ^= avalanche >> 15;
                avalanche *= primeB;
                avalanche ^= avalanche >> 13;
                avalanche *= primeC;
                avalanche ^= avalanche >> 16;
                return avalanche;
            }
        }
    }
}