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
    public class CoordinatesTests
    {
        [Test]
        public void DefaultConstructor_InitializesXAndYTo0()
        {
            ICoordinates coordinates = new Coordinates();

            Assert.That(coordinates.X, Is.EqualTo(0));
            Assert.That(coordinates.Y, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenXAndY_InitializesXAndY()
        {
            ICoordinates coordinates = new Coordinates(1, 2);

            Assert.That(coordinates.X, Is.EqualTo(1));
            Assert.That(coordinates.Y, Is.EqualTo(2));
        }

        [Test]
        public void Copy_CreatesADeepClone()
        {
            ICoordinates coordinates = new Coordinates(1, 2);
            ICoordinates copiedCoordinate = coordinates.Copy();

            Assert.That(coordinates, Is.Not.SameAs(copiedCoordinate));
            Assert.That(coordinates.X, Is.EqualTo(copiedCoordinate.X));
            Assert.That(coordinates.Y, Is.EqualTo(copiedCoordinate.Y));
        }


        // IEquatable<Coordinate> interface implementation tests
        [Test]
        public void Equals_ImplementedFromIEquatableTInterface_CanCompareCoordinates()
        {
            ICoordinates coordinates1 = new Coordinates(1, 2);
            ICoordinates coordinates2 = new Coordinates(1, 2);

            Assert.That(coordinates1.Equals(coordinates2), Is.True);
        }

        [Test]
        public void Equals_ImplementedFromIEquatableTInterface_CanCompareToNull()
        {
            ICoordinates coordinates = new Coordinates(1, 2);

            Assert.That(coordinates.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_CanCompareCoordinates()
        {
            Object coordinates1 = new Coordinates(1, 2);
            Object coordinates2 = new Coordinates(1, 2);

            Assert.That(coordinates1.Equals(coordinates2), Is.True);

            coordinates2 = new Coordinates(2, 3);
            Assert.That(coordinates1.Equals(coordinates2), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_CanComparePositionToNull()
        {
            Object coordinates = new Coordinates(1, 2);

            Assert.That(coordinates.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectClass_ReturnsFalseIfOtherObjectIsNotACoordinate()
        {
            Object coordinates = new Coordinates(1, 2);

            Assert.That(coordinates.Equals(new Object()), Is.False);
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            Coordinates coordinates1 = new Coordinates(1, 2);
            Coordinates coordinates2 = new Coordinates(1, 2);

            Assert.That(coordinates1 == coordinates2, Is.True);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            Coordinates coordinates1 = new Coordinates(1, 2);
            Coordinates coordinates2 = new Coordinates(2, 3);

            Assert.That(coordinates1 != coordinates2, Is.True);
        }

        [Test]
        public void EqualityOperator_CanCompareCoordinateToNull()
        {
            Coordinates coordinates = null;

            Assert.That(coordinates == null, Is.True);
        }

        [Test]
        public void InequalityOperator_CanComparePositionToNull()
        {
            Coordinates coordinates = new Coordinates(1, 2);

            Assert.That(coordinates != null, Is.True);
        }

        [Test]
        public void GetHashCode_IsOverridenToReturnASuitableHashCode()
        {
            Coordinates coordinates = new Coordinates(1, 2);
            int calculatedHash;

            unchecked 
            {
                calculatedHash = 17;
                // Suitable nullity checks etc, of course :)
                calculatedHash = calculatedHash * 486187739 + coordinates.X.GetHashCode();
                calculatedHash = calculatedHash * 486187739 + coordinates.Y.GetHashCode();
            }

            Assert.That(coordinates.GetHashCode(), Is.EqualTo(calculatedHash));
        }

        [Test]
        public void ToString_DisplaysXAndYCoordinates()
        {
            ICoordinates coordinates = new Coordinates(4, 5);

            Assert.That(coordinates.ToString(), Is.EqualTo("(4, 5)"));
        }

        /*


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
    #1#
*/
