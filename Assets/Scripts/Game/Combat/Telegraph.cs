using System;
using Game.Data;
using UnityEngine;

namespace Game.Combat
{
    public class Telegraph : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_renderer = new Renderer();

        private Transform m_source = null;

        public void Initialise(AbilityDatabase.AbilityDefinition ability, Transform source)
        {
            if (ability.TelegraphMaterial != null)
            {
                m_renderer.material = ability.TelegraphMaterial;
            }
            transform.localScale = new Vector3(ability.AttackRange, 1.0f, ability.AttackRange);
            transform.position = source.position;
            transform.forward = source.forward;
            m_source =  source;
        }

        private void Update()
        {
            if (m_source == null) { return; }

            transform.position = m_source.position;
            transform.forward = m_source.forward;
        }
    }
}