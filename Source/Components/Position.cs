using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Components
{
    public class Position : IComponent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Entity Entity { get; set; }

        public Position() : this(0, 0)
        {
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position DeepClone()
        {
            return new Position(X, Y);
        }

        public override bool Equals(object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;

            return this == (Position)obj;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public static bool operator ==(Position position1, Position position2)
        {
            if (object.ReferenceEquals(position1, null))
            {
                return object.ReferenceEquals(position2, null);
            }
            if (object.ReferenceEquals(position2, null))
            {
                return object.ReferenceEquals(position1, null);
            }

            return position1.X == position2.X && position1.Y == position2.Y;
        }

        public static bool operator !=(Position position1, Position position2)
        {
            return !(position1 == position2);
        }
    }
}