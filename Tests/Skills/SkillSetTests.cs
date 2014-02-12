using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;
using System.Collections.Generic;
using System.Tuples;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Components;
using Entropy;

namespace Tests.Skills
{
    [TestClass]
    public class SkillSetTests
    {
        private SkillSet _skillSet;

        [TestInitialize]
        public void Initialize()
        {
            _skillSet = new SkillSet();
        }

        [TestMethod]
        public void SkillSet_Construction_IsSuccessful()
        {
            SkillSet skillSet = new SkillSet();
        }

        [TestMethod]
        public void SkillSet_IsAnEntropyComponent()
        {
            SkillSet skillSet = new SkillSet();

            Assert.IsInstanceOfType(skillSet, typeof(IComponent));
        }

        [TestMethod]
        public void SkillSet_AddingASkill_IsSuccessful()
        {
            _skillSet.Add(new Skill("Melee Attack"));

            Assert.AreEqual(1, _skillSet.Count);
            Assert.IsNotNull(_skillSet["Melee Attack"]);
        }
    }
}
