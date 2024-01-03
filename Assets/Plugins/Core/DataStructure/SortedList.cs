using System;
using System.Collections;
using System.Collections.Generic;
using Core.Extensions;

namespace Core.DataStructure
{
    [Serializable]
    public class SortedList<T> : IList<T>
    {
        private List<T> m_internalList = new List<T>();

        private IComparer<T> m_sorter;

        public SortedList()
        {
            m_internalList = new List<T>();
            m_sorter = Comparer<T>.Default;
        }

        public SortedList(int initialLength)
        {
            m_internalList = new List<T>(initialLength);
            m_sorter = Comparer<T>.Default;
        }

        public SortedList(IComparer<T> comparer)
        {
            m_internalList = new List<T>();
            m_sorter = comparer;
        }

        public SortedList(IComparer<T> comparer, int initialLength)
        {
            m_internalList = new List<T>(initialLength);
            m_sorter = comparer;
        }

        public int Count => m_internalList.Count;
        public bool IsReadOnly => false;

        public void Add(T item)
        {
            m_internalList.AddSorted(item, m_sorter);
        }

        // Almost complete re-implementation of IList, but without index-based assignation
        public IEnumerator<T> GetEnumerator()
        {
            return m_internalList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T item)
        {
            return m_internalList.Remove(item);
        }

        public void Clear()
        {
            m_internalList.Clear();
        }

        public bool Contains(T item)
        {
            return m_internalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_internalList.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            return m_internalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            m_internalList.RemoveAt(index);
        }

        public T this[int index]
        {
            get => m_internalList[index];
            set => throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<T> list)
        {
            foreach (T elem in list) Add(elem);
        }

        public void FindAndRemove(Func<T, bool> func)
        {
            for (int i = m_internalList.Count - 1; i >= 0; i--)
            {
                if (func.Invoke(m_internalList[i]))
                {
                    m_internalList.RemoveAt(i);
                    return;
                }
            }
        }
    }
}