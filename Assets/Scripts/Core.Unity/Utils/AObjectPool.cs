using Game.Entity;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.Unity.Utils
{
    public abstract class AObjectPool<T> : MonoBehaviour where T : class
    {
        public bool DoCollisionChecks { get; private set; } = true;
        public int MaxPoolSize { get; private set; } = 10;

        private IObjectPool<T> m_pool;

        protected IObjectPool<T> Pool
        {
            get
            {
                if (m_pool == null)
                {
                    m_pool = new ObjectPool<T>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject,
                                               DoCollisionChecks, maxSize: MaxPoolSize);
                }

                return m_pool;
            }
        }

        protected abstract void OnDestroyPoolObject(T obj);

        protected abstract void OnReturnedToPool(T obj);

        protected abstract void OnTakeFromPool(T obj);
        protected abstract T CreatePooledItem();
    }
}