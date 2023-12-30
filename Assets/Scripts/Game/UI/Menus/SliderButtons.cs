using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menus
{
    public class SliderButtons : MonoBehaviour
    {
        [SerializeField]
        private Slider m_slider = null;

        [SerializeField]
        private int m_segments = 10;

        public void _Increment()
        {
            m_slider.value += Mathf.Clamp((m_slider.maxValue - m_slider.minValue) / m_segments, m_slider.minValue, m_slider.maxValue);
        }

        public void _Decrement()
        {
            m_slider.value -= Mathf.Clamp((m_slider.maxValue - m_slider.minValue) / m_segments, m_slider.minValue, m_slider.maxValue);
        }
    }
}