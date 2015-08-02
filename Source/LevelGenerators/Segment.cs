using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Segment
    {
        public Position Start { get; set; }
        public Position End { get; set; }

        public Segment(Position start, Position end)
        {
            if (start.X != end.X && start.Y != end.Y)
            {
                throw new InvalidOperationException();
            }

            Start = start;
            End = end;
        }

        public Position GetRandomPoint()
        {
            Random random = new Random();

            return new Position(random.Next(Math.Min(Start.X, End.X + 1), Math.Max(Start.X, End.X + 1)), 
                random.Next(Math.Min(Start.Y, End.Y + 1), Math.Max(Start.Y, End.Y + 1)));
        }

        public bool IsParallelTo(Segment other)
        {
            return (Start.X == End.X && other.Start.X == other.End.X || Start.Y == End.Y && other.Start.Y == other.End.Y);
        }

        public int DistanceBetween(Segment other)
        {
            if (!IsParallelTo(other))
            {
                throw new ArgumentException();
            }

            if (Start.X == other.Start.X)
            {
                return Math.Abs(other.Start.Y - Start.Y);
            }

            return Math.Abs(other.Start.X - Start.X);
        }
    }
}
