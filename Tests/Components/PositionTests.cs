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
    }
}
