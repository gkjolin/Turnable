using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;

namespace Tests.Characters
{
    [TestClass]
    public class TurnManagerTests
    {
        private Board _board;
        private TurnManager _turnManager;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
            _turnManager = new TurnManager(_board);
        }

        [TestMethod]
        public void TurnManager_Construction_IsSuccessful()
        {
            Board board = LocationsFactory.BuildBoard();

            TurnManager turnManager = new TurnManager(board);

            Assert.AreEqual(turnManager.Board, board);
            Assert.IsNotNull(turnManager.TurnQueue);
            Assert.AreEqual(9, turnManager.TurnQueue.Count);
            Assert.IsTrue(turnManager.TurnQueue[0].IsPlayer);
        }

        [TestMethod]
        public void TurnManager_EndingATurn_MovesTheCharacterToTheEndOfTheTurnQueue()
        {

        }
    }
}
