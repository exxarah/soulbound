using System;

namespace Core.Maths.FOV
{

    public class RayCastVisibility : AVisibility
    {
        private Vector2Int m_mapSize;

        public RayCastVisibility(Vector2Int mapSize, Func<Vector2Int, bool> blocksLight,
                                 Func<Vector2Int, bool> setVisible, Func<Vector2Int, Vector2Int, int> getDistance) :
            base(blocksLight, setVisible, getDistance)
        {
            m_mapSize = mapSize;
        }

        public override void Compute(Vector2Int origin, int range)
        {
            SetVisible(origin);

            if (range != 0)
            {
                // cast to the edge of the map by default
                Rect area = new Rect(0, 0, m_mapSize.X, m_mapSize.Y);

                // but limit the area to the rectangle containing the sight radius if one was provided
                if (range >= 0)
                {
                    area = area.GetIntersection(new Rect(origin.X - range, origin.Y - range, range * 2 + 1,
                                                         range * 2 + 1));
                }

                // cast rays towards the top and bottom of the area
                for (int x = area.MinX; x < area.MaxX; x++)
                {
                    TraceLine(origin, x, area.MinY, range);
                    TraceLine(origin, x, area.MaxY - 1, range);
                }

                // and to the left and right
                for (int y = area.MinY + 1; y < area.MaxY; y++)
                {
                    TraceLine(origin, area.MinX, y, range);
                    TraceLine(origin, area.MaxX - 1, y, range);
                }
            }
        }

        private void TraceLine(Vector2Int origin, int x2, int y2, int range)
        {
            int xDiff = x2 - origin.X, yDiff = y2 - origin.Y, xLen = Math.Abs(xDiff), yLen = Math.Abs(yDiff);
            int xInc = Math.Sign(xDiff), yInc = Math.Sign(yDiff) << 16, index = (origin.Y << 16) + origin.X;

            // make sure we walk along the long axis
            if (xLen < yLen)
            {
                Utils.Swap(ref xLen, ref yLen);
                Utils.Swap(ref xInc, ref yInc);
            }

            int errorInc = yLen * 2, error = -xLen, errorReset = xLen * 2;
            // skip the first point (the origin) since it's always visible and should never stop rays
            while (--xLen > 0)
            {
                index += xInc; // advance down the long axis (could be X or Y)
                error += errorInc;
                if (error > 0)
                {
                    error -= errorReset;
                    index += yInc;
                }

                int x = index & 0xFFFF, y = index >> 16;
                if (range >= 0 && GetDistance(origin, new Vector2Int(x, y)) > range)
                    break;
                SetVisible(new Vector2Int(x, y));
                if (BlocksLight(new Vector2Int(x, y)))
                    break;
            }
        }
    }
}