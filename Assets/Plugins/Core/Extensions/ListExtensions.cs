using System;
using System.Collections.Generic;

namespace Core.Extensions
{

    public static class ListExtensions
    {
        public static T RandomItem<T>(this List<T> list, Random rnd = null)
        {
            if (list.Count == 0)
            {
                return default;
            }
            // Allow seeded random, but if not provided just make one ourselves
            if (rnd == null) { rnd = new Random(); }
            
            // Choose a random integer
            int idx = rnd.Next(0, list.Count); // exclusive max
            return list[idx];
        }
        
        /// <summary>
        /// Insert a value into an IList{T} that is presumed to be already sorted such that sort
        /// ordering is preserved
        /// </summary>
        /// <param name="list">List to insert into</param>
        /// <param name="newElement">Value to insert</param>
        /// <param name="comparer">Comparer to determine sort order with</param>
        /// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
        public static void AddSorted<T>(this List<T> list, T newElement, IComparer<T> comparer)
        {
            int startIndex = 0;
            int endIndex = list.Count;
            while (endIndex > startIndex)
            {
                int windowSize = endIndex - startIndex;
                int middleIndex = startIndex + (windowSize / 2);
                T middleValue = list[middleIndex];
                int compareToResult = comparer.Compare(middleValue, newElement);
                if (compareToResult == 0)
                {
                    list.Insert(middleIndex, newElement);
                    return;
                } else if (compareToResult < 0)
                {
                    startIndex = middleIndex + 1;
                }
                else
                {
                    startIndex = middleIndex + 1;
                }
            }

            list.Insert(startIndex, newElement);
        }

        /// <summary>
        /// Insert a value into an IList{T} that is presumed to be already sorted such that sort
        /// ordering is preserved
        /// </summary>
        /// <param name="list">List to insert into</param>
        /// <param name="newElement">Value to insert</param>
        /// <param name="comparer">Comparer to determine sort order with</param>
        /// <typeparam name="T">Type of element to insert and type of elements in the list</typeparam>
        public static void AddSorted<T>(this List<T> list, T newElement) where T : IComparable<T>
        {
            int startIndex = 0;
            int endIndex = list.Count;
            while (endIndex > startIndex)
            {
                int windowSize = endIndex - startIndex;
                int middleIndex = startIndex + (windowSize / 2);
                T middleValue = list[middleIndex];
                int compareToResult = middleValue.CompareTo(newElement);
                if (compareToResult == 0)
                {
                    list.Insert(middleIndex, newElement);
                    return;
                } else if (compareToResult < 0)
                {
                    startIndex = middleIndex + 1;
                }
                else
                {
                    endIndex = middleIndex;
                }
            }

            list.Insert(startIndex, newElement);
        }

        public static List<T> MergeSort<T>(List<T> sortedA, List<T> sortedB) where T : IComparable<T>
        {
            if (sortedB == null && sortedA != null)
            {
                return sortedA;
            }

            if (sortedA == null && sortedB != null)
            {
                return sortedB;
            }

            if (sortedA == null && sortedB == null)
            {
                return new List<T>();
            }

            List<T> result = new List<T>(sortedA.Count + sortedB.Count);
            int aIndex = 0;
            int bIndex = 0;

            while (aIndex < sortedA.Count && bIndex < sortedB.Count)
            {
                int compareResult = sortedA[aIndex].CompareTo(sortedB[bIndex]);
                if (compareResult <= 0)
                {
                    result.Add(sortedA[aIndex]);
                    aIndex++;
                }
                else
                {
                    result.Add(sortedB[bIndex]);
                    bIndex++;
                }
            }

            // Add any leftovers
            while (aIndex < sortedA.Count)
            {
                result.Add(sortedA[aIndex]);
                aIndex++;
            }

            while (bIndex < sortedB.Count)
            {
                result.Add(sortedB[bIndex]);
                bIndex++;
            }

            return result;
        }

        public static bool EmptyOrNull<T>(this List<T> list)
        {
            return list == null || list.Count <= 0;
        }
    }
}