using UnityEngine;

namespace Game.Audio
{
    public class PlaySFXComponent : MonoBehaviour
    {
        [SerializeField]
        private SFXAudioDatabase.SFXKey m_toPlay = SFXAudioDatabase.SFXKey.None;

        public void _Play()
        {
            AudioManager.Instance.Play(m_toPlay);
        }
    }
}