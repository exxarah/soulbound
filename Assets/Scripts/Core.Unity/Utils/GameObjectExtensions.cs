using UnityEngine;

namespace Core.Unity.Utils
{
    public static class GameObjectExtensions
    {
        public static void SetActiveSafe(this GameObject obj, bool isActive)
        {
            if (obj == null) { return; }
            if (obj.activeSelf == isActive) { return; }
            
            obj.SetActive(isActive);
        }
    }
}