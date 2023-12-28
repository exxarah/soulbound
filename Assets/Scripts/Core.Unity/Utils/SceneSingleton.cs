using System;
using UnityEngine;

namespace Core.Unity.Utils
{
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                throw new Exception($"An instance of SceneSingleton[{gameObject.name}] already exists");
            }

            Instance = (T)this;
        }
    }
}