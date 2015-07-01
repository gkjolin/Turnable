using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class ChunkTests
    {
        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Chunk chunk = new Chunk(new Position(0, 0), 5, 3);

            Assert.AreEqual(new Position(0, 0), chunk.Position);
            Assert.AreEqual(5, chunk.Width);
            Assert.AreEqual(3, chunk.Height);
        }

        [TestMethod]
        public void SplitChunk_WhenSplittingVertically_SplitsTheChunkIntoTwoChunksAlongAVerticalLineAtTheGivenDistance()
        {
            // *******
            // *******
            // New chunks:
            // ***|****
            // ***|****
            Chunk initialChunk = new Chunk(new Position(0, 0), 7, 2);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 3);

            Assert.AreEqual(2, newChunks.Count);
            Assert.AreEqual(new Position(0, 0), newChunks[0].Position);
            Assert.AreEqual(3, newChunks[0].Width);
            Assert.AreEqual(2, newChunks[0].Height);
            Assert.AreEqual(new Position(3, 0), newChunks[1].Position);
            Assert.AreEqual(4, newChunks[1].Width);
            Assert.AreEqual(2, newChunks[1].Height);
        }

        [TestMethod]
        public void SplitChunk_WhenSplittingHorizontally_SplitsTheChunkIntoTwoChunksAlongAHorizontalLineAtTheGivenDistance()
        {
            // *******
            // *******
            // *******
            // New chunks:
            // *******
            // -------
            // *******
            // *******
            Chunk initialChunk = new Chunk(new Position(0, 0), 7, 3);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 1);

            Assert.AreEqual(2, newChunks.Count);
            Assert.AreEqual(new Position(0, 0), newChunks[0].Position);
            Assert.AreEqual(7, newChunks[0].Width);
            Assert.AreEqual(1, newChunks[0].Height);
            Assert.AreEqual(new Position(0, 1), newChunks[1].Position);
            Assert.AreEqual(7, newChunks[1].Width);
            Assert.AreEqual(2, newChunks[1].Height);
        }

    }
}
