using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Turnable.LevelGenerators;
using Turnable.Components;
using System.Collections.Generic;

namespace Tests.LevelGenerators
{
    [TestClass]
    public class DungeonGeneratorTests
    {
        private DungeonGenerator _dungeonGenerator;

        [TestInitialize]
        public void Initialize()
        {
            _dungeonGenerator = new DungeonGenerator();
        }

        [TestMethod]
        public void GenerateFrom_GivenAnInitialChunk_BreaksUpThatChunkRandomly()
        {
            Chunk initialChunk = new Chunk(new Position(0, 0), 100, 100);

            List<Chunk> randomChunks = _dungeonGenerator.GenerateFrom(initialChunk);
        }
    }
}
