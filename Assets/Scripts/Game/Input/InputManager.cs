using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField]
        private EventSystem m_eventSystem = null;

        public bool InputEnabled { get; private set; } = true;
    }
}