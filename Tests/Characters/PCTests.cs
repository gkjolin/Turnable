using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;
using TurnItUp.Skills;
using TurnItUp.Stats;

namespace Tests.Characters
{
    [TestClass]
    public class PCTests
    {
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
        }

        [TestMethod]
        public void PC_CreatingFromATemplate_IsCreatedWithTheCorrectComponents()
        {
            Entity pc = _world.CreateEntityFromTemplate<PC>();

            Assert.AreEqual(5, pc.Components.Count);
            Assert.IsTrue(pc.Components.ContainsKey(typeof(OnLevel)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(Position)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(InTeam)));
            Assert.AreEqual("PCs", pc.GetComponent<InTeam>().Name);

            // Is there a StatManager?
            StatManager statManager = pc.GetComponent<StatManager>();
            Assert.AreEqual(1, statManager.Stats.Count);
            Stat stat = pc.GetComponent<StatManager>().GetStat("Health");
            Assert.AreEqual(100, stat.Value);
            Assert.AreEqual(0, stat.MinimumValue);
            Assert.AreEqual(100, stat.MaximumValue);
            Assert.IsTrue(stat.IsHealth);

            // Is there an empty SkillSet?
            Assert.IsTrue(pc.Components.ContainsKey(typeof(SkillSet)));
            Assert.AreEqual(0, pc.GetComponent<SkillSet>().Count);
        }
    }
}
