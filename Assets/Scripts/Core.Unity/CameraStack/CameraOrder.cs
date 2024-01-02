using System;

namespace Core.Unity.CameraStack
{
    public enum CameraOrder
    {
        Boot = Int32.MinValue,
        Default = 0,
        
        Environment = 10,
        
        UI = 20,
        
        LoadingScreen = 30,
    }
}