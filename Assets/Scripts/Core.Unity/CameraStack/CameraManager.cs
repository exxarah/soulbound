using System;
using System.Linq;
using Core.DataStructure;
using Core.Unity.Flow;
using Core.Unity.Utils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Core.Unity.CameraStack
{
    public class CameraManager : SceneSingleton<CameraManager>
    {
        [SerializeField]
        private UniversalAdditionalCameraData m_urpBaseCamera;

        [SerializeField]
        private SortedList<StackedCamera> m_cameraList;

        private void OnEnable()
        {
            DontDestroyOnLoad(m_urpBaseCamera);
            m_cameraList = new SortedList<StackedCamera>(StackedCameraSorter.Instance);

            FlowManager.Instance.OnViewLoaded += OnViewLoaded;
            FlowManager.Instance.OnViewUnloaded += OnViewUnloaded;
        }

        private void OnDisable()
        {
            FlowManager.Instance.OnViewLoaded -= OnViewLoaded;
            FlowManager.Instance.OnViewUnloaded -= OnViewUnloaded;
        }

        private void Update()
        {
            if (false)
            {
                Log.Info("");
            }
        }

        private void OnViewLoaded(View view)
        {
            // Camera management, since URP got rid of depth-based overlays (╯°□°）╯︵ ┻━┻
            if (view.CameraStack != null && m_urpBaseCamera != null) AddCamera(view.CameraStack);
        }

        private void OnViewUnloaded(View view)
        {
            // Camera management, since URP got rid of depth-based overlays (╯°□°）╯︵ ┻━┻
            if (view.CameraStack != null && m_urpBaseCamera != null) RemoveCamera((CameraStack)view.CameraStack);
        }

        public void SetBaseCamera(UniversalAdditionalCameraData cam)
        {
            m_urpBaseCamera = cam;
        }

        public void AddCamera(CameraStack cameraStack)
        {
            m_cameraList.AddRange(cameraStack.Cameras);
            UpdateCameraList();
        }

        public void AddCamera(UniversalAdditionalCameraData cameraData, CameraOrder cameraOrder)
        {
            for (int i = 0; i < cameraData.cameraStack.Count; i++)
                m_cameraList.Add(new StackedCamera
                {
                    Camera = cameraData.cameraStack[i],
                    CameraOrder = cameraOrder,
                    StackOrder = i
                });
            UpdateCameraList();
        }

        public void RemoveCamera(CameraStack cameraStack)
        {
            foreach (StackedCamera camera in cameraStack.Cameras) m_cameraList.Remove(camera);
            UpdateCameraList();
        }

        public void RemoveCamera(UniversalAdditionalCameraData cameraData)
        {
            foreach (Camera cam in cameraData.cameraStack)
                m_cameraList.FindAndRemove(stackedCamera => stackedCamera.Camera == cam);
            UpdateCameraList();
        }

        private void UpdateCameraList()
        {
            m_urpBaseCamera.cameraStack.Clear();
            m_urpBaseCamera.cameraStack.AddRange(m_cameraList.Select(stackedCamera => (Camera)stackedCamera));
        }

        [Serializable]
        public struct StackedCamera
        {
            public Camera Camera; // The camera to track
            public CameraOrder CameraOrder; // What order this camera's stack is
            public int StackOrder; // What order in the stack this camera is

            public static implicit operator Camera(StackedCamera stackedCamera)
            {
                return stackedCamera.Camera;
            }
        }
    }
}