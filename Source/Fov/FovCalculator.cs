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
        // http://www.roguebasin.com/index.php?title=Ruby_shadowcasting_implementation

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
                CastLight(startX, startY, 1, 1.0, 0.0, visualRange, multipliers[0, octantIndex], multipliers[1, octantIndex], multipliers[2, octantIndex], multipliers[3, octantIndex], 0);
            }

            return _visiblePositions;
        }

        private void CastLight(int startX, int startY, int row, double startSlope, double endSlope, int visualRange, int xx, int xy, int yx, int yy, int id)
        {
            if (startSlope < endSlope)
            {
                return;
            }

            int visualRangeSquared = visualRange * visualRange;

            for (int j = row; j <= visualRange; j++)
            {
                int dx = -j - 1;
                int dy = -j;
                bool blocked = false;

                while (dx <= 0)
                {
                    dx++;

                    // Translate the dx, dy co-ordinates into map co-ordinates
                    int mx = startX + dx * xx + dy * xy;
                    int my = startY + dx * yx + dy * yy;

                    // leftSlope and rightSlope store the slopes of the left and right of the square we're considering:
                    double leftSlope = CalculateSlope((double)dx, (double)dy, 0.5, -0.5);
                    double rightSlope = CalculateSlope((double)dx, (double)dy, -0.5, 0.5);
                    double newStartSlope = 0.0;

                    if (startSlope <= rightSlope)
                    {
                        continue;
                    }
                    else if (endSlope >= leftSlope)
                    {
                        break;
                    }
                    else
                    {
                        // Our light beam is touching this square; light it
                        if (CalculateVisibleDistance(dx, dy, 0, 0) <= visualRangeSquared)
                        {
                            _visiblePositions.Add(new Position(mx, my));
                        }

                        if (blocked)
                        {
                            if (Level.IsObstacle(mx, my))
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
                            if (Level.IsObstacle(mx, my) && j < visualRange)
                            {
                                // This is a blocking square, start a child scan
                                blocked = true;
                                CastLight(startX, startY, j + 1, startSlope, leftSlope, visualRange, xx, xy, yx, yy, id + 1);
                                newStartSlope = rightSlope;
                            }
                        }
                    }

                    if (blocked)
                    {
                        break;
                    }
                }
            }

            return;
        }
    }
}
