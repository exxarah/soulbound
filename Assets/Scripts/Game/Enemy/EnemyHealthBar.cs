using System;
using Core.Unity.Utils;
using Game.Combat;
using Game.Data;
using Game.Input;
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

        [SerializeField]
        private GameObject m_charmableEffect = null;

        private Entity.Entity m_entity;
        private bool m_canBeCharmed = false;
        
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
            m_canBeCharmed = m_entity.TryGetComponent(out CharmableComponent _);
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
            
            AbilityDatabase.AbilityDefinition playerCharm = GameContext.Instance.Database.AbilityDatabase.GetAbility(GameContext.Instance.PlayerEntity.AbilitiesComponent.GetAbility(FrameInputData.ActionType.CharmAbility));
            m_charmableEffect.SetActiveSafe(m_canBeCharmed && m_entity.HealthComponent.HealthPercentage <= playerCharm.TargetMaxHealth);
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