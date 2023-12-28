using System;

namespace Core.Maths.FOV
{
    // http://www.adammil.net/blog/v125_roguelike_vision_algorithms.html
    public abstract class AVisibility
    {
        protected readonly Func<Vector2Int, bool> BlocksLight;
        protected readonly Func<Vector2Int, bool> SetVisible;
        protected readonly Func<Vector2Int, Vector2Int, int> GetDistance;

        public AVisibility(Func<Vector2Int, bool> blocksLight, Func<Vector2Int, bool> setVisible,
                           Func<Vector2Int, Vector2Int, int> getDistance)
        {
            BlocksLight = blocksLight;
            SetVisible = setVisible;
            GetDistance = getDistance;
        }

        /// <param name="origin">The location of the monster whose field of view will be calculated.</param>
        /// <param name="range">The maximum distance from the origin that tiles will be lit.
        /// If equal to -1, no limit will be applied.
        /// </param>
        public abstract void Compute(Vector2Int origin, int range);
    }
}