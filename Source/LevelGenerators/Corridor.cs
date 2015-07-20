using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Corridor
    {
        public List<Segment> Segments { get; set; }

        public Corridor()
        {
            Segments = new List<Segment>();
        }

        public Corridor(List<Segment> segments)
        {
            Segments = segments;
        }

        public static Corridor Build(Position start, Position end)
        {
            Corridor corridor = new Corridor();
            // TODO: Use a utility class to reorder these positions as well as the positions in construction of a Rectangle
            // Reorder the start and end points of the corridor so that the start is always to left of the end point
            if (start.X > end.X)
            {
                Swap(ref start, ref end);
            }

            Position segmentStart = null;
            Position segmentEnd = null;

            // Move right first (if needed)
            // In order to form a Z shaped corridor, we need to stop moving right about halfway.
            // We use the following calculation which gives these results (horizontal separation between start and end, how far to stop moving right): (2: 1, 3: 2, 4: 2, 5:3, 6:4, 7: 4, 8:4)
            if (start.X != end.X)
            {
                if (start.Y == end.Y)
                {
                    // No need to turn the corridor halfway
                    segmentStart = new Position(start.X + 1, start.Y);
                    segmentEnd = new Position(end.X - 1, start.Y);
                }
                else
                {
                    // Turn the corridor halfway
                    int segmentStartTurnDistance = (int)(Math.Ceiling(((double)end.X - (double)start.X - 1.0d) / 2.0d));
                    segmentStart = new Position(start.X + 1, start.Y);
                    segmentEnd = new Position(segmentStartTurnDistance, start.Y);
                }

                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }

            // Then move up
            if (start.Y < end.Y)
            {
                if (segmentStart == null) // We need a vertical corridor
                {
                    segmentStart = new Position(start.X, start.Y + 1);
                    segmentEnd = new Position(start.X, end.Y - 1);
                }
                else
                {
                    segmentStart = new Position(segmentEnd.X, segmentStart.Y + 1);
                    segmentEnd = new Position(segmentEnd.X, end.Y);
                }
                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }
            // or down
            if (start.Y > end.Y)
            {
                if (segmentStart == null) // We need a vertical corridor
                {
                    segmentStart = new Position(start.X, start.Y - 1);
                    segmentEnd = new Position(start.X, end.Y + 1);
                }
                else
                {
                    segmentStart = new Position(segmentEnd.X, segmentStart.Y - 1);
                    segmentEnd = new Position(segmentEnd.X, end.Y);
                }
                corridor.Segments.Add(new Segment(segmentStart, segmentEnd));
            }

            // If the corridor now reaches to the end, return it.
            if (segmentEnd.X == end.X && (Math.Abs(segmentEnd.Y - end.Y) == 1) || segmentEnd.Y == end.Y && (Math.Abs(segmentEnd.X - end.X) == 1))
            {
                return corridor;
            }

            // Move right again
            segmentStart = new Position(segmentEnd.X + 1, segmentEnd.Y);
            segmentEnd = new Position(end.X - 1, end.Y);
            corridor.Segments.Add(new Segment(segmentStart, segmentEnd));

            return corridor;
        }

        private static void Swap(ref Position firstPosition, ref Position secondPosition)
        {
            Position tempPosition = firstPosition;
            firstPosition = secondPosition;
            secondPosition = tempPosition;
        }
    }
}
