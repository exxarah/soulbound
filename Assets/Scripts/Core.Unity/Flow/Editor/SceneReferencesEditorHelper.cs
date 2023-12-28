// Public Domain. NO WARRANTIES. License: https://opensource.org/licenses/0BSD

using UnityEditor;
using UnityEngine;

namespace Core.Unity.Flow.Editor
{
    internal class SceneReferencesEditorHelper : ScriptableObject
    {
        [SerializeField]
        internal SceneAsset missingScene, nullScene;
    }
}