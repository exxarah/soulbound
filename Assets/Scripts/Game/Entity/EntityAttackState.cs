using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Dev.ComradeVanti.WaitForAnim;
using Game.Audio;
using Game.Combat;
using Game.Combat.Effects;
using Game.Data;
using Game.Input;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Entity
{
    public class EntityAttackState : AEntityState
    {
        private bool m_attackInProgress = false;
        private FrameInputData.ActionType m_actionType;
        private AbilityDatabase.AbilityDefinition m_ability;
        private Telegraph m_activeTelegraph = null;

        public EntityAttackState(Entity stateMachine, FrameInputData.ActionType action) : base(stateMachine)
        {
            m_actionType = action;
            m_ability = GameContext.Instance.Database.AbilityDatabase.GetAbility(stateMachine.AbilitiesComponent.GetAbility(action));
        }

        public override void Enter()
        {
            base.Enter();

            if (m_ability == null)
            {
                Log.Error("No ability was found. Quitting attack");
                StateMachine.ChangeState(new EntityIdleState(Entity));
                return;
            }

            if (!CanUseAbility())
            {
                Log.Info("Ability is currently unable to be used. Quitting attack");
                StateMachine.ChangeState(new EntityIdleState(Entity));
                return;
            }

            if (!m_ability.HasTelegraph)
            {
                DoAttack().Forget(OnException);   
            }
            else
            {
                // Spawn telegraph
                if (m_ability.TelegraphPrefab != null)
                {
                    m_activeTelegraph = Object.Instantiate(m_ability.TelegraphPrefab, Entity.transform);
                    m_activeTelegraph.Initialise(m_ability, Entity.Rigidbody.transform);
                }
            }
        }

        public override void ApplyInput(FrameInputData input)
        {
            if (m_attackInProgress) { return; }
            
            // Telegraphed attack, check if the input has been released, and if so start the attack
            if (m_ability.HasTelegraph && !input.GetAction(m_actionType))
            {
                DoAttack().Forget(OnException); 
            }
        }
        
        public override void FixedUpdate()
        {
            Entity.Rigidbody.velocity = Vector3.zero;
        }

        private bool CanUseAbility()
        {
            if (Entity.InventoryComponent == null || !Entity.InventoryComponent.CanAfford(m_ability.CharmCost))
            {
                return false;
            }

            if (Entity.IsOnCooldown(m_ability))
            {
                return false;
            }

            return true;
        }

        private void OnException(Exception obj)
        {
            Log.Exception(LogCategory.COMBAT, obj.Message);
            throw obj;
        }

        public async UniTask DoAttack()
        {
            m_attackInProgress = true;
            AudioManager.Instance.Play(SFXAudioDatabase.SFXKey.Attack);

            // Spend charms
            Entity.InventoryComponent.Spend(m_ability.CharmCost);
            
            // Start new cooldown
            Entity.StartCooldown(m_ability);

            // Play and wait for animation
            if (!string.IsNullOrEmpty(m_ability.AnimationName))
            {
                await Entity.Animator.PlayAndWait(m_ability.AnimationName);   
            }
            
            // Snapshot the people to attack
            List<IDamageable> targets = CombatUtils.GetTargets(new CombatUtils.AttackParams
            {
                TargetType = m_ability.TargetType,
                AttackSource = Entity.Rigidbody.transform,
                AttackDirection = Entity.Rigidbody.transform.forward,
                AttackRange = m_ability.AttackRange,
                ConeAngleDegrees = m_ability.ConeDegrees,
                TargetMaximumCount = m_ability.MaxTargets,
                Layers = GetLayer(m_ability),
            });

            
            // Apply OnHit effects (before damage, incase the target dies and can't be referenced anymore)
            foreach (AAbilityEffect effect in m_ability.GetEffects((effect) => effect.Timing == Enums.EffectTiming.OnHit))
            {
                switch (effect.Target)
                {
                    case Enums.EffectTarget.Caster:
                        effect.ApplyToTarget(Entity.Rigidbody.transform, Entity.gameObject);
                        break;
                    case Enums.EffectTarget.Target:
                        foreach (IDamageable target in targets)
                        {
                            effect.ApplyToTarget(target.Transform, Entity.gameObject);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            // Do attack
            foreach (IDamageable target in targets)
            {
                int damage = target.DoDamage(new DamageParams{DamageAmount = m_ability.HealthDecrement, ForceKill = m_ability.InstaKill});
                if (Entity.IsPlayer)
                {
                    GameContext.Instance.GameState.PlayerDamageGiven += damage;
                }
            }
            
            // Clean up telegraph
            if (m_activeTelegraph != null)
            {
                Object.Destroy(m_activeTelegraph.gameObject);
            }
            
            // Return to idle
            StateMachine.ChangeState(new EntityIdleState(Entity));
        }

        private LayerMask GetLayer(AbilityDatabase.AbilityDefinition ability)
        {
            return ability.TargetLayer switch
            {
                Enums.TargetLayer.Enemies => Entity.EnemiesLayer,
                Enums.TargetLayer.Allies => Entity.AlliesLayer,
                Enums.TargetLayer.Both => Entity.AlliesLayer & Entity.EnemiesLayer,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}