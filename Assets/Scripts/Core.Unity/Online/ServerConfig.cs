using UnityEngine;

namespace Core.Unity.Online
{
    [CreateAssetMenu(fileName = "Server_", menuName = "Online/New Server Config")]
    public class ServerConfig : ScriptableObject
    {
        [SerializeField]
        private string m_serverURI;
        public string ServerURI => m_serverURI;

        [SerializeField]
        private int m_serverPort;
        public int ServerPort => m_serverPort;
    }
}