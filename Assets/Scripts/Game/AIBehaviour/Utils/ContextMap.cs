using Core.Extensions;
using UnityEngine;

namespace Game.AIBehaviour.Utils
{
    public class ContextMap
    {
        private float[] m_directionWeights;
        private int m_segments;
        
        public ContextMap(int segments)
        {
            m_segments = segments;
            m_directionWeights = new float[segments];
            m_directionWeights.Fill(0.5f);
        }

        public void Influence(Vector2 direction, float influenceAngleSize = 180.0f)
        {
            Influence(direction, AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f), influenceAngleSize);
        }

        public void Influence(Vector2 direction, AnimationCurve curveFunction, float influenceAngleSize = 180.0f)
        {
            // Combine the weights with the current weights, based on the curve and the angle size
            for (int i = 0; i < m_segments; i++)
            {
                // Calculate the direction of this segment
                Vector2 segmentDirection = GetDirection(i);

                // Calculate the angle of this direction from the influencing direction
                float angle = Vector2.Angle(direction, segmentDirection);

                // Calculate the influencing weight for this segment
                float weight = 0.0f;
                if (angle <= influenceAngleSize / 2.0f)
                {
                    // inverted because smaller angle = better
                    float curvePosition = 1.0f - (angle / (influenceAngleSize / 2.0f));
                    weight = curveFunction.Evaluate(curvePosition);
                }

                // Average the influencing weight and the stored weight
                m_directionWeights[i] = (m_directionWeights[i] + weight) / 2.0f;
            }
        }

        private Vector2 GetDirection(int idx)
        {
            float angle = (360.0f / m_segments) * idx;
            Vector3 rotation = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward;
            return new Vector2(rotation.x, rotation.z).normalized;
        }

        public Vector2 GetDirection()
        {
            int bestIdx = 0;
            for (int i = 0; i < m_directionWeights.Length; i++)
            {
                if (m_directionWeights[i] > m_directionWeights[bestIdx])
                {
                    bestIdx = i;
                }
            }

            return GetDirection(bestIdx);
        }
    }
}