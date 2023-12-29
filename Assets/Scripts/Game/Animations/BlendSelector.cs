using UnityEngine;

namespace Game.Animations
{
    public class BlendSelector : StateMachineBehaviour
    {
        [SerializeField]
        private string m_parameterName;

        [SerializeField]
        private float m_minimumValue = 0.0f;

        [SerializeField]
        private float m_maximumValue = 1.0f;

        [SerializeField]
        private AnimationCurve m_distributionCurve;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float randomNormalised = Random.Range(0.0f, 1.0f);
            float randomCurved = m_distributionCurve.Evaluate(randomNormalised);
            float value = m_minimumValue + randomCurved * (m_maximumValue - m_minimumValue);
            
            animator.SetFloat(m_parameterName, value);
        }
    }
}