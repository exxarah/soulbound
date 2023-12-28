using System;
using System.Collections.Generic;
using Core;
using Cysharp.Threading.Tasks;
using Dev.ComradeVanti.WaitForAnim;
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
                AttackSource = Entity.CharacterController.transform,
                AttackDirection = Entity.CharacterController.transform.forward,
                AttackRange = m_ability.AttackRange,
                ConeAngleDegrees = m_ability.ConeDegrees,
                TargetMaximumCount = m_ability.MaxTargets,
                Layers = LayerMask.GetMask(Layers.ENEMIES),
            });
            
            // Play and wait for animation
            await Entity.Animator.PlayAndWait(m_ability.AnimationName);

            // Do attack
            foreach (IDamageable target in targets)
            {
                // TODO:
                target.DoDamage(new DamageParams{DamageAmount = 10});
            }
            
            // Return to idle
            StateMachine.ChangeState(new EntityIdleState(Entity));
        }
    }
}