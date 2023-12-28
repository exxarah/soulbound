using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.DataStructure
{
    // TODO: make it actually implement IList
    public class WeightedList<TItem>
    {
        private readonly List<Entry> m_entries = new List<Entry>();
        private int m_totalWeight;

        public int Count => m_entries.Count;

        public void Add(TItem item, int weight = 1)
        {
            m_entries.Add(new Entry { Item = item, Weight = weight });
            m_totalWeight += weight;
        }

        public void RemoveAt(int index)
        {
            m_totalWeight -= m_entries[index].Weight;
            m_entries.RemoveAt(index);
        }

        public TItem Get(ref Random rnd)
        {
            int chosenWeight = rnd.Next(m_totalWeight);
            int processedWeights = 0;
            foreach (Entry entry in m_entries)
            {
                processedWeights += entry.Weight;
                if (chosenWeight <= processedWeights) return entry.Item;
            }

            return default;
        }

        public TItem GetAt(int index)
        {
            return m_entries[index].Item;
        }

        public List<TItem> ToList()
        {
            return m_entries.Select(entry => entry.Item).ToList();
        }

        private struct Entry
        {
            public TItem Item;
            public int Weight;
        }
    }
}