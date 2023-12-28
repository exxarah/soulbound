using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Core.Unity.Utils
{
    [Serializable]
    public class SerializableGuid : ISerializationCallbackReceiver
    {
        [SerializeField, ReadOnly]
        private string m_guid;
        public System.Guid Guid;
    
        public void OnBeforeSerialize()
        {
            m_guid = Guid.ToString();
        }
    
        public void OnAfterDeserialize()
        {
            Guid = System.Guid.Parse( m_guid );
        }   
    }
}