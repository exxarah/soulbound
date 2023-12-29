using UnityEngine;

namespace Game.UI.Player
{
    public abstract class APlayerInformationComponent : MonoBehaviour
    {
        private int m_componentID;
        private void OnEnable()
        {
            m_componentID = PlayerProviderComponent.Instance.RegisterComponent(this);
        }

        private void OnDisable()
        {
            PlayerProviderComponent.Instance.Unregister(m_componentID);
        }

        public abstract void Show(Entity.Entity playerEntity);
    }
}