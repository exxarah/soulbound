using System;
using System.Collections.Generic;
using Game.Data;
using Game.Input;
using UnityEngine;

namespace Game.AIBehaviour.Conditionals
{
    public class FindTargetInRange : Node
    {
        private Func<Transform> m_inRangeFunc;

        public FindTargetInRange(ABehaviourTree tree, Func<Transform> inRangeFunc) : base(tree)
        {
            m_inRangeFunc = inRangeFunc;
        }

        public FindTargetInRange(ABehaviourTree tree, List<Node> children, Func<Transform> inRangeFunc) :
            base(tree, children)
        {
            m_inRangeFunc = inRangeFunc;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                // Check if we can find a new target
                Transform newTarget = m_inRangeFunc?.Invoke();
                if (newTarget != null)
                {
                    Tree.Root.SetData("target", newTarget);
                    State = NodeState.Success;
                    return State;
                }

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
            return InSphereRange(layerMask, ability.AttackRange, source);
        }

        public static Transform InActionRange(int layerMask, FrameInputData.ActionType action, Transform source)
        {
            // TODO: Get currently equipped ability in given action slot, and pass through to InAbilityRange
            return null;
        }
    }
}