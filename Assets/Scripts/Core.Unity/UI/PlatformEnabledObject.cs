using Core.Unity.Utils;
using UnityEngine;

namespace Core.Unity.UI
{
    public class PlatformEnabledObject : MonoBehaviour
    {
        [SerializeField]
        private RuntimePlatform[] m_enabledPlatforms;

        private void Awake()
        {
            bool shouldBeEnabled = false;
            foreach (RuntimePlatform platform in m_enabledPlatforms)
            {
                if (platform == Application.platform)
                {
                    shouldBeEnabled = true;
                    break;
                }
            }
            
            gameObject.SetActiveSafe(shouldBeEnabled);
        }
    }
}