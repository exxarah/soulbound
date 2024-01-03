using System;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera3D = null;

        [SerializeField]
        private RectTransform m_canvasRect = null;

        [SerializeField]
        private CanvasGroup m_canvasGroup = null;

        [SerializeField]
        private SlicedFilledImage m_healthFill = null;

        private Entity.Entity m_entity;
        private Vector3? m_lastCameraPosition = null;
        private Vector3? m_lastObjectPosition = null;
        private float? m_lastCameraSize = null;

        public void Initialise(Entity.Entity entity)
        {
            if (m_entity != null)
            {
                m_entity.HealthComponent.OnHealthChanged -= OnHealthChanged;
            }
            m_entity = entity;
            m_entity.HealthComponent.OnHealthChanged += OnHealthChanged;
            OnHealthChanged();
        }

        private void OnDestroy()
        {
            if (m_entity != null)
            {
                m_entity.HealthComponent.OnHealthChanged -= OnHealthChanged;
            }
        }
        
        private void OnHealthChanged()
        {
            if (m_entity.HealthComponent.MaxHealth == m_entity.HealthComponent.CurrentHealth)
            {
                m_canvasGroup.alpha = 0.0f;
                return;
            }
            
            m_canvasGroup.alpha = 1.0f;
            m_healthFill.fillAmount = m_entity.HealthComponent.HealthPercentage;
        }

        private void Update()
        {
            if (m_lastCameraSize != m_camera3D.orthographicSize || m_lastCameraPosition != m_camera3D.transform.position || m_lastObjectPosition != m_entity.Rigidbody.position)
            {
                transform.localPosition = GetCanvasPosition(m_entity.UIPosition != null ? m_entity.UIPosition.position : m_entity.Rigidbody.position);
                m_lastCameraPosition = m_camera3D.transform.position;
                m_lastObjectPosition = m_entity.Rigidbody.position;
                m_lastCameraSize = m_camera3D.orthographicSize;
            }
        }

        private Vector3 GetCanvasPosition(Vector3 worldPosition)
        {
            Vector2 viewportPoint = m_camera3D.WorldToViewportPoint(worldPosition);
            Vector2 canvasPoint = Vector2.Scale(viewportPoint, m_canvasRect.sizeDelta) - m_canvasRect.sizeDelta / 2.0f;
            return canvasPoint;
        }
    }
}