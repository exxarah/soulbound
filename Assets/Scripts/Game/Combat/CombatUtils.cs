using System;
using System.Collections.Generic;
using Core.Unity.Utils;
using UnityEngine;

namespace Game.Combat
{
    public static class CombatUtils
    {
        public static List<IDamageable> GetTargets(AttackParams attackParams)
        {
            List<IDamageable> allTargets =  attackParams.TargetType switch
            {
                Enums.TargetType.Raycast => GetTargets_Raycast(attackParams),
                Enums.TargetType.Cone => GetTargets_Cone(attackParams),
                Enums.TargetType.Sphere => GetTargets_Sphere(attackParams),
                Enums.TargetType.Self => GetTargets_Self(attackParams),
                _ => throw new ArgumentOutOfRangeException(),
            };
            
            allTargets.Sort(((a, b) => Proximity(a, b, attackParams.AttackSource)));
            return allTargets.GetRange(0, Math.Min(allTargets.Count, attackParams.TargetMaximumCount));
        }

        private static int Proximity(IDamageable x, IDamageable y, Transform source)
        {
            float distanceX = Vector3.Distance(x.Transform.position, source.position);
            float distanceY = Vector3.Distance(y.Transform.position, source.position);

            return distanceX.CompareTo(distanceY);
        }

        private static List<IDamageable> GetTargets_Self(AttackParams attackParams)
        {
            List<IDamageable> targets = new List<IDamageable>();

            if (attackParams.AttackSource.gameObject.TryGetComponent(out IDamageable target))
            {
                targets.Add(target);
            }
            
            return targets;
        }
        
        private static List<IDamageable> GetTargets_Raycast(AttackParams attackParams)
        {
            throw new NotImplementedException();
            List<IDamageable> targets = new List<IDamageable>();
            
            return targets;
        }

        private static List<IDamageable> GetTargets_Cone(AttackParams attackParams)
        {
            Collider[] coneHits = PhysicsExtensions.ConeCastAll(attackParams.AttackSource.position, attackParams.AttackRange, attackParams.AttackDirection, attackParams.ConeAngleDegrees, attackParams.Layers);
            List<IDamageable> targets = new(attackParams.TargetMaximumCount);
            for (int i = coneHits.Length - 1; i >= 0; i--)
            {
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