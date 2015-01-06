using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Components
{
    // Notes and example on implementing IEquatable<Position>: http://msdn.microsoft.com/en-us/library/ms131190%28v=vs.110%29.aspx
    public class Position : IComponent, IEquatable<Position>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {
        }

        protected Position(Position other)
        {
            X = other.X;
            Y = other.Y;
        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Position Copy()
        {
            return new Position(this);
        }

        public bool Equals(Position other)
        {
            if (other == null) return false;

            return (this.X == other.X && this.Y == other.Y);
        }

        public override bool Equals(Object other)
        {
            Position otherPosition = other as Position;

            if (otherPosition == null) 
            {
                return false;
            }
            else
            {
                return Equals(otherPosition);
            }
        }

        public static bool operator ==(Position position1, Position position2)
        {
            //if ((object)person1 == null || ((object)person2) == null)
            //    return Object.Equals(person1, person2);

            return position1.Equals(position2);
        }

        public static bool operator !=(Position position1, Position position2)
        {
            //if (person1 == null || person2 == null)
            //    return !Object.Equals(person1, person2);

            return !(position1.Equals(position2));
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
