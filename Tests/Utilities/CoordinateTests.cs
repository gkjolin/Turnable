using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using NUnit.Framework;
using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Tests.Utilities
{
    [TestFixture]
    public class CoordinateTests
    {
        [Test]
        public void DefaultConstructor_InitializesXAndYTo0()
        {
            ICoordinate coordinate = new Coordinate();

            Assert.That(coordinate.X, Is.EqualTo(0));
            Assert.That(coordinate.Y, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenXAndY_InitializesXAndY()
        {
            ICoordinate coordinate = new Coordinate(1, 2);

            Assert.That(coordinate.X, Is.EqualTo(1));
            Assert.That(coordinate.Y, Is.EqualTo(2));
        }

        [Test]
        public void Copy_CreatesADeepClone()
        {
            ICoordinate coordinate = new Coordinate(1, 2);
            ICoordinate copiedCoordinate = coordinate.Copy();

            Assert.That(coordinate, Is.Not.SameAs(copiedCoordinate));
            Assert.That(coordinate.X, Is.EqualTo(copiedCoordinate.X));
            Assert.That(coordinate.Y, Is.EqualTo(copiedCoordinate.Y));
        }


        // IEquatable<Coordinate> interface implementation tests
        [Test]
        public void Equals_ImplementedFromIEquatableTInterface_CanCompareCoordinates()
        {
            ICoordinate coordinate1 = new Coordinate(1, 2);
            ICoordinate coordinate2 = new Coordinate(1, 2);

            Assert.That(coordinate1.Equals(coordinate2), Is.True);
        }

        [Test]
        public void Equals_ImplementedFromIEquatableTInterface_CanCompareToNull()
        {
            ICoordinate coordinate = new Coordinate(1, 2);

            Assert.That(coordinate.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_CanCompareCoordinates()
        {
            Object coordinate1 = new Coordinate(1, 2);
            Object coordinate2 = new Coordinate(1, 2);

            Assert.That(coordinate1.Equals(coordinate2), Is.True);

            coordinate2 = new Coordinate(2, 3);
            Assert.That(coordinate1.Equals(coordinate2), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_CanComparePositionToNull()
        {
            Object coordinate = new Coordinate(1, 2);

            Assert.That(coordinate.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_ReturnsFalseIfOtherObjectIsNotACoordinate()
        {
            Object coordinate = new Coordinate(1, 2);

            Assert.That(coordinate.Equals(new Object()), Is.False);
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            ICoordinate coordinate1 = new Coordinate(1, 2);
            ICoordinate coordinate2 = new Coordinate(1, 2);

            Assert.That(coordinate1 == coordinate2, Is.True);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            ICoordinate coordinate1 = new Coordinate(1, 2);
            ICoordinate coordinate2 = new Coordinate(1, 2);

            Assert.That(coordinate1 != coordinate2, Is.True);
        }

        [Test]
        public void EqualityOperator_CanCompareCoordinateToNull()
        {
            ICoordinate coordinate = null;

            Assert.That(coordinate == null, Is.True);
        }

        [Test]
        public void InequalityOperator_CanComparePositionToNull()
        {
            ICoordinate coordinate = new Coordinate(1, 2);

            Assert.That(coordinate != null, Is.True);
        }

        /*

[Test]
public void GetHashCode_IsOverridenToReturnASuitableHashCode()
{
    Position position = new Position(1, 2);
    int calculatedHash;

    unchecked // Overflow is fine, just wrap
    {
        int hash = (int)2166136261;
        // Suitable nullity checks etc, of course :)
        hash = hash * 16777619 ^ position.X.GetHashCode();
        hash = hash * 16777619 ^ position.Y.GetHashCode();
        calculatedHash = hash;
    }

    Assert.That(position.GetHashCode(), Is.EqualTo(calculatedHash));
}

[Test]
public void ToString_DisplaysXAndYCoordinates()
{
    Position position = new Position(4, 5);

    Assert.That(position.ToString(), Is.EqualTo("(4, 5)"));
}

[Test]
public void NeighboringPosition_GivenADirection_ReturnsTheNeighboringPositionInThatDirection()
{
    Position position = new Position(4, 5);

    Position newPosition = position.NeighboringPosition(Direction.North);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(4, 6)));
    newPosition = position.NeighboringPosition(Direction.NorthEast);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(5, 6)));
    newPosition = position.NeighboringPosition(Direction.East);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(5, 5)));
    newPosition = position.NeighboringPosition(Direction.SouthEast);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(5, 4)));
    newPosition = position.NeighboringPosition(Direction.South);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(4, 4)));
    newPosition = position.NeighboringPosition(Direction.SouthWest);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(3, 4)));
    newPosition = position.NeighboringPosition(Direction.West);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(3, 5)));
    newPosition = position.NeighboringPosition(Direction.North);
    Assert.That(newPosition, Is.Not.EqualTo(position));
    Assert.That(newPosition, Is.EqualTo(new Position(4, 6)));
}
*/
    }
}
    /*
            // Notes on overriding GetHashCode: http://msdn.microsoft.com/en-us/library/system.object.gethashcode%28v=vs.110%29.aspx
            public class Position : IComponent, IEquatable<Position>
            {
                public int X { get; private set; }
                public int Y { get; private set; }
                public Entity Owner { get; set; }

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
                    if (other == null)
                    {
                        return false;
                    }

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
                    if ((object)position1 == null || ((object)position2) == null)
                    {
                        return Object.Equals(position1, position2);
                    }

                    return position1.Equals(position2);
                }

                public static bool operator !=(Position position1, Position position2)
                {
                    if ((object)position1 == null || ((object)position2) == null)
                    {
                        return !Object.Equals(position1, position2);
                    }

                    return !(position1.Equals(position2));
                }

                public override int GetHashCode()
                {
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

                public Position NeighboringPosition(Direction direction)
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
    */
