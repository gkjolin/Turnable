using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;

namespace TurnItUp.Fov
{
    public class FovCalculator
    {
        public Level Level { get; set; }

        public FovCalculator(Level level)
        {
            Level = level;
        }

        public double CalculateSlope(double x1, double y1, double x2, double y2, bool inverse = false)
        {
            if (!inverse)
            {
                return ((x1 - x2) / (y1 - y2));
            }
            else
            {
                return 1.0 / CalculateSlope(x1, y1, x2, y2);
            }
        }

        public int CalculateVisibleDistance(int x1, int y1, int x2, int y2)
        {
            return ((x1 - x2) * (x1 - x2)) + ((y1 - y2) * (y1 - y2));
        }

        public List<Position> CalculateVisiblePositions(int visualRange, int originX, int originY)
        {
            var returnValue = new List<Position>();

            if (visualRange == 0)
            {
                returnValue.Add(new Position(originX, originY));
            }

            List<int> visibleOctants = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };

            foreach (int octantIndex in visibleOctants)
            {
                returnValue.AddRange(CalculateVisiblePositionsInOctant(visualRange, originX, originY, 1, octantIndex, 1.0, 0.0));
            }

            return returnValue;
        }

        protected List<Position> CalculateVisiblePositionsInOctant(int visualRange, int originX, int originY, int scanDepth, int octantIndex, double startSlope, double endSlope)
        {
            List<Position> returnValue = new List<Position>();

            int visualRangeSquared = visualRange * visualRange;
            int x = 0;
            int y = 0;

            switch (octantIndex)
            {
                case 1: //nnw
                    y = originY - scanDepth;
                    x = originX - Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (y < 0) y = 0;
                    if (x < 0) x = 0;

                    while (CalculateSlope(x, y, originX, originY, false) >= endSlope)
                    {
                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {
                            if (Level.IsObstacle(x, y)) //cell blocked
                            {
                                if (    //if prior cell open AND within range
                                        x - 1 >= 0
                                        && CalculateVisibleDistance(x - 1, y, originX, originY) <= visualRangeSquared
                                        && !(Level.IsObstacle(x -1 , y))
                                   )
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x - 0.5, y + 0.5, originX, originY, false));
                            }
                            else
                            {

                                if (    //if prior open AND within range
                                        x - 1 >= 0
                                        && CalculateVisibleDistance(x - 1, y, originX, originY) <= visualRangeSquared
                                        && Level.IsObstacle(x -1, y))
                                    startSlope = CalculateSlope(x - 0.5, y - 0.5, originX, originY, false);

                            }
                            returnValue.Add(new Position(x, y));
                        }
                        x++;
                    }
                    x--;
                    break;

                case 2: //nne

