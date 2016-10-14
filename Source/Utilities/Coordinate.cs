using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Utilities.Api;

namespace Turnable.Utilities
{
    public class Coordinate : ICoordinate, IEquatable<Coordinate>
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
        // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
        public bool Equals(Coordinate other)
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

    }
}
