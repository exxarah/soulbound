using System;

namespace PracticeJam.Game.Combat
{
    public static class Enums
    {
        [Serializable]
        public enum TargetType
        {
            Raycast,
            Cone,
            Sphere,
        }
    }
}