using System;
using System.Collections.Generic;

namespace Core
{
    public static partial class Utils
    {
        public static T GetAndRemoveFirst<T>(this LinkedList<T> list)
        {
            T obj = list.First.Value;
            list.RemoveFirst();
            return obj;
        }

        public static void AddSorted<T>(this LinkedList<T> list, T newElement, IComparer<T> comparer)
        {
            for (LinkedListNode<T> node = list.First; node != null; node = node.Next)
            {
                int compareResult = comparer.Compare(node.Value, newElement);
                switch (compareResult)
                {
                    // Current node goes after, insert here
                    case > 0:
                        list.AddBefore(node, new LinkedListNode<T>(newElement));
                        return;
                }
            }

            list.AddLast(newElement);
        }

        public static void AddSorted<T>(this LinkedList<T> list, T newElement) where T : IComparable<T>
        {
            for (LinkedListNode<T> node = list.First; node != null; node = node.Next)
            {
                int compareResult = node.Value.CompareTo(newElement);
                switch (compareResult)
                {
                    // Current node goes after, insert here
                    case > 0:
                        list.AddBefore(node, new LinkedListNode<T>(newElement));
                        return;
                }
            }

            list.AddLast(newElement);
        }
    }
}