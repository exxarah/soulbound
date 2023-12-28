// Public Domain. NO WARRANTIES. License: https://opensource.org/licenses/0BSD

using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Unity.Flow.Editor
{
    internal class SceneReferencesBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        private const string resourcePath = "Assets/_autoBuild_SceneIndicesByGuid.asset";

        public void OnPostprocessBuild(BuildReport report)
        {
            Object[] preloaded = PlayerSettings.GetPreloadedAssets();
            if (preloaded != null)
            {
                int n = preloaded.Length - 1, i = n;
                for (; i >= 0; i--)
                    if (preloaded[i] is SceneReferences)
                        break; // i = Array.FindLastIndex
                if (i >= 0)
                {
                    Object[] newPreloaded = new Object[n];
                    Array.Copy(preloaded, newPreloaded, i);
                    Array.Copy(preloaded, i + 1, newPreloaded, i, n - i);
                    PlayerSettings.SetPreloadedAssets(newPreloaded);
                }
            }

            AssetDatabase.DeleteAsset(resourcePath);
            AssetDatabase.SaveAssets();
        }

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            SceneReferences sceneReferences = ScriptableObject.CreateInstance<SceneReferences>();
            AssetDatabase.CreateAsset(sceneReferences, resourcePath);
            Object[] preloaded = PlayerSettings.GetPreloadedAssets();
            int n = preloaded.Length;
            Array.Resize(ref preloaded, n + 1);
            preloaded[n] = sceneReferences;
            PlayerSettings.SetPreloadedAssets(preloaded);
        }
    }
}