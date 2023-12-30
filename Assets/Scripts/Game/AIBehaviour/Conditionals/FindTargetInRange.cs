using System;
using System.Collections.Generic;
using Game.Combat;
using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.AIBehaviour.Conditionals
{
    public class FindTargetInRange : Node
    {
        private Func<Collider[]> m_findTargetFunc;
        private Func<float> m_getRangeFunc;
        private Func<Collider[], Collider> m_targetPriorityFunc;

        public FindTargetInRange(ABehaviourTree tree, Func<Collider[]> findTargetFunc, Func<float> getRangeFunc, Func<Collider[], Collider> targetPriorityFunc = null) : base(tree)
        {
            m_findTargetFunc = findTargetFunc;
            m_getRangeFunc = getRangeFunc;

            if (targetPriorityFunc == null)
            {
                targetPriorityFunc = GetFirstCollider;
            }
            m_targetPriorityFunc = targetPriorityFunc;
        }

        public FindTargetInRange(ABehaviourTree tree, List<Node> children, Func<Collider[]> findTargetFunc, Func<float> getRangeFunc) :
            base(tree, children)
        {
            m_findTargetFunc = findTargetFunc;
            m_getRangeFunc = getRangeFunc;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target == null)
            {
                // Check if we can find a new target
                Collider[] targets = m_findTargetFunc?.Invoke();
                if (targets == null || targets.Length <= 0)
                {
                    State = NodeState.Failure;
                    return State;
                }

                Transform newTarget = m_targetPriorityFunc(targets).transform;
                if (newTarget != null)
                {
                    Tree.Root.SetData("target", newTarget);
                    State = NodeState.Success;
                    return State;
                }

                State = NodeState.Failure;
                return State;
            }
            
            // Check if the target is dead and clear them if so
            if (target.TryGetComponent(out HealthComponent health) && health.IsDead)
            {
                ClearData("target");
                State = NodeState.Failure;
                return State;
            }
            
            // Make sure the target is still in range
            float targetDistance =
                Vector3.Distance(Tree.ControlledEntity.Rigidbody.transform.position, target.position);
            float maxDistance = m_getRangeFunc.Invoke();
            if (targetDistance > maxDistance)
            {
                ClearData("target");
                State = NodeState.Failure;
                return State;
            }

            State = NodeState.Success;
            return State;
        }

        public static Collider[] InSphereRange(int layerMask, float viewRange, Transform source)
        {
            Collider[] colliders = Physics.OverlapSphere(source.position, viewRange, layerMask);
            // Exclude self
            colliders = Array.FindAll(colliders, collider => collider.transform != source);
            
            return colliders;
        }

        public static Collider[] InAbilityRange(int layerMask, string abilityID, Transform source)
        {
            AbilityDatabase.AbilityDefinition ability = GameContext.Instance.Database.AbilityDatabase.GetAbility(abilityID);
            if (ability == null) { return null; }

            return InSphereRange(layerMask, ability.AttackRange, source);
        }

        public static Collider[] InActionRange(int layerMask, Entity.Entity entity, FrameInputData.ActionType action, Transform source)
        {
            return InAbilityRange(layerMask, entity.AbilitiesComponent.GetAbility(action), source);
        }

        public static float GetAbilityRange(string abilityID)
        {
            AbilityDatabase.AbilityDefinition ability = GameContext.Instance.Database.AbilityDatabase.GetAbility(abilityID);
            if (ability == null) { return 0.0f; }

            return ability.AttackRange;
        }

        public static float GetActionRange(Entity.Entity entity, FrameInputData.ActionType action)
        {
            return GetAbilityRange(entity.AbilitiesComponent.GetAbility(action));
        }

        public static Collider GetFirstCollider(Collider[] colliders)
        {
            return colliders[0];
        }

        public static Collider GetLowestHealth(Collider[] colliders)
        {
            int highestMaxHealth = int.MinValue;
            float lowestHealth = float.MaxValue;
            Collider lowestCollider = null;
            
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out HealthComponent health))
                {
                    if (health.HealthPercentage < lowestHealth)
                    {
                        highestMaxHealth = health.MaxHealth;
                        lowestHealth = health.HealthPercentage;
                        lowestCollider = colliders[i];
                        continue;
                    }

                    if (Math.Abs(health.HealthPercentage - lowestHealth) < float.Epsilon && health.MaxHealth > highestMaxHealth)
                    {
                        highestMaxHealth = health.MaxHealth;
                        lowestHealth = health.HealthPercentage;
                        lowestCollider = colliders[i];
                        continue;
                    }
                }
            }

            return lowestCollider;
        }
    }
}