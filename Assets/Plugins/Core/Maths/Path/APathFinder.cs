using System;
using System.Collections.Generic;

namespace Core.Maths.Path
{

    public abstract class APathFinder
    {
        protected readonly Func<Vector2Int, bool> IsValidExit;
        protected readonly Func<Vector2Int, Vector2Int, int> GetDistance;

        protected APathFinder(Func<Vector2Int, bool> isValidExit, Func<Vector2Int, Vector2Int, int> getDistance)
        {
            IsValidExit = isValidExit;
            GetDistance = getDistance;
        }

        public abstract Path FindPath(Vector2Int start, Vector2Int end);
    }

    public struct Path
    {
        public bool Success;
        public Stack<Vector2Int> Steps;
    }
}