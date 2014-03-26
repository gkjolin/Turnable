using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;

namespace TurnItUp.Fov
{
    public class FovCalculator
    {
        // http://www.roguebasin.com/index.php?title=Improved_Shadowcasting_in_Java

        private List<Position> _visiblePositions;
        public ILevel Level { get; set; }
        private int[,] multipliers = 
        {
            {1,  0,  0, -1, -1,  0,  0,  1},
            {0,  1, -1,  0,  0, -1,  1,  0},    
            {0,  1,  1,  0,  0, -1, -1,  0},
            {1,  0,  0,  1, -1,  0,  0, -1},
        };

        public FovCalculator(ILevel level)
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

        public List<Position> CalculateVisiblePositions(int startX, int startY, int visualRange)
        {
            _visiblePositions = new List<Position>();

            _visiblePositions.Add(new Position(startX, startY));

            List<int> octantIndices = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

            foreach (int octantIndex in octantIndices)
            {
                CastLight(startX, startY, 1, 1.0, 0.0, visualRange, multipliers[0, octantIndex], multipliers[1, octantIndex], multipliers[2, octantIndex], multipliers[3, octantIndex]);
            }

            return _visiblePositions;
        }

        private void CastLight(int startX, int startY, int row, double startSlope, double endSlope, int visualRange, int xx, int xy, int yx, int yy)
        {
            double newStartSlope = 0.0;

            if (startSlope < endSlope)
            {
                return;
            }

            bool blocked = false;
            int visualRangeSquared = visualRange * visualRange;

            for (int distance = row; distance <= visualRange && !blocked; distance++)
            {
                int deltaY = -distance;

                for (int deltaX = -distance; deltaX <= 0; deltaX++)
                {
                    int currentX = startX + deltaX * xx + deltaY * xy;
                    int currentY = startY + deltaX * yx + deltaY * yy;
                    float leftSlope = (deltaX - 0.5f) / (deltaY + 0.5f);
                    float rightSlope = (deltaX + 0.5f) / (deltaY - 0.5f);

                    if (!(currentX >= 0 && currentY >= 0 && currentX < Level.Map.Width && currentY < Level.Map.Height) || startSlope < rightSlope)
                    {
                        continue;
                    }
                    else if (endSlope > leftSlope)
                    {
                        break;
                    }

                    // Check if it's within the lightable area and light if needed
                    if ((deltaX * deltaX + deltaY * deltaY) <= visualRangeSquared)
                    {
                        _visiblePositions.Add(new Position(currentX, currentY));
                    }

                    // Previous cell was a blocking one
                    if (blocked)
                    { 
                        if (Level.IsObstacle(currentX, currentY))
                        // Hit a wall
                        { 
                            newStartSlope = rightSlope;
                            continue;
                        }
                        else
                        {
                            blocked = false;
                            startSlope = newStartSlope;
                        }
                    }
                    else
                    {
                        if (Level.IsObstacle(currentX, currentY) && distance < visualRange)
                        // Hit a wall within sight line
                        {
                            blocked = true;
                            CastLight(startX, startY, distance + 1, startSlope, leftSlope, visualRange, xx, xy, yx, yy);
                            newStartSlope = rightSlope;
                        }
                    }
                }
            }

            return;
        }
    }
}
