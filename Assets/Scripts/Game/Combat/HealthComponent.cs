using System;
using Core;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using Dev.ComradeVanti.WaitForAnim;
using UnityEngine;

namespace Game.Combat
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int m_maxHealth = 50;

        [SerializeField, ReadOnly]
        private int m_currentHealth;

        [SerializeField]
        private bool m_gameLostOnDeath = false;

        [SerializeField]
        private Animator m_animator = null;
        
        [SerializeField]
        private string m_deathAnimState = "";

        [SerializeField]
        private float m_secondsWaitAfterDeath = 2.0f;

        [SerializeField]
        private string m_hurtAnimTrigger = "";

        private bool m_isDead = false;
        public bool IsDead => m_isDead;
        
        public int MaxHealth => m_maxHealth;
        public int CurrentHealth => m_currentHealth;
        public float HealthPercentage => m_currentHealth / (float)m_maxHealth;
        public Transform Transform => transform;

        public event Action OnDead;
        public event Action OnHealthChanged;

        private void Start()
        {
            m_currentHealth = m_maxHealth;
            OnHealthChanged?.Invoke();
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
                OnDamaged();
            } else if (newHealth > m_currentHealth)
            {
                OnRestored();
            }

            m_currentHealth = newHealth;
            
            if (m_currentHealth == 0)
            {
                OnDeath();
            }
            
            OnHealthChanged?.Invoke();
        }

        private void OnDamaged()
        {
            if (!IsDead)
            {
                m_animator.SetTrigger(m_hurtAnimTrigger);   
            }
        }

        private void OnRestored()
        {
            
        }

        private void OnDeath()
        {
            m_isDead = true;
            DeathSequence().Forget(OnException);
            OnDead?.Invoke();
        }

        private async UniTask DeathSequence()
        {
            // Play and wait for animation
            if (m_animator != null && !string.IsNullOrEmpty(m_deathAnimState))
            {
                // Don't let the hurt anim trigger, or it will squash the death anim
                m_animator.ResetTrigger(m_hurtAnimTrigger);
                m_animator.Play(m_deathAnimState);
                await new WaitForAnimationToStart(m_animator, m_deathAnimState);
                await new WaitForAnimationToFinish(m_animator, m_deathAnimState);
            }

            if (!m_gameLostOnDeath)
            {
                Destroy(this.gameObject);
                return;
            }

            await UniTask.Delay((int)(m_secondsWaitAfterDeath * 1000));
            
            GameContext.Instance.GameState.EndGame(false);
        }
        
        private void OnException(Exception obj)
        {
            Log.Exception(LogCategory.COMBAT, obj.Message);
            throw obj;
        }
    }
}