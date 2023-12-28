using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.Utils
{
    public static class PhysicsExtensions
    {
        public static Collider[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float coneAngle, int layerMask)
        {
            List<Collider> hits = new List<Collider>(Physics.OverlapSphere(origin, maxRadius, layerMask));

            for (int i = hits.Count - 1; i >= 0; i--)
            {
                // Wtf Unity
                // https://forum.unity.com/threads/spherecastall-returns-0-0-0-for-all-raycasthit-points.428302/
                Vector3 hitPoint = hits[i].ClosestPoint(origin);
                Vector3 directionToHit = (hitPoint - origin).normalized;
                float angleToHit = Vector3.Angle(direction, directionToHit);

                if (angleToHit >= coneAngle)
                {
                    hits.RemoveAt(i);
                }
            }

            return hits.ToArray();
        }
    }
}