using UnityEngine;

namespace Core.Unity.Flow
{
    public class LoadingScreen : MonoBehaviour
    {
        public virtual void OnLoadBegin() { }
        public virtual void OnLoadEnd() { }
    }
}