using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Utilities.Api;

namespace Turnable.Utilities
{
    public class Coordinate : ICoordinate, IEquatable<ICoordinate>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate()
        {
        }

        public Coordinate(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public ICoordinate Copy()
        // http://stackoverflow.com/a/78856
        {
            return new Coordinate(X, Y);
        }

        // Notes and example on implementing IEquatable<Coordinate>: http://msdn.microsoft.com/en-us/library/ms131190%28v=vs.110%29.aspx
        public bool Equals(ICoordinate other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.X == other.X && this.Y == other.Y);
        }

        public override bool Equals(Object other)
        {
            ICoordinate otherCoordinate = other as ICoordinate;

            if (otherCoordinate == null)
            {
                return false;
            }
            else
            {
                return Equals(otherCoordinate);
            }
        }

        public static bool operator ==(Coordinate coordinate1, Coordinate coordinate2)
        {
            if ((object)coordinate1 == null || ((object)coordinate2) == null)
            {
                return Object.Equals(coordinate1, coordinate2);
            }

            return coordinate1.Equals(coordinate2);
        }

        public static bool operator !=(Coordinate coordinate1, Coordinate coordinate2)
        {
            if ((object)coordinate1 == null || ((object)coordinate2) == null)
            {
                return !Object.Equals(coordinate1, coordinate2);
            }

            return !(coordinate1.Equals(coordinate2));
        }

        public override int GetHashCode()
        {
            // http://stackoverflow.com/a/263416
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 486187739 + X.GetHashCode();
                hash = hash * 486187739 + Y.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            // http://stackoverflow.com/a/10278430
            return String.Format("({0}, {1})", X, Y);
        }
    }
}
