using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;

namespace Tests.Skills
{
    [TestClass]
    public class SkillTests
    {
        [TestMethod]
        public void Skill_Construction_IsSuccessful()
        {
            Skill skill = new Skill();

            // Skills can target self, enemies, walkable locations, non walkable locations
            // Skills can have a range of Infinite, adjacent, in line, in line + diagonal, circle
            // Skills have a number of effects
            Assert.IsTrue((skill.TargetTypes & TargetTypes.Enemies) != 0);
            Assert.IsTrue((skill.RangeTypes & RangeTypes.Adjacent) != 0);
        }
    }
}
