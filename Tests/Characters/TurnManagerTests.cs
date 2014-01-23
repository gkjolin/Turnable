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
        [TestMethod]
        public void TurnManager_Construction_IsSuccessful()
        {
            Board board = LocationsFactory.BuildBoard();

            TurnManager turnManager = new TurnManager(board.Map.Tilesets["Characters"], board.Map.Layers["Characters"]);

            Assert.IsNotNull(turnManager.TurnQueue);
            Assert.AreEqual(9, turnManager.TurnQueue.Count);
            Assert.IsTrue(turnManager.TurnQueue[0].IsPlayer);
        }
    }
}
