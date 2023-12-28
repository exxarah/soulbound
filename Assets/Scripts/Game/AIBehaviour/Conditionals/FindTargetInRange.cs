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
        private Func<Transform> m_findTargetFunc;
        private Func<float> m_getRangeFunc;

        public FindTargetInRange(ABehaviourTree tree, Func<Transform> findTargetFunc, Func<float> getRangeFunc) : base(tree)
        {
            m_findTargetFunc = findTargetFunc;
            m_getRangeFunc = getRangeFunc;
        }

        public FindTargetInRange(ABehaviourTree tree, List<Node> children, Func<Transform> findTargetFunc, Func<float> getRangeFunc) :
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
                Transform newTarget = m_findTargetFunc?.Invoke();
                if (newTarget != null)
                {
                    Tree.Root.SetData("target", newTarget);
                    State = NodeState.Success;
                    return State;
                }

                State = NodeState.Failure;
                return State;
            }
            
            // Make sure the target is still in range
            float targetDistance =
                Vector3.Distance(Tree.ControlledEntity.Rigidbody.transform.position, target.position);
            float maxDistance = m_getRangeFunc.Invoke();
            if (targetDistance > maxDistance)
            {
                State = NodeState.Failure;
                return State;
            }

            State = NodeState.Success;
            return State;
        }

        public static Transform InSphereRange(int layerMask, float viewRange, Transform source)
        {
            Collider[] colliders = Physics.OverlapSphere(source.position, viewRange, layerMask);
            return colliders.Length > 0 ? colliders[0].transform : null;
        }

        public static Transform InAbilityRange(int layerMask, string abilityID, Transform source)
        {
            AbilityDatabase.AbilityDefinition ability = Database.Instance.AbilityDatabase.GetAbility(abilityID);
            if (ability == null) { return null; }

            return InSphereRange(layerMask, ability.AttackRange, source);
        }

        public static Transform InActionRange(int layerMask, Entity.Entity entity, FrameInputData.ActionType action, Transform source)
        {
            return InAbilityRange(layerMask, entity.AbilitiesComponent.GetAbility(action), source);
        }

        public static float GetAbilityRange(string abilityID)
        {
            AbilityDatabase.AbilityDefinition ability = Database.Instance.AbilityDatabase.GetAbility(abilityID);
            if (ability == null) { return 0.0f; }

            return ability.AttackRange;
        }

        public static float GetActionRange(Entity.Entity entity, FrameInputData.ActionType action)
        {
            return GetAbilityRange(entity.AbilitiesComponent.GetAbility(action));
        }
    }
}