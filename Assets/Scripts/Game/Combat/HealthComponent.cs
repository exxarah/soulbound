using System;
using Core;
using Core.Unity.Utils;
using Cysharp.Threading.Tasks;
using Dev.ComradeVanti.WaitForAnim;
using Game.Audio;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Combat
{
    public class HealthComponent : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private int m_maxHealth = 50;

        [SerializeField, ReadOnly]
        private int m_currentHealth;

        [SerializeField]
        private int m_maxDamagePerHit = -1;
        public int MaxDamagePerHit => m_maxDamagePerHit;

        [FormerlySerializedAs("m_gameLostOnDeath")]
        [SerializeField]
        private bool m_isPlayer = false;

        [SerializeField]
        private Animator m_animator = null;
        
        [SerializeField]
        private string m_deathAnimState = "";

        [SerializeField]
        private float m_secondsWaitAfterDeath = 2.0f;

        [SerializeField]
        private string m_hurtAnimTrigger = "";

        [SerializeField]
        private SFXAudioDatabase.SFXKey m_onHurtSFX = SFXAudioDatabase.SFXKey.Hurt;

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

        public bool CanBeDamaged(CombatUtils.AttackParams attackParams)
        {
            // No damage if dead
            if (m_isDead) { return false; }

            int minHealth = Mathf.RoundToInt(m_maxHealth * attackParams.TargetMinHealth);
            int maxHealth = Mathf.RoundToInt(m_maxHealth * attackParams.TargetMaxHealth);
            if (m_currentHealth < minHealth || m_currentHealth > maxHealth) { return false; }

            return true;
        }

        public int DoDamage(DamageParams @params)
        {
            if (@params.ForceKill && MaxDamagePerHit < 0)
            {
                return ChangeHealth(-CurrentHealth);
            }

            int damage = MaxDamagePerHit >= 0
                ? -Mathf.Min(@params.DamageAmount, MaxDamagePerHit)
                : -@params.DamageAmount;
            return ChangeHealth(damage);
        }

        private int ChangeHealth(int amount)
        {
            int damage = 0;
            int newHealth = Math.Max(Math.Min(m_currentHealth + amount, MaxHealth), 0);
            Log.Info(LogCategory.COMBAT, $"{gameObject.name} health from {m_currentHealth} to {newHealth}");
            if (newHealth < m_currentHealth)
            {
                if (m_isPlayer)
                {
                    GameContext.Instance.GameState.PlayerDamageTaken += m_currentHealth - newHealth;
                }
                damage = m_currentHealth - newHealth;
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
            return damage;
        }

        private void OnDamaged()
        {
            if (!IsDead)
            {
                m_animator.SetTrigger(m_hurtAnimTrigger);   
                AudioManager.Instance.Play(m_onHurtSFX);
            }
        }

        private void OnRestored()
        {
            
        }

        private void OnDeath()
        {
            m_isDead = true;
            AudioManager.Instance.Play(SFXAudioDatabase.SFXKey.Death);
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

            if (!m_isPlayer)
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