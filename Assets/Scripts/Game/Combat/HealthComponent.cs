using System;
using Core;
using Core.Unity.Utils;
using PracticeJam.Game.Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Combat
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int m_maxHealth = 50;

        [SerializeField, ReadOnly]
        private int m_currentHealth;

        private bool m_isDead = false;
        
        public int MaxHealth => m_maxHealth;
        public int CurrentHealth => m_currentHealth;
        public float HealthPercentage => m_currentHealth / (float)m_maxHealth;
        public Transform Transform => transform;

        public UnityEvent OnDamaged;
        public UnityEvent OnRestored;
        public UnityEvent OnDeath;

        private void Start()
        {
            m_currentHealth = m_maxHealth;
        }
        
        public bool CanBeDamaged()
        {
            // No damage if dead
            return !m_isDead;
        }

        public void DoDamage(DamageParams @params)
        {
            if (@params.ForceKill)
            {
                ChangeHealth(-CurrentHealth);
                return;
            }
            ChangeHealth(-@params.DamageAmount);
        }

        private void ChangeHealth(int amount)
        {
            int newHealth = Math.Max(Math.Min(m_currentHealth + amount, MaxHealth), 0);
            Log.Info(LogCategory.COMBAT, $"{gameObject.name} health from {m_currentHealth} to {newHealth}");
            if (newHealth < m_currentHealth)
            {
                OnDamaged?.Invoke();
            } else if (newHealth > m_currentHealth)
            {
                OnRestored?.Invoke();
            }

            m_currentHealth = newHealth;
            
            if (m_currentHealth == 0)
            {
                DoDeath();
                OnDeath?.Invoke();
            }
        }

        private void DoDeath()
        {
            m_isDead = true;
        }
    }
}