                    y = originY - scanDepth;
                    x = originX + Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x >= map.GetLength(0)) x = map.GetLength(0) - 1;
                    if (y < 0) y = 0;

                    while (CalculateSlope(x, y, originX, originY, false) <= endSlope)
                    {
                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {
                            if (map[x, y] == 1)
                            {
                                if (x + 1 < map.GetLength(0)
                                        && CalculateVisibleDistance(x + 1, y, originX, originY) <= visualRangeSquared
                                        && map[x + 1, y] == 0
                                    )
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x + 0.5, y + 0.5, originX, originY, false));
                            }
                            else
                            {
                                if (
                                        x + 1 < map.GetLength(0)
                                        && CalculateVisibleDistance(x + 1, y, originX, originY) <= visualRangeSquared
                                        && map[x + 1, y] == 1
                                    )
                                    startSlope = -CalculateSlope(x + 0.5, y - 0.5, originX, originY, false);


                            }
                            returnValue.Add(new Position(x, y));
                        }
                        x--;
                    }
                    x++;
                    break;

                case 3:

                    x = originX + scanDepth;
                    y = originY - Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x >= map.GetLength(0)) x = map.GetLength(0) - 1;
                    if (y < 0) y = 0;

                    while (CalculateSlope(x, y, originX, originY, true) <= endSlope)
                    {

                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {

                            if (map[x, y] == 1)
                            {
                                if (y - 1 >= 0
                                        && CalculateVisibleDistance(x, y - 1, originX, originY) <= visualRangeSquared
                                        && map[x, y - 1] == 0
                                    )
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x - 0.5, y - 0.5, originX, originY, true));
                            }
                            else
                            {
                                if (y - 1 >= 0
                                       && CalculateVisibleDistance(x, y - 1, originX, originY) <= visualRangeSquared
                                       && map[x, y - 1] == 1
                                   )
                                    startSlope = -CalculateSlope(x + 0.5, y - 0.5, originX, originY, true);


                            }
                            returnValue.Add(new Position(x, y));

                        }
                        y++;
                    }
                    y--;
                    break;

                case 4:

                    x = originX + scanDepth;
                    y = originY + Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));


                    if (x >= map.GetLength(0)) x = map.GetLength(0) - 1;
                    if (y >= map.GetLength(1)) y = map.GetLength(1) - 1;

                    while (CalculateSlope(x, y, originX, originY, false) >= endSlope)
                    {

                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {

                            if (map[x, y] == 1)
                            {
                                if (y + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x, y + 1, originX, originY) <= visualRangeSquared
                                        && map[x, y + 1] == 0)
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x - 0.5, y + 0.5, originX, originY, true));
                            }
                            else
                            {
                                if (y + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x, y + 1, originX, originY) <= visualRangeSquared
                                        && map[x, y + 1] == 1
                                    )
                                    startSlope = CalculateSlope(x + 0.5, y + 0.5, originX, originY, true);


                            }
                            returnValue.Add(new Position(x, y));

                        }
                        y--;
                    }
                    y++;
                    break;

                case 5:

                    y = originY + scanDepth;
                    x = originX + Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x >= map.GetLength(0)) x = map.GetLength(0) - 1;
                    if (y >= map.GetLength(1)) y = map.GetLength(1) - 1;

                    while (CalculateSlope(x, y, originX, originY, false) >= endSlope)
                    {
                        if (CalculateVisibleDistance(x, y, originX, originY) <= VisualRange * VisualRange)
                        {

                            if (map[x, y] == 1)
                            {
                                if (x + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x + 1, y, originX, originY) <= visualRangeSquared
                                        && map[x + 1, y] == 0)
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x + 0.5, y - 0.5, originX, originY, false));

                            }
                            else
                            {
                                if (x + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x + 1, y, originX, originY) <= visualRangeSquared
                                        && map[x + 1, y] == 1)
                                    startSlope = CalculateSlope(x + 0.5, y + 0.5, originX, originY, false);

                                returnValue.Add(new Position(x, y));
                            }

                        }
                        x--;
                    }
                    x++;
                    break;

                case 6:

                    y = originY + scanDepth;
                    x = originX - Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x < 0) x = 0;
                    if (y >= map.GetLength(1)) y = map.GetLength(1) - 1;

                    while (CalculateSlope(x, y, originX, originY, false) <= endSlope)
                    {
                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {

                            if (map[x, y] == 1)
                            {
                                if (x - 1 >= 0
                                        && CalculateVisibleDistance(x - 1, y, originX, originY) <= visualRangeSquared
                                        && map[x - 1, y] == 0)
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x - 0.5, y - 0.5, originX, originY, false));
                            }
                            else
                            {
                                if (x - 1 >= 0
                                        && CalculateVisibleDistance(x - 1, y, originX, originY) <= visualRangeSquared
                                        && map[x - 1, y] == 1)
                                    startSlope = -CalculateSlope(x - 0.5, y + 0.5, originX, originY, false);

                                returnValue.Add(new Position(x, y));
                            }

                        }
                        x++;
                    }
                    x--;
                    break;

                case 7:

                    x = originX - scanDepth;
                    y = originY + Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x < 0) x = 0;
                    if (y >= map.GetLength(1)) y = map.GetLength(1) - 1;

                    while (CalculateSlope(x, y, originX, originY, true) <= endSlope)
                    {

                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {

                            if (map[x, y] == 1)
                            {
                                if (y + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x, y + 1, originX, originY) <= visualRangeSquared
                                        && map[x, y + 1] == 0)
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x + 0.5, y + 0.5, originX, originY, true));
                            }
                            else
                            {
                                if (y + 1 < map.GetLength(1)
                                        && CalculateVisibleDistance(x, y + 1, originX, originY) <= visualRangeSquared
                                        && map[x, y + 1] == 1)
                                    startSlope = -CalculateSlope(x - 0.5, y + 0.5, originX, originY, true);

                                returnValue.Add(new Position(x, y));
                            }

                        }
                        y--;
                    }
                    y++;
                    break;

                case 8: //wnw

                    x = originX - scanDepth;
                    y = originY - Convert.ToInt32((startSlope * Convert.ToDouble(scanDepth)));

                    if (x < 0) x = 0;
                    if (y < 0) y = 0;

                    while (CalculateSlope(x, y, originX, originY, true) >= endSlope)
                    {

                        if (CalculateVisibleDistance(x, y, originX, originY) <= visualRangeSquared)
                        {

                            if (map[x, y] == 1)
                            {
                                if (y - 1 >= 0
                                        && CalculateVisibleDistance(x, y - 1, originX, originY) <= visualRangeSquared
                                        && map[x, y - 1] == 0)
                                    CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, CalculateSlope(x + 0.5, y - 0.5, originX, originY, true));

                            }
                            else
                            {
                                if (y - 1 >= 0
                                        && CalculateVisibleDistance(x, y - 1, originX, originY) <= visualRangeSquared
                                        && map[x, y - 1] == 1)
                                    startSlope = CalculateSlope(x - 0.5, y - 0.5, originX, originY, true);

                                returnValue.Add(new Position(x, y));
                            }

                        }
                        y++;
                    }
                    y--;
                    break;
            }

            if (x < 0)
            {
                x = 0;
            }
            else if (x >= map.GetLength(0))
            {
                x = map.GetLength(0) - 1;
            }

            if (y < 0)
            {
                y = 0;
            }
            else if (y >= map.GetLength(1))
            {
                y = map.GetLength(1) - 1;
            }

            if (scanDepth < visualRange & map[x, y] == 0)
            {
                CalculateVisiblePositionsInOctant(visualRange, originX, originY, scanDepth + 1, octantIndex, startSlope, endSlope);
            }
        }
    }
}
