using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;
using TurnItUp.AI.Brains;

namespace Tests.Characters
{
    [TestClass]
    public class NpcTests
    {
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
        }

        [TestMethod]
        public void Npc_CreatingFromATemplate_IsCreatedWithTheCorrectComponents()
        {
            Entity npc = _world.CreateEntityFromTemplate<Npc>();

            Assert.AreEqual(3, npc.Components.Count);
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Brain)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(OnBoard)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Position)));
        }
    }
}
