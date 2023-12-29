using UnityEngine;

namespace Game.UI.Player
{
    public abstract class APlayerInformationComponent : MonoBehaviour
    {
        private int m_componentID;
        private void Start()
        {
            m_componentID = PlayerProviderComponent.Instance.RegisterComponent(this);
        }

        private void OnDestroy()
        {
            PlayerProviderComponent.Instance.Unregister(m_componentID);
        }

        public abstract void Show(Entity.Entity playerEntity);
    }
}