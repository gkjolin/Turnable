using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Components;
using Entropy;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Components
{
    [TestClass]
    public class PositionTests
    {
        private Level _level;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
        }

        [TestMethod]
        public void Position_IsAnEntropyComponent()
        {
            Position position = new Position(1, 2);

            Assert.IsInstanceOfType(position, typeof(IComponent));
        }

        [TestMethod]
        public void Position_HasADefaultConstructor()
        {
            Position position = new Position();

            Assert.AreEqual(0, position.X);
            Assert.AreEqual(0, position.Y);
        }

        [TestMethod]
        public void Position_Construction_IsSuccessful()
        {
            Position position = new Position(1, 2);

            Assert.AreEqual(1, position.X);
            Assert.AreEqual(2, position.Y);
        }

        [TestMethod]
        public void Position_DeepCloning_CreatesANewInstance()
        {
            Position position = new Position(1, 2);
            Position clonedPosition = position.DeepClone();

            clonedPosition.X = 2;
            clonedPosition.Y = 3;

            Assert.AreNotEqual(position, clonedPosition);
            Assert.AreEqual(1, position.X);
            Assert.AreEqual(2, position.Y);
        }

        [TestMethod]
        public void Position_ImplementsEquals()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.AreEqual(position, position2);

            position2 = new Position(2, 3);
            Assert.AreNotEqual(position, position2);
        }

        [TestMethod]
        public void Position_ImplementsEqualityOperator()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(1, 2);

            Assert.IsTrue(position == position2);
        }

        [TestMethod]
        public void Position_ImplementsInequalityOperator()
        {
            Position position = new Position(1, 2);
            Position position2 = new Position(2, 3);

            Assert.IsTrue(position != position2);
        }

        [TestMethod]
        public void Position_WhenUsingEqualityOperator_CanBeComparedToNull()
        {
            Position position = null;

            Assert.IsTrue(position == null);
        }

        [TestMethod]
        public void Position_WhenUsingInequalityOperator_CanBeComparedToNull()
        {
            Position position = new Position(1, 2);

            Assert.IsTrue(position != null);
        }

        [TestMethod]
        public void Position_ToString_DisplaysItselfInAHumanReadableFormat()
        {
            Position position = new Position(4, 5);
            Assert.AreEqual("(4, 5)", position.ToString());
        }

        [TestMethod]
        public void Position_ReturningAPositionInACertainDirection_ReturnsANewPosition()
        {
            Position position = new Position(4, 5);

            Position newPosition = position.InDirection(Direction.North);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 6), newPosition);
            newPosition = position.InDirection(Direction.NorthEast);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 6), newPosition);
            newPosition = position.InDirection(Direction.East);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 5), newPosition);
            newPosition = position.InDirection(Direction.SouthEast);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(5, 4), newPosition);
            newPosition = position.InDirection(Direction.South);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 4), newPosition);
            newPosition = position.InDirection(Direction.SouthWest);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(3, 4), newPosition);
            newPosition = position.InDirection(Direction.West);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(3, 5), newPosition);
            newPosition = position.InDirection(Direction.North);
            Assert.AreNotEqual(newPosition, position);
            Assert.AreEqual(new Position(4, 6), newPosition);
        }

    }
}
