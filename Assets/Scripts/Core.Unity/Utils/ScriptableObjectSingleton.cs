using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Unity.Utils
{
    public class ScriptableObjectSingleton<T> : ScriptableObject where T : ScriptableObjectSingleton<T>
    {
        private static T s_instance;
        public static T Instance
        {
            get
            {
                if (s_instance == null)
                {
                    AsyncOperationHandle<T> op;
                    string assetAddress = typeof(T).Name;
#if UNITY_EDITOR
                    if (EditorApplication.isPlaying)
                    {
                        op = Addressables.LoadAssetAsync<T>(assetAddress);
                    }
                    else
                    {
                        var allAssets = new List<AddressableAssetEntry>(AddressableAssetSettingsDefaultObject.Settings.groups
                                                                           .SelectMany(g => g.entries));
                        AddressableAssetEntry selectedAsset = allAssets.FirstOrDefault(e => e.address == assetAddress);
                        op = Addressables.LoadAssetAsync<T>(selectedAsset?.AssetPath);
#else
                    Addressables.LoadAssetAsync<T>(typeof(T).Name);
#endif
                    }
 
                    s_instance = op.WaitForCompletion(); //Forces synchronous load so that we can return immediately
                }
                return s_instance;
            }
        }
    }
}