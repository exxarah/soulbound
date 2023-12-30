using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Unity.Flow
{
    public class LoadingScreen : MonoBehaviour
    {
        public virtual UniTask OnLoadBegin() { return UniTask.CompletedTask; }
        public virtual UniTask OnLoadEnd() { return UniTask.CompletedTask; }
    }
}