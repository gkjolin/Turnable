using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;

namespace Tests.Locations
{
    [TestClass]
    public class BoardTests
    {
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = new Board();
            _board.Initialize("../../Fixtures/FullExample.tmx");
        }

        [TestMethod]
        public void Board_Initialization_LoadsMapAndSetsParentReferences()
        {
            Board board = new Board();
            board.Initialize("../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(board.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.IsNotNull(board.TurnManager);
            Assert.AreEqual(9, board.TurnManager.TurnQueue.Count);
            Assert.AreEqual(board, board.TurnManager.Board);
            Assert.IsNotNull(board.CharacterManager);
            Assert.AreEqual(9, board.CharacterManager.Characters.Count);
            Assert.AreEqual(board, board.CharacterManager.Board);
        }

        [TestMethod]
        public void Board_DeterminingObstacles_TakesIntoAccountLayerHavingTrueForIsCollisionProperty()
        {
            // The example board has a "wall" around the entire 15x15 level
            Assert.IsTrue(_board.IsObstacle(0, 0));
            Assert.IsTrue(_board.IsObstacle(0, 1));
            Assert.IsTrue(_board.IsObstacle(1, 0));
            Assert.IsTrue(_board.IsObstacle(2, 0));

            Assert.IsFalse(_board.IsObstacle(1, 1));
        }

        [TestMethod]
        public void Board_DeterminingIfCharacterIsAtAPosition_IsSuccessful()
        {
            Assert.IsTrue(_board.IsCharacterAt(_board.CharacterManager.Characters[0].Position.X, _board.CharacterManager.Characters[0].Position.Y));
        }
    }
}
