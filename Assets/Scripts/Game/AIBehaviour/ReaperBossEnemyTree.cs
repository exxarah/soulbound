using System.Collections.Generic;
using Game.AIBehaviour.Conditionals;
using Game.AIBehaviour.Nodes;
using Game.AIBehaviour.Tasks;
using Game.Input;

namespace Game.AIBehaviour
{
    public class ReaperBossEnemyTree : ABehaviourTree
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
                        new IsTrue(this, () => IsDataSet("basic_attack_in_progress")),
                        new FindTargetInRange(this,
                                              () => FindTargetInRange.InActionRange(ControlledEntity.EnemiesLayer, ControlledEntity,
                                                  FrameInputData.ActionType.BasicAttack, ControlledEntity.Rigidbody.transform),
                                              () => FindTargetInRange.GetActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack)),
                    }),
                    new TaskDoAction(this, FrameInputData.ActionType.BasicAttack),
                }),
                // Try to find an enemy to attack
                new SequenceNode(this, new List<Node>
                {
                    new FindTargetInRange(this,
                                          () => FindTargetInRange.InSphereRange(ControlledEntity.EnemiesLayer,
                                                                                    FOVRange, ControlledEntity.Rigidbody.transform),
                                          () => FOVRange),
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