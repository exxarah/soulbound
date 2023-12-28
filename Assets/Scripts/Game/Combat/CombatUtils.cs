using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using Game.Combat;
using UnityEngine;

namespace PracticeJam.Game.Combat
{
    public static class CombatUtils
    {
        public static List<IDamageable> GetTargets(AttackParams attackParams)
        {
            return attackParams.TargetType switch
            {
                Enums.TargetType.Raycast => GetTargets_Raycast(attackParams),
                Enums.TargetType.Cone => GetTargets_Cone(attackParams),
                Enums.TargetType.Sphere => GetTargets_Sphere(attackParams),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
        
        private static List<IDamageable> GetTargets_Raycast(AttackParams attackParams)
        {
            List<IDamageable> targets = new List<IDamageable>();
            
            return targets;
        }

        private static List<IDamageable> GetTargets_Cone(AttackParams attackParams)
        {
            Collider[] coneHits = PhysicsExtensions.ConeCastAll(attackParams.AttackSource.position, attackParams.AttackRange, attackParams.AttackDirection, attackParams.ConeAngleDegrees, attackParams.Layers);
            List<IDamageable> targets = new(attackParams.TargetMaximumCount);
            for (int i = coneHits.Length - 1; i >= 0; i--)
            {
                if (targets.Count >= attackParams.TargetMaximumCount)
                {
                    break;
                }
                if (coneHits[i].gameObject.TryGetComponent(out IDamageable damage) && damage.CanBeDamaged())
                {
                    targets.Add(damage);
                }
            }

            return targets;
        }

        private static List<IDamageable> GetTargets_Sphere(AttackParams attackParams)
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackParams.AttackSource.position, attackParams.AttackRange, attackParams.Layers);
            List<IDamageable> targets = new(attackParams.TargetMaximumCount);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (targets.Count >= attackParams.TargetMaximumCount)
                {
                    break;
                }
                if (hitColliders[i].TryGetComponent(out IDamageable damage) && damage.CanBeDamaged())
                {
                    targets.Add(damage);
                }
            }

            return targets;
        }
        
        public struct AttackParams
        {
            public Enums.TargetType TargetType;
            public Transform AttackSource;
            public Vector3 AttackDirection;
            public float AttackRange;
            public float ConeAngleDegrees;
            public int Layers;
            public int TargetMaximumCount;
        }
    }
}