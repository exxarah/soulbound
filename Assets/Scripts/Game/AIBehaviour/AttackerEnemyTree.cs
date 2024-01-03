﻿using System.Collections.Generic;
using Game.AIBehaviour.Conditionals;
using Game.AIBehaviour.Nodes;
using Game.AIBehaviour.Tasks;
using Game.Input;

namespace Game.AIBehaviour
{
    public class AttackerEnemyTree : ABehaviourTree
    {
        protected override Node SetupTree()
        {
            Node root = new SelectorNode(this, new List<Node>()
            {
                // Do Attack
                new SequenceNode(this, new List<Node>
                {
                    new IsNotOnCooldown(this, FrameInputData.ActionType.BasicAttack),
                    new IsTargetInRange(this,
                                          () => IsTargetInRange.InActionRange(ControlledEntity.EnemiesLayer, ControlledEntity,
                                              FrameInputData.ActionType.BasicAttack, ControlledEntity.Rigidbody.transform),
                                          () => IsTargetInRange.GetActionRange(ControlledEntity, FrameInputData.ActionType.BasicAttack)),
                    new TaskDoAction(this, FrameInputData.ActionType.BasicAttack),
                }),
                // Try to find an enemy to attack
                new SequenceNode(this, new List<Node>
                {
                    new IsTargetInRange(this,
                                          () => IsTargetInRange.InSphereRange(ControlledEntity.EnemiesLayer,
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