using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Dev.ComradeVanti.WaitForAnim;
using Game.Combat;
using Game.Combat.Effects;
using Game.Data;
using Game.Input;
using Game.Toy;
using PracticeJam.Game.Combat;
using UnityEngine;

namespace Game.Entity
{
    public class EntityAttackState : AEntityState
    {
        private AbilityDatabase.AbilityDefinition m_ability;

        public EntityAttackState(Entity stateMachine, string ability) : base(stateMachine)
        {
            m_ability = Database.Instance.AbilityDatabase.GetAbility(ability);
        }

        public EntityAttackState(Entity stateMachine, AbilityDatabase.AbilityDefinition ability) : base(stateMachine)
        {
            m_ability = ability;
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

            DoAttack().Forget(OnException);
        }

        public override void ApplyInput(FrameInputData input) { }

        private void OnException(Exception obj)
        {
            Log.Exception(LogCategory.COMBAT, obj.Message);
            throw obj;
        }

        public async UniTask DoAttack()
        {
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

            // Apply OnCast effects
            foreach (AAbilityEffect effect in m_ability.GetEffects((effect) => effect.Timing == Enums.EffectTiming.OnCast))
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
            
            // Play and wait for animation
            if (!string.IsNullOrEmpty(m_ability.AnimationName))
            {
                await Entity.Animator.PlayAndWait(m_ability.AnimationName);   
            }
            
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
                // TODO:
                target.DoDamage(new DamageParams{DamageAmount = m_ability.HealthDecrement, ForceKill = m_ability.InstaKill});
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