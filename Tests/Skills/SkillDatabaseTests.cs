using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;
using System.Collections.Generic;
using System.Tuples;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Components;

namespace Tests.Skills
{
    [TestClass]
    public class SkillDatabaseTests
    {
        private SkillDatabase _skillDatabase;

        [TestInitialize]
        public void Initialize()
        {
            _skillDatabase = new SkillDatabase();
        }

        [TestMethod]
        public void SkillDatabase_CanAddASkill()
        {
            Skill skill = new Skill("Melee Attack");

            _skillDatabase.Add(skill);

            Assert.AreEqual(1, _skillDatabase.Count);
            Assert.AreEqual(skill, _skillDatabase[0]);
        }

        [TestMethod]
        public void SkillDatabase_GivenAnArrayOfSkillNames_CanCreateASkillSet()
        {
            Skill skill = new Skill("Melee Attack");
            Skill skill2 = new Skill("Ranged Attack");
            Skill skill3 = new Skill("Magic Attack");

            _skillDatabase.Add(skill);
            _skillDatabase.Add(skill2);
            _skillDatabase.Add(skill3);

            SkillSet skillSet = _skillDatabase.CreateSkillSet(new string[] {"Melee Attack", "Ranged Attack", "Magic Attack"});
            Assert.AreEqual(3, skillSet.Count);
        }
    }
}
