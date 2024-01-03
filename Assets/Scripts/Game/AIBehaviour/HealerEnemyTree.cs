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
                    new SelectorNode(this, new List<Node>()
                    {
                        new IsTrue(this, () => IsDataSet("basic_ability_in_progress")),
                        new IsTargetInRange(this,
                                              () => IsTargetInRange.InActionRange(ControlledEntity.AlliesLayer, ControlledEntity,
                                                  FrameInputData.ActionType.BasicAttack, ControlledEntity.Rigidbody.transform),
                                              () => IsTargetInRange.GetActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack),
                                              IsTargetInRange.GetLowestHealth),
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