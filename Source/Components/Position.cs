using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Components
{
    public class Position : IComponent
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Entity Owner { get; set; }

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

        public override string ToString()
        {
            return String.Format("({0}, {1})", X.ToString(), Y.ToString());
        }

        public Position InDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return new Position(X, Y + 1);
                case Direction.South:
                    return new Position(X, Y - 1);
                case Direction.West:
                    return new Position(X - 1, Y);
                case Direction.East:
                    return new Position(X + 1, Y);
                case Direction.NorthEast:
                    return new Position(X + 1, Y + 1);
                case Direction.NorthWest:
                    return new Position(X - 1, Y + 1);
                case Direction.SouthEast:
                    return new Position(X + 1, Y - 1);
                case Direction.SouthWest:
                    return new Position(X - 1, Y - 1);
                default:
                    return null;
            }
        }
    }
}