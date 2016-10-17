using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Utilities.Api;

namespace Turnable.Utilities
{
    public class Coordinates : ICoordinates, IEquatable<ICoordinates>
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinates()
        {
        }

        public Coordinates(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public ICoordinates Copy()
        // http://stackoverflow.com/a/78856
        {
            return new Coordinates(X, Y);
        }

        // Notes and example on implementing IEquatable<Coordinate>: http://msdn.microsoft.com/en-us/library/ms131190%28v=vs.110%29.aspx
        public bool Equals(ICoordinates other)
        {
            if (other == null)
            {
                return false;
            }

            return (this.X == other.X && this.Y == other.Y);
        }

        public override bool Equals(Object other)
        {
            ICoordinates otherCoordinate = other as ICoordinates;

            if (otherCoordinate == null)
            {
                return false;
            }
            else
            {
                return Equals(otherCoordinate);
            }
        }

        public static bool operator ==(Coordinates coordinates1, Coordinates coordinates2)
        {
            if ((object)coordinates1 == null || ((object)coordinates2) == null)
            {
                return Object.Equals(coordinates1, coordinates2);
            }

            return coordinates1.Equals(coordinates2);
        }

        public static bool operator !=(Coordinates coordinates1, Coordinates coordinates2)
        {
            if ((object)coordinates1 == null || ((object)coordinates2) == null)
            {
                return !Object.Equals(coordinates1, coordinates2);
            }

            return !(coordinates1.Equals(coordinates2));
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
