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

            Assert.AreEqual(4, pc.Components.Count);
            Assert.IsTrue(pc.Components.ContainsKey(typeof(OnBoard)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(Position)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(InTeam)));
            Assert.AreEqual("PCs", pc.GetComponent<InTeam>().Name);
            Assert.IsTrue(pc.Components.ContainsKey(typeof(SkillSet)));

            // Is there a basic melee attack for the PC?
            SkillSet skillSet = pc.GetComponent<SkillSet>();

            Assert.IsNotNull(skillSet["Melee Attack"]);
            Skill skill = skillSet["Melee Attack"];
            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.Adjacent, skill.RangeType);
            Assert.AreEqual(1, skill.Range);
        }
    }
}
