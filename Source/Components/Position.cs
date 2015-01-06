using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Components
{
    // Notes and example on implementing IEquatable<Position>: http://msdn.microsoft.com/en-us/library/ms131190%28v=vs.110%29.aspx
    // Notes on overriding GetHashCode: http://msdn.microsoft.com/en-us/library/system.object.gethashcode%28v=vs.110%29.aspx
    public class Position : IComponent, IEquatable<Position>
    {
        public int X { get; private set; }
        public int Y { get; private set; }

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
            if ((object)position1 == null || ((object)position2) == null) return Object.Equals(position1, position2);

            return position1.Equals(position2);
        }

        public static bool operator !=(Position position1, Position position2)
        {
            if ((object)position1 == null || ((object)position2) == null) return !Object.Equals(position1, position2);

            return !(position1.Equals(position2));
        }

        public override int GetHashCode()
        {
            // TODO: Unit test this
            unchecked // Overflow is fine, just wrap
            {
                int hash = (int)2166136261;
                // Suitable nullity checks etc, of course :)
                hash = hash * 16777619 ^ X.GetHashCode();
                hash = hash * 16777619 ^ Y.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
