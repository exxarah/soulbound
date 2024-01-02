using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Menus
{
    public class DefaultSelectable : MonoBehaviour
    {
        [SerializeField]
        private Selectable m_selectable = null;

        private void OnEnable()
        {
            if (m_selectable != null)
            {
                GameContext.Instance.InputManager.EventSystem.SetSelectedGameObject(m_selectable.gameObject);   
            }
        }
    }
}