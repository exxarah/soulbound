using System;
using Core.Unity.Utils;
using UnityEngine;

namespace Core.Unity.UI
{
    /// <summary>
    /// Simple utility component for when you want an object enabled in-editor, but then disabled in-game by default 
    /// </summary>
    public class DisableOnAwake : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.SetActiveSafe(false);
        }
    }
}