using System;
using Entropy;
using Tests.Factories;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Components;
using Entropy.Core;
using NUnit.Framework;

namespace Tests.Components
{
    [TestFixture]
    public class PositionTests
    {
        [Test]
        public void Position_IsAnEntropyComponent()
        {
            Position position = new Position();

            Assert.That(position, Is.InstanceOf<IComponent>());
        }

        [Test]
        public void DefaultConstructor_InitializesXAndYTo0()
        {
            Position position = new Position();

            Assert.That(position.X, Is.EqualTo(0));
            Assert.That(position.Y, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_InitializesXAndY()
        {
            Position position = new Position(1, 2);

            Assert.That(position.X, Is.EqualTo(1));
            Assert.That(position.Y, Is.EqualTo(2));
        }

        [Test]
        public void Copy_CreatesADeepClone()
        {
            Position position = new Position(1, 2);
            Position copiedPosition = position.Copy();

            Assert.That(position, Is.Not.SameAs(copiedPosition));
            Assert.That(position.X, Is.EqualTo(copiedPosition.X));
            Assert.That(position.Y, Is.EqualTo(copiedPosition.Y));
        }

        // Equals Tests
        [Test]
        public void Equals_FromIEquatableTInterface_CanComparePositions()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.That(position.Equals(position2), Is.True);

            position2 = new Position(2, 3);
            Assert.That(position.Equals(position2), Is.False);
        }

        [Test]
        public void Equals_FromIEquatableTInterface_CanComparePositionToNull()
        {
            Position position = new Position(1, 2);

            Assert.That(position.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanComparePositions()
        {
            Object position = new Position(1, 2);
            Object position2 = new Position(1, 2);

            Assert.That(position.Equals(position2), Is.True);

            position2 = new Position(2, 3);
            Assert.That(position.Equals(position2), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_CanComparePositionToNull()
        {
            Object position = new Position(1, 2);

            Assert.That(position.Equals(null), Is.False);
        }

        [Test]
        public void Equals_OverridenFromObjectEquals_ReturnsFalseIfOtherObjectIsNotAPosition()
        {
            Object position = new Position(1, 2);

            Assert.That(position.Equals(new Object()), Is.False);
        }

        [Test]
        public void EqualityOperator_IsImplemented()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.That(position == position2, Is.True);
        }

        [Test]
        public void InequalityOperator_IsImplemented()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(2, 3);

            Assert.That(position != position2, Is.True);
        }

        [Test]
        public void EqualityOperator_CanComparePositionToNull()
        {
            Position position = null;

            Assert.That(position == null, Is.True);
        }

        [Test]
        public void InequalityOperator_CanComparePositionToNull()
        {
            Position position = new Position(1, 2);

            Assert.That(position != null, Is.True);
        }

        [Test]
        public void GetHashCode_IsOverridenToReturnASuitableHashCode()
        {
            Position position = new Position(1, 2);
            int calculatedHash;

            // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
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
    }
}
