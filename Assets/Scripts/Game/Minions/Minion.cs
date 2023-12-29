using System;
using Core.Unity.Utils;
using Game.AIBehaviour;
using Game.Combat;
using UnityEngine;

namespace Game.Minions
{
    public class Minion : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_renderer = null;

        [SerializeField]
        private SerializableDictionary<Enums.CharmType, Material> m_charmMaterials =
            new SerializableDictionary<Enums.CharmType, Material>();

        [SerializeField]
        private FollowPlayerTree m_behaviourTree = null;

        public void Initialise(Enums.CharmType charmType, Transform creator)
        {
            if (m_charmMaterials.TryGetValue(charmType, out Material material))
            {
                m_renderer.material = material;
            }

            if (m_behaviourTree != null)
            {
                m_behaviourTree.SetToFollow(creator);   
            }
        }
    }
}