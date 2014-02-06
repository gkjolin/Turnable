using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Components;
using Entropy;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Components
{
    [TestClass]
    public class OnBoardTests
    {
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
        }

        [TestMethod]
        public void OnBoard_IsAnEntropyComponent()
        {
            OnBoard onBoard = new OnBoard(_board);

            Assert.IsInstanceOfType(onBoard, typeof(IComponent));
        }

        [TestMethod]
        public void OnBoard_HasADefaultConstructor()
        {
            OnBoard onBoard = new OnBoard();

            Assert.IsNull(onBoard.Board);
        }

        [TestMethod]
        public void OnBoard_Construction_IsSuccessful()
        {
            OnBoard onBoard = new OnBoard(_board);

            Assert.AreEqual(_board, onBoard.Board);
        }
    }
}
