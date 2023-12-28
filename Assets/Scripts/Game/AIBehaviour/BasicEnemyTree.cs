using System.Collections.Generic;
using Game.AIBehaviour.Conditionals;
using Game.AIBehaviour.Nodes;
using Game.AIBehaviour.Tasks;
using Game.Input;
using Game.Toy;
using UnityEngine;

namespace Game.AIBehaviour
{
    public class BasicEnemyTree : ABehaviourTree
    {
        protected override Node SetupTree()
        {
            Node root = new SelectorNode(this, new List<Node>()
            {
                // TODO: Add abilities, healing, etc here
                // Do Basic Attack
                new SequenceNode(this, new List<Node>
                {
                    new FindTargetInRange(this,
                                          () => FindTargetInRange.InActionRange(LayerMask.GetMask(Layers.PLAYER), ControlledEntity,
                                                                                    FrameInputData.ActionType.BasicAttack, ControlledEntity.Rigidbody.transform),
                                          () => FindTargetInRange.GetActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack)),
                    new TaskDoAction(this, FrameInputData.ActionType.BasicAttack),
                }),
                // Try to find an enemy to attack
                new SequenceNode(this, new List<Node>
                {
                    new FindTargetInRange(this,
                                          () => FindTargetInRange.InSphereRange(LayerMask.GetMask(Layers.PLAYER),
                                                                                    FOVRange, ControlledEntity.Rigidbody.transform),
                                          () => FOVRange),
                    new TaskGoToTarget(this),
                }),
                // Else idle
                new TaskIdle(this),
            });


            return root;
        }
    }
}