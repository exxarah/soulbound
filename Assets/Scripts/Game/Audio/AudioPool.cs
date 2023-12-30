using Core.Unity.Utils;
using UnityEngine;

namespace Game.Audio
{
    public class AudioPool : AObjectPool<AudioSource>
    {
        [SerializeField]
        private AudioSource m_template = null;
        
        protected override void OnDestroyPoolObject(AudioSource obj)
        {
            Destroy(obj.gameObject);
        }

        protected override void OnReturnedToPool(AudioSource obj)
        {
            obj.gameObject.SetActiveSafe(false);
        }

        protected override void OnTakeFromPool(AudioSource obj)
        {
            obj.gameObject.SetActiveSafe(true);
        }

        protected override AudioSource CreatePooledItem()
        {
            AudioSource minion = Instantiate(m_template, transform);
            return minion;
        }

        public AudioSource Get()
        {
            return Pool.Get();
        }

        public void Return(AudioSource source)
        {
            Pool.Release(source);
        }
    }
}