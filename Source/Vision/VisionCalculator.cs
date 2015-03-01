using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;

namespace Turnable.Vision
{
    public class VisionCalculator : IVisionCalculator
    {
        // http://www.roguebasin.com/index.php?title=Improved_Shadowcasting_in_Java
        public ILevel Level { get; set; }

        private int[,] multipliers = 
        {
            {1,  0,  0, -1, -1,  0,  0,  1},
            {0,  1, -1,  0,  0, -1,  1,  0},    
            {0,  1,  1,  0,  0, -1, -1,  0},
            {1,  0,  0,  1, -1,  0,  0, -1},
        };

        public VisionCalculator(ILevel level)
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
            List<Position> visiblePositions = new List<Position>();

            visiblePositions.Add(new Position(startX, startY));

            List<int> octantIndices = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };

            foreach (int octantIndex in octantIndices)
            {
                CastLight(visiblePositions, startX, startY, 1, 1.0, 0.0, visualRange, multipliers[0, octantIndex], multipliers[1, octantIndex], multipliers[2, octantIndex], multipliers[3, octantIndex]);
            }

            return visiblePositions.Distinct<Position>().ToList<Position>();
        }

        private void CastLight(List<Position> visiblePositions, int startX, int startY, int row, double startSlope, double endSlope, int visualRange, int xx, int xy, int yx, int yy)
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
                    Position currentPosition = new Position(currentX, currentY);
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
                        visiblePositions.Add(currentPosition);
                    }

                    // Previous cell was a blocking one
                    if (blocked)
                    {
                        if (Level.IsCollidable(currentPosition))
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
                        if (Level.IsCollidable(currentPosition) && distance < visualRange)
                        // Hit a wall within sight line
                        {
                            blocked = true;
                            CastLight(visiblePositions, startX, startY, distance + 1, startSlope, leftSlope, visualRange, xx, xy, yx, yy);
                            newStartSlope = rightSlope;
                        }
                    }
                }
            }

            return;
        }
    }
}
