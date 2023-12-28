using System;
using System.Collections.Generic;
using Priority_Queue;

namespace Core.Maths.Path
{
    public class AStarPathFinder : APathFinder
    {
        public AStarPathFinder(Func<Vector2Int, bool> isValidExit, Func<Vector2Int, Vector2Int, int> getDistance) :
            base(isValidExit, getDistance)
        {
        }

        public override Path FindPath(Vector2Int start, Vector2Int end)
        {
            // The set of discovered nodes that may need to be (re-)expanded, sorted by their fScore
            SimplePriorityQueue<Vector2Int, int> openList = new SimplePriorityQueue<Vector2Int, int>();

            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>(); // The preceding position of a node
            Dictionary<Vector2Int, int>
                gScore = new Dictionary<Vector2Int, int>(); // gScore[n] is the cost of the cheapest path from start to n currently known
            // hScore is the estimated distance from a point to the end (use GetDistance Func)
            // fScore is the sum of gScore and hScore, our best guess for how cheap a path going through n would be

            List<Vector2Int> adjacencies = new List<Vector2Int>();
            Vector2Int current = start;

            // Often we want ot path to a specific entity, which blocks. So we have an extra breaking check for if a tile is
            // the neighbour of the end point and the end is invalid, allow that to become the end
            bool endInvalid = !IsValidExit(end);

            openList.Enqueue(start, 0);
            gScore.Add(start, 0);

            while (openList.Count > 0)
            {
                current = openList.Dequeue();
                if (current == end)
                {
                    return BuildPath(cameFrom, current);
                }

                adjacencies = GetAdjacent(current);
                foreach (Vector2Int neighbour in adjacencies)
                {
                    if (endInvalid && neighbour == end)
                    {
                        return BuildPath(cameFrom, current);
                    }

                    if (!IsValidExit(neighbour))
                    {
                        continue;
                    }

                    int possibleG = gScore[current] + GetDistance(current, neighbour);
                    if (!gScore.ContainsKey(neighbour) || possibleG < gScore[neighbour])
                    {
                        // This neighbour is better than the others!
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = possibleG;
                        if (!openList.Contains(neighbour))
                        {
                            openList.Enqueue(neighbour, possibleG);
                        }
                    }
                }
            }

            // We failed, find the closest possible point instead
            Vector2Int closest = FindClosestToEnd(gScore, end);
            return BuildPath(cameFrom, closest);
        }

        private Vector2Int FindClosestToEnd(Dictionary<Vector2Int, int> gScore, Vector2Int end)
        {
            int smallestF = Int32.MaxValue;
            Vector2Int closestPoint = Vector2Int.Zero;
            foreach (KeyValuePair<Vector2Int, int> pair in gScore)
            {
                int f = pair.Value + GetDistance(pair.Key, end);
                if (f < smallestF)
                {
                    smallestF = f;
                    closestPoint = pair.Key;
                }
            }

            return closestPoint;
        }

        private Path BuildPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            Stack<Vector2Int> pathSteps = new Stack<Vector2Int>();
            pathSteps.Push(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                pathSteps.Push(current);
            }

            return new Path { Success = true, Steps = pathSteps };
        }

        private List<Vector2Int> GetAdjacent(Vector2Int centre)
        {
            return new List<Vector2Int>
            {
                new Vector2Int(centre.X - 1, centre.Y),
                new Vector2Int(centre.X + 1, centre.Y),
                new Vector2Int(centre.X, centre.Y - 1),
                new Vector2Int(centre.X, centre.Y + 1),
            };
        }

        private class Cell
        {
            public Vector2Int Position;
            public int CostFromStart; // g
            public int EstimatedCostToEnd; // h
            public int CellCost => CostFromStart + EstimatedCostToEnd; // f
            public Cell Parent; // The node previous to this on our best path
        }
    }
}