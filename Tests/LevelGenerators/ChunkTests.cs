using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Api;
using Turnable.Utilities;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class ChunkTests
    {
        [TestMethod]
        public void Chunk_ImplementsTheIBoundedInterface()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 5);
            Chunk chunk = new Chunk(bounds);

            Assert.IsInstanceOfType(chunk, typeof(IBounded));
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 5, 3);
            Chunk chunk = new Chunk(bounds);

            Assert.AreEqual(chunk.Bounds, bounds);
        }

        [TestMethod]
        public void SplitChunk_WhenSplittingVertically_SplitsTheChunkIntoTwoChunksAlongAVerticalLineAtTheGivenDistance()
        {
            // Initial Chunk
            // *******
            // *******
            // New chunks
            // ***|****
            // ***|****
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 2);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 3);

            Assert.AreEqual(2, newChunks.Count);
            Assert.AreEqual(new Position(0, 0), newChunks[0].Bounds.TopLeft);
            Assert.AreEqual(3, newChunks[0].Bounds.Width);
            Assert.AreEqual(2, newChunks[0].Bounds.Height);
            Assert.AreEqual(new Position(3, 0), newChunks[1].Bounds.TopLeft);
            Assert.AreEqual(4, newChunks[1].Bounds.Width);
            Assert.AreEqual(2, newChunks[1].Bounds.Height);
        }

        [TestMethod]
        public void SplitChunk_WhenSplittingHorizontally_SplitsTheChunkIntoTwoChunksAlongAHorizontalLineAtTheGivenDistance()
        {
            // Initial Chunk
            // *******
            // *******
            // *******
            // New chunks:
            // *******
            // -------
            // *******
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 1);

            Assert.AreEqual(2, newChunks.Count);
            Assert.AreEqual(new Position(0, 0), newChunks[0].Bounds.TopLeft);
            Assert.AreEqual(7, newChunks[0].Bounds.Width);
            Assert.AreEqual(1, newChunks[0].Bounds.Height);
            Assert.AreEqual(new Position(0, 1), newChunks[1].Bounds.TopLeft);
            Assert.AreEqual(7, newChunks[1].Bounds.Width);
            Assert.AreEqual(2, newChunks[1].Bounds.Height);
        }

        [TestMethod]
        public void SplitChunk_WhenAChunkCannotBeSplitHorizontally_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 1);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 1);

            Assert.AreEqual(0, newChunks.Count);
        }

        [TestMethod]
        public void SplitChunk_WhenAChunkCannotBeSplitVertically_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *
            // *
            // *
            Rectangle bounds = new Rectangle(new Position(0, 0), 1, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 1);

            Assert.AreEqual(0, newChunks.Count);
        }

        [TestMethod]
        public void SplitChunk_GivenAMinimumHeightWhenBothSplitChunksCannotMeetRequirement_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *******
            // *******
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 2, 2);

            Assert.AreEqual(0, newChunks.Count);
        }

        [TestMethod]
        public void SplitChunk_GivenAMinimumWidthRequirementWhenBothSplitChunksCannotMeetRequirementReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // ***
            // ***
            // ***
            Rectangle bounds = new Rectangle(new Position(0, 0), 3, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 2, 2);

            Assert.AreEqual(0, newChunks.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitChunk_GivenAMinimumHeightWhenDistanceIsLessThanThisRequirement_ThrowsAnException()
        {
            // Initial Chunk
            // *******
            // *******
            // *******
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 4);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 1, 2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SplitChunk_GivenAMinimumWidthRequirementWhenDistanceIsLessThanThisRequirement_ThrowsAnException()
        {
            // Initial Chunk
            // ****
            // ****
            // ****
            Rectangle bounds = new Rectangle(new Position(0, 0), 4, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 1, 2);
        }
    }
}
