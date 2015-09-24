using System;
using NUnit.Framework;
using Turnable.Components;
using Turnable.LevelGenerators;
using System.Collections.Generic;
using Turnable.Api;
using Turnable.Utilities;

namespace Tests.LevelGenerators
{
    [TestFixture]
    public class ChunkTests
    {
        [Test]
        public void Chunk_ImplementsTheIBoundedInterface()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 5);
            Chunk chunk = new Chunk(bounds);

            Assert.That(chunk, Is.InstanceOf<IBounded>());
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Rectangle bounds = new Rectangle(new Position(0, 0), 5, 3);
            Chunk chunk = new Chunk(bounds);

            Assert.That(chunk.Bounds, Is.SameAs(bounds));
        }

        [Test]
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

            Assert.That(newChunks.Count, Is.EqualTo(2));
            Assert.That(newChunks[0].Bounds.BottomLeft, Is.EqualTo(new Position(0, 0)));
            Assert.That(newChunks[0].Bounds.Width, Is.EqualTo(3));
            Assert.That(newChunks[0].Bounds.Height, Is.EqualTo(2));
            Assert.That(newChunks[1].Bounds.BottomLeft, Is.EqualTo(new Position(3, 0)));
            Assert.That(newChunks[1].Bounds.Width, Is.EqualTo(4));
            Assert.That(newChunks[1].Bounds.Height, Is.EqualTo(2));
        }

        [Test]
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

            Assert.That(newChunks.Count, Is.EqualTo(2));
            Assert.That(newChunks[0].Bounds.BottomLeft, Is.EqualTo(new Position(0, 0)));
            Assert.That(newChunks[0].Bounds.Width, Is.EqualTo(7));
            Assert.That(newChunks[0].Bounds.Height, Is.EqualTo(1));
            Assert.That(newChunks[1].Bounds.BottomLeft, Is.EqualTo(new Position(0, 1)));
            Assert.That(newChunks[1].Bounds.Width, Is.EqualTo(7));
            Assert.That(newChunks[1].Bounds.Height, Is.EqualTo(2));
        }

        [Test]
        public void SplitChunk_WhenAChunkCannotBeSplitHorizontally_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 1);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 1);

            Assert.That(newChunks.Count, Is.EqualTo(0));
        }

        [Test]
        public void SplitChunk_WhenAChunkCannotBeSplitVertically_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *
            // *
            // *
            Rectangle bounds = new Rectangle(new Position(0, 0), 1, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 1);

            Assert.That(newChunks.Count, Is.EqualTo(0));
        }

        [Test]
        public void SplitChunk_GivenAMinimumHeightWhenBothSplitChunksCannotMeetRequirement_ReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // *******
            // *******
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Horizontal, 2, 2);

            Assert.That(newChunks.Count, Is.EqualTo(0));
        }

        [Test]
        public void SplitChunk_GivenAMinimumWidthRequirementWhenBothSplitChunksCannotMeetRequirementReturnsAnEmptyListOfChunks()
        {
            // Initial Chunk
            // ***
            // ***
            // ***
            Rectangle bounds = new Rectangle(new Position(0, 0), 3, 3);
            Chunk initialChunk = new Chunk(bounds);
            List<Chunk> newChunks = initialChunk.Split(SplitDirection.Vertical, 2, 2);

            Assert.That(newChunks.Count, Is.EqualTo(0));
        }

        [Test]
        public void SplitChunk_GivenAMinimumHeightWhenDistanceIsLessThanThisRequirement_ThrowsAnException()
        {
            // Initial Chunk
            // *******
            // *******
            // *******
            // *******
            Rectangle bounds = new Rectangle(new Position(0, 0), 7, 4);
            Chunk initialChunk = new Chunk(bounds);

            Assert.That(() => initialChunk.Split(SplitDirection.Horizontal, 1, 2), Throws.ArgumentException);
        }

        [Test]
        public void SplitChunk_GivenAMinimumWidthRequirementWhenDistanceIsLessThanThisRequirement_ThrowsAnException()
        {
            // Initial Chunk
            // ****
            // ****
            // ****
            Rectangle bounds = new Rectangle(new Position(0, 0), 4, 3);
            Chunk initialChunk = new Chunk(bounds);

            Assert.That(() => initialChunk.Split(SplitDirection.Vertical, 1, 2), Throws.ArgumentException);
        }
    }
}
