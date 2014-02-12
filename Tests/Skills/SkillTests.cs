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
    public class SkillTests
    {
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
        }

        [TestMethod]
        public void Skill_Construction_IsSuccessful()
        {
            Skill skill = new Skill();

            Assert.IsTrue((skill.TargetTypes & TargetTypes.InAnotherTeam) == TargetTypes.InAnotherTeam);
            Assert.IsTrue((skill.RangeTypes & RangeTypes.Adjacent) == RangeTypes.Adjacent);
            Assert.AreEqual(1, skill.Range);
        }

        // Testing the generation of a skill's Target Map
        // A Target Map is the set of nodes from which an enemy can use the skill on the player

        [TestMethod]
        public void Skill_WithARangeOfAdjacentNodes_GeneratesTheCorrectTargetMap()
        {
            Skill skill = new Skill() { RangeTypes = RangeTypes.Adjacent, TargetTypes = TargetTypes.InAnotherTeam };

            TargetMap targetMap = skill.CalculateTargetMap(_board);

            //Assert.AreEqual(1, targetMap.possibleTargetPositions.Count);
            //Assert.IsTrue(possibleTargetPositions.Contains(new Position(0, 0)));
        }

        // RangeTypes.Any
        // TargetTypes.Any
        // NOT IMPLEMENTED

        // RangeTypes.Any
        // TargetTypes.NotSelf
        // NOT IMPLEMENTED
    }
}
