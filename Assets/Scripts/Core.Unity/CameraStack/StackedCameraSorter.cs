using System.Collections.Generic;

namespace Core.Unity.CameraStack
{
    public class StackedCameraSorter : Singleton<StackedCameraSorter>, IComparer<CameraManager.StackedCamera>
    {
        private StackedCameraSorter()
        {
        }

        public int Compare(CameraManager.StackedCamera x, CameraManager.StackedCamera y)
        {
            int cameraOrderComparison = x.CameraOrder.CompareTo(y.CameraOrder);
            if (cameraOrderComparison != 0) return cameraOrderComparison;
            return x.StackOrder.CompareTo(y.StackOrder);
        }
    }
}