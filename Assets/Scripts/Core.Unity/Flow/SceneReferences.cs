using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// could use 128-bit int type to serialize, but it would be less legible in text assets


namespace Core.Unity.Flow
{
    /// <summary>SceneReferences contains the GUID to build index mapping, used to reference scenes at runtime.</summary>
    /// <remarks>This is auto-generated on build as a preloaded asset.</remarks>
    public class SceneReferences : ScriptableObject
    {
        ///<summary>The singleton instance, automatically preloaded at runtime.</summary>
#if UNITY_EDITOR
        public static SceneReferences Instance =>
            s_asset ?? (s_asset = CreateInstance<SceneReferences>());
#else
        public static SceneReferences Instance => s_asset;

#endif
        private static SceneReferences s_asset;

        ///<summary>The GUID for each scene asset, indexed by its build index in the array.</summary>
        public string[] Guids => sceneGuids;

        [SerializeField]
        private string[] sceneGuids;

        private Dictionary<string, int> m_cache;

        private void Awake()
        {
#if UNITY_EDITOR
            hideFlags = HideFlags.NotEditable;
            sceneGuids = Array.ConvertAll(EditorBuildSettings.scenes, s => s.guid.ToString());
#endif
            if (sceneGuids == null) sceneGuids = new string[0];
            int n = sceneGuids.Length;
            m_cache = new Dictionary<string, int>(n);
            for (int i = 0; i < n; i++)
                m_cache.Add(sceneGuids[i], i);
            s_asset = this;
        }

        /// <summary>The build index of a scene by GUID. It can be used to load it or to obtain scene info.</summary>
        /// <param name="sceneGuid">The GUID of the scene to look up.</param>
        /// <remarks>All mappings are cached on a dictionary when the asset is preloaded at startup.</remarks>
        public int SceneIndex(string sceneGuid)
        {
            return m_cache.TryGetValue(sceneGuid, out int i) ? i : -1;
        }
    }

    [Serializable]
    public class SceneReference
    {
        /// <summary>The GUID that uniquely identifies this scene asset, used to serialize scene references reliably.</summary>
        /// <remarks>Even if you move/rename the scene asset, GUID references stay valid.</remarks>
        public string guid => sceneGuid;

        [SerializeField]
        private string sceneGuid;

        /// <summary>Create a reference to a scene using its GUID.</summary>
        /// <param name="guid">The GUID of the scene, found in its .scene.meta file, or obtained from AssetDatabase.</param>
        public SceneReference(string guid)
        {
            sceneGuid = guid;
        }

        ///<summary>The build index of this scene, which can be used to load it or to obtain scene info.</summary>
#if UNITY_EDITOR
        public int sceneIndex
        {
            get
            {
                EditorBuildSettingsScene[] s = EditorBuildSettings.scenes;
                for (int i = 0, n = s.Length; i < n; i++)
                    if (s[i].guid.ToString() == sceneGuid)
                        return i;
                return -1;
            }
        }
#else
      public int sceneIndex => SceneReferences.Instance.SceneIndex(sceneGuid);
#endif
    }
}