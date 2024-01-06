using System.Collections.Generic;
using Game.AIBehaviour.Conditionals;
using Game.AIBehaviour.Nodes;
using Game.AIBehaviour.Tasks;
using Game.Input;
using Game.Toy;
using UnityEngine;

namespace Game.AIBehaviour
{
    public class HealerEnemyTree : ABehaviourTree
    {
        protected override Node SetupTree()
        {
            Node root = new SelectorNode(this, new List<Node>()
            {
                // Do Attack
                new SequenceNode(this, new List<Node>
                {
                    // Selector/OR node, conditions to process an attack
                    new SelectorNode(this, new List<Node>()
                    {
                        //  If they're already doing an attack, they must continue
                        new IsTrue(this, () => Root.IsDataSet("action_in_progress", FrameInputData.ActionType.BasicAttack)),
                        new SequenceNode(this, new List<Node>()
                        {
                            // Must not be on cooldown
                            new IsNotOnCooldown(this, FrameInputData.ActionType.BasicAttack),
                            // Must have a target in range
                            new IsTargetInRange(this,
                                                () => IsTargetInRange.InActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack, ControlledEntity.Rigidbody.transform),
                                                () => IsTargetInRange.GetActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack)),
                        }),
                    }),
                    new TaskDoAction(this, FrameInputData.ActionType.BasicAttack),
                }),
                // Try to find an ally to heal
                new SequenceNode(this, new List<Node>
                {
                    new IsTargetInRange(this,
                                          () => IsTargetInRange.InSphereRange(ControlledEntity.AlliesLayer,
                                                                                    FOVRange, ControlledEntity.Rigidbody.transform),
                                          () => FOVRange,
                                          IsTargetInRange.GetLowestHealth),
                    new TaskGoToTarget(this),
                }),
                // Else idle
                new TaskIdle(this),
            });


            return root;
        }

        private bool IsDataSet(string attackKey)
        {
            object data = Root.GetData(attackKey);
            return data != null && (bool)Root.GetData(attackKey);
        }
    }
}