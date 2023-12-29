using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.UI.Player
{
    public class PlayerProviderComponent : SceneSingleton<PlayerProviderComponent>
    {
        [SerializeField]
        private Entity.Entity m_playerEntity = null;

        private List<APlayerInformationComponent> m_components = new List<APlayerInformationComponent>();

        protected override void Awake()
        {
            base.Awake();

            m_playerEntity.HealthComponent.OnHealthChanged += Refresh;
            
            Refresh();
        }

        private void Refresh()
        {
            for (int i = 0; i < m_components.Count; i++)
            {
                if (m_components[i] != null)
                {
                    m_components[i].Show(m_playerEntity);
                }
            }
        }

        public int RegisterComponent(APlayerInformationComponent component)
        {
            component.Show(m_playerEntity);
            for (int i = 0; i < m_components.Count; i++)
            {
                if (m_components[i] == null)
                {
                    m_components[i] = component;
                    return i;
                }
            }
            m_components.Add(component);
            return m_components.Count - 1;
        }
        
        public void Unregister(int spawnPointID)
        {
            if (spawnPointID >= m_components.Count) { return; }

            m_components[spawnPointID] = null;
            
            // Clean up components
            for (int i = m_components.Count - 1; i >= 0; i--)
            {
                if (m_components[i] == null)
                {
                    m_components.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
    }
}