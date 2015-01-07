using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy;
using Tests.Factories;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Components;
using Entropy.Core;

namespace Tests.Components
{
    [TestClass]
    public class PositionTests
    {
        private ILevel _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = new Level();
        }

        [TestMethod]
        public void Position_IsAnEntropyComponent()
        {
            Position position = new Position();

            Assert.IsInstanceOfType(position, typeof(IComponent));
        }

        [TestMethod]
        public void DefaultConstructor_InitializesXAndYTo0()
        {
            Position position = new Position();

            Assert.AreEqual(0, position.X);
            Assert.AreEqual(0, position.Y);
        }

        [TestMethod]
        public void Constructor_InitializesXAndY()
        {
            Position position = new Position(1, 2);

            Assert.AreEqual(1, position.X);
            Assert.AreEqual(2, position.Y);
        }
        
        public void Copy_CreatesADeepClone()
        {
            Position position = new Position(1, 2);
            Position copiedPosition = position.Copy();

            Assert.AreNotEqual(position, copiedPosition);
            Assert.AreEqual(position.X, copiedPosition.X);
            Assert.AreEqual(position.Y, copiedPosition.Y);
        }

        // Equals Tests
        [TestMethod]
        public void Equals_FromIEquatableTInterface_CanComparePositions()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.IsTrue(position.Equals(position2));

            position2 = new Position(2, 3);
            Assert.IsFalse(position.Equals(position2));
        }

        [TestMethod]
        public void Equals_FromIEquatableTInterface_CanComparePositionToNull()
        {
            Position position = new Position(1, 2);

            Assert.IsFalse(position.Equals((Position)null));
        }

        [TestMethod]
        public void Equals_OverridenFromObjectEquals_CanComparePositions()
        {
            Object position = new Position(1, 2);
            Object position2 = new Position(1, 2);

            Assert.IsTrue(position.Equals(position2));

            position2 = new Position(2, 3);
            Assert.IsFalse(position.Equals(position2));
        }

        [TestMethod]
        public void Equals_OverridenFromObjectEquals_CanComparePositionToNull()
        {
            Object position = new Position(1, 2);

            Assert.IsFalse(position.Equals(null));
        }

        [TestMethod]
        public void Equals_OverridenFromObjectEquals_ReturnsFalseIfOtherObjectIsNotAPosition()
        {
            Object position = new Position(1, 2);

            Assert.IsFalse(position.Equals(new Object()));
        }

        [TestMethod]
        public void EqualityOperator_IsImplemented()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.IsTrue(position == position2);
        }

        [TestMethod]
        public void InequalityOperator_IsImplemented()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(2, 3);

            Assert.IsTrue(position != position2);
        }

        [TestMethod]
        public void EqualityOperator_CanComparePositionToNull()
        {
            Position position = null;

            Assert.IsTrue(position == null);
        }

        [TestMethod]
        public void InequalityOperator_CanComparePositionToNull()
        {
            Position position = new Position(1, 2);

            Assert.IsTrue(position != null);
        }

        [TestMethod]
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

            Assert.AreEqual(calculatedHash, position.GetHashCode());
        }

        [TestMethod]
        public void ToString_DisplaysXAndYCoordinates()
        {
            Position position = new Position(4, 5);

            Assert.AreEqual("(4, 5)", position.ToString());
        }

        [TestMethod]
        public void NeighboringPosition_GivenADirection_ReturnsTheNeighboringPositionInThatDirection()
        {
            Position position = new Position(4, 5);

            Position newPosition = position.NeighboringPosition(Direction.North);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 6), newPosition);
            newPosition = position.NeighboringPosition(Direction.NorthEast);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 6), newPosition);
            newPosition = position.NeighboringPosition(Direction.East);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 5), newPosition);
            newPosition = position.NeighboringPosition(Direction.SouthEast);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 4), newPosition);
            newPosition = position.NeighboringPosition(Direction.South);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 4), newPosition);
            newPosition = position.NeighboringPosition(Direction.SouthWest);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(3, 4), newPosition);
            newPosition = position.NeighboringPosition(Direction.West);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(3, 5), newPosition);
            newPosition = position.NeighboringPosition(Direction.North);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 6), newPosition);
        }
    }
}
