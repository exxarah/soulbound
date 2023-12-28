using System;
using System.Linq;

namespace Core.Extensions
{
    public static class ArrayExtensions
    {
        public static void Fill<T>(this T[] originalArray, T with)
        {
            for (int i = 0; i < originalArray.Length; i++) originalArray[i] = with;
        }

        public static void Fill<T>(this T[,] originalArray, T with)
        {
            for (int x = 0; x < originalArray.GetUpperBound(0); x++)
            for (int y = 0; y < originalArray.GetUpperBound(1); y++)
                originalArray[x, y] = with;
        }

        public static void Fill<T>(this T[,,] originalArray, T with)
        {
            for (int x = 0; x <= originalArray.GetUpperBound(0); x++)
            for (int y = 0; y <= originalArray.GetUpperBound(1); y++)
            for (int z = 0; z <= originalArray.GetUpperBound(2); z++)
                originalArray[x, y, z] = with;
        }

        public static bool SequenceEquals<T>(this T[,] a, T[,] b)
        {
            return a.Rank == b.Rank
                   && Enumerable.Range(0, a.Rank).All(d => a.GetLength(d) == b.GetLength(d))
                   && a.Cast<T>().SequenceEqual(b.Cast<T>());
        }

        public static void Shuffle<T>(this T[] array, ref Random rng)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
        
        public static T CreateJaggedArray<T>(params uint[] lengths)
        {
            return (T)InitializeJaggedArray(typeof(T).GetElementType(), 0, lengths);
        }
        
        private static object InitializeJaggedArray(Type type, int index, uint[] lengths)
        {
            Array array = Array.CreateInstance(type, lengths[index]);
            Type elementType = type.GetElementType();

            if (elementType != null)
            {
                for (int i = 0; i < lengths[index]; i++)
                {
                    array.SetValue(
                                   InitializeJaggedArray(elementType, index + 1, lengths), i);
                }
            }

            return array;
        }
    }
}