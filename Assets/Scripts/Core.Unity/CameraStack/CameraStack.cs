using System.Collections.Generic;
using UnityEngine;

namespace Core.Unity.CameraStack
{
    public class CameraStack : MonoBehaviour
    {
        [SerializeField]
        private List<CameraManager.StackedCamera> m_cameras = new List<CameraManager.StackedCamera>();

        public IReadOnlyList<CameraManager.StackedCamera> Cameras => m_cameras;

        private void OnValidate()
        {
            for (int i = 0; i < m_cameras.Count; i++)
            {
                CameraManager.StackedCamera currentCamera = m_cameras[i];
                if (currentCamera.StackOrder != i)
                    m_cameras[i] = new CameraManager.StackedCamera
                    {
                        Camera = currentCamera.Camera,
                        CameraOrder = currentCamera.CameraOrder,
                        StackOrder = i
                    };
            }
        }
    }
}