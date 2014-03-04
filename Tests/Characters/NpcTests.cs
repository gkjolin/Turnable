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
using TurnItUp.Skills;
using TurnItUp.Stats;

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

            Assert.AreEqual(6, npc.Components.Count);
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Brain)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(OnBoard)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Position)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(InTeam)));
            Assert.AreEqual("NPCs", npc.GetComponent<InTeam>().Name);

            // Is there a StatManager?
            StatManager statManager = npc.GetComponent<StatManager>();
            Assert.AreEqual(1, statManager.Stats.Count);
            Stat stat = npc.GetComponent<StatManager>().GetStat("Health");
            Assert.AreEqual(100, stat.Value);
            Assert.AreEqual(0, stat.MinimumValue);
            Assert.AreEqual(100, stat.MaximumValue);
            Assert.IsTrue(stat.IsHealth);

            // Is there an empty SkillSet?
            Assert.IsTrue(npc.Components.ContainsKey(typeof(SkillSet)));
            Assert.AreEqual(0, npc.GetComponent<SkillSet>().Count);
        }
    }
}
