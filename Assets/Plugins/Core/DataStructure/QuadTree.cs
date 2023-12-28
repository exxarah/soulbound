using System;
using System.Collections.Generic;
using System.Linq;
using Core.Maths;

namespace Core.DataStructure
{
    public class QuadTree
    {
        private readonly List<ISpatialEntity> m_elements = new List<ISpatialEntity>();

        private readonly int m_bucketCapacity;
        private readonly int m_maxDepth;

        private QuadTree m_upperLeft;
        private QuadTree m_upperRight;
        private QuadTree m_lowerLeft;
        private QuadTree m_lowerRight;

        public RectF Bounds { get; }

        public bool IsLeaf =>
            m_upperLeft == null && m_upperRight == null && m_lowerLeft == null && m_lowerRight == null;

        public int Level { get; set; } = 0;

        /// <param name="bounds">The rectangular region this QuadTree encompasses</param>
        /// <param name="bucketCapacity">How many elements can be in the region, before we split</param>
        /// <param name="maxDepth">How deep we can go before splitting further</param>
        public QuadTree(RectF bounds, int bucketCapacity = 32, int maxDepth = 5)
        {
            m_bucketCapacity = bucketCapacity;
            m_maxDepth = maxDepth;

            Bounds = bounds;
        }

        public void Insert(ISpatialEntity element)
        {
            if (!Bounds.Contains(element.Bounds))
            {
                throw new ArgumentException("Element outside QuadTree bounds");
            }

            if (m_elements.Count >= m_bucketCapacity)
            {
                Split();
            }

            QuadTree containingChild = GetContainingChild(element.Bounds);
            if (containingChild != null)
            {
                containingChild.Insert(element);
            }
            else
            {
                // If no children, this is either a leaf node (not yet split), or the element's boundaries overlap multiple children. Assign to this node
                m_elements.Add(element);
            }
        }

        public bool Remove(ISpatialEntity element)
        {
            QuadTree containingChild = GetContainingChild(element.Bounds);
            bool removed = containingChild?.Remove(element) ?? m_elements.Remove(element);

            if (removed && CountElements() <= m_bucketCapacity)
            {
                Merge();
            }

            return removed;
        }

        public IEnumerable<ISpatialEntity> FindCollisions(ISpatialEntity element,
                                                          Func<ISpatialEntity, bool> selector = null) =>
            FindCollisions(element.Bounds, selector);

        public IEnumerable<ISpatialEntity> FindCollisions(RectF bounds, Func<ISpatialEntity, bool> selector = null)
        {
            Queue<QuadTree> nodes = new Queue<QuadTree>();
            List<ISpatialEntity> collisions = new List<ISpatialEntity>();

            nodes.Enqueue(this);
            while (nodes.Count > 0)
            {
                QuadTree node = nodes.Dequeue();
                if (!bounds.Intersects(node.Bounds))
                {
                    continue;
                }

                collisions.AddRange(node.m_elements.Where(e => e.Bounds.Intersects(bounds) &&
                                                               (selector == null || selector(e))));
                if (node.IsLeaf) continue;

                if (node.m_upperLeft != null && bounds.Intersects(node.m_upperLeft.Bounds))
                {
                    nodes.Enqueue(node.m_upperLeft);
                }

                if (node.m_upperRight != null && bounds.Intersects(node.m_upperRight.Bounds))
                {
                    nodes.Enqueue(node.m_upperRight);
                }

                if (node.m_lowerLeft != null && bounds.Intersects(node.m_lowerLeft.Bounds))
                {
                    nodes.Enqueue(node.m_lowerLeft);
                }

                if (node.m_lowerRight != null && bounds.Intersects(node.m_lowerRight.Bounds))
                {
                    nodes.Enqueue(node.m_lowerRight);
                }
            }

            return collisions;
        }

        private int CountElements()
        {
            if (IsLeaf)
            {
                return m_elements.Count;
            }

            int count = 0;
            if (m_upperLeft != null) count += m_upperLeft.CountElements();
            if (m_upperRight != null) count += m_upperRight.CountElements();
            if (m_lowerLeft != null) count += m_lowerLeft.CountElements();
            if (m_lowerRight != null) count += m_lowerRight.CountElements();

            return count;
        }

        private void Merge()
        {
            if (IsLeaf)
            {
                return;
            }

            if (m_upperLeft != null) m_elements.AddRange(m_upperLeft.m_elements);
            if (m_upperRight != null) m_elements.AddRange(m_upperRight.m_elements);
            if (m_lowerLeft != null) m_elements.AddRange(m_lowerLeft.m_elements);
            if (m_lowerRight != null) m_elements.AddRange(m_lowerRight.m_elements);

            m_upperLeft = m_upperRight = m_lowerLeft = m_lowerRight = null;
        }

        private void Split()
        {
            if (!IsLeaf)
            {
                return;
            }

            if (Level + 1 > m_maxDepth)
            {
                return;
            }

            float childWidth = Bounds.Width / 2.0f, childHeight = Bounds.Height / 2.0f;
            m_upperLeft = CreateChild(Bounds.MinX, Bounds.MinY, childWidth, childHeight);
            m_upperRight = CreateChild(Bounds.MinX + childWidth, Bounds.MinY, childWidth, childHeight);
            m_lowerLeft = CreateChild(Bounds.MinX, Bounds.MinY + childHeight, childWidth, childHeight);
            m_lowerRight = CreateChild(Bounds.MinX + childWidth, Bounds.MinY + childHeight, childWidth, childHeight);

            for (int i = m_elements.Count - 1; i >= 0; i--)
            {
                QuadTree containingChild = GetContainingChild(m_elements[i].Bounds);
                if (containingChild == null) continue;

                containingChild.Insert(m_elements[i]);
                m_elements.RemoveAt(i);
            }
        }

        private QuadTree CreateChild(float x, float y, float width, float height)
        {
            QuadTree child = new QuadTree(new RectF(x, y, width, height))
            {
                Level = Level + 1,
            };
            return child;
        }

        private QuadTree GetContainingChild(RectF bounds)
        {
            if (IsLeaf)
            {
                return null;
            }

            if (m_upperLeft != null && m_upperLeft.Bounds.Contains(bounds)) return m_upperLeft;
            if (m_upperRight != null && m_upperRight.Bounds.Contains(bounds)) return m_upperRight;
            if (m_lowerLeft != null && m_lowerLeft.Bounds.Contains(bounds)) return m_lowerLeft;
            if (m_lowerRight != null && m_lowerRight.Bounds.Contains(bounds)) return m_lowerRight;

            return null;
        }
    }

    public interface ISpatialEntity
    {
        RectF Bounds { get; }
    }
}