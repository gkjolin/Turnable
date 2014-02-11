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

            // Skills can target Any, Self, NotSelf, WalkableLocation, NonWalkableLocation
            // Skills can have a range of Any, Adjacent, Orthogonal, Diagonal, Circle
            // Skills have a number of effects
            Assert.IsTrue((skill.TargetTypes & TargetTypes.NotSelf) == TargetTypes.NotSelf);
            Assert.IsTrue((skill.RangeTypes & RangeTypes.Adjacent) == RangeTypes.Adjacent);
        }

        // Testing the generation of a skill's Target Map
        // A Target Map is the set of targets that the user can target from each position on the map

        // TargetTypes.Self
        [TestMethod]
        public void Skill_AbleToTargetSelf_GeneratesTheCorrectTargetMapAndIgnoresRange()
        {
            RangeTypes[] rangeTypesChoices = {RangeTypes.Adjacent, RangeTypes.Any, RangeTypes.Circle, RangeTypes.Diagonal, RangeTypes.Orthogonal};

            foreach (RangeTypes rangeTypesChoice in rangeTypesChoices)
            {
                Skill skill = new Skill() { RangeTypes = rangeTypesChoice, TargetTypes = TargetTypes.Self };

                TargetMap targetMap = skill.CalculateTargetMap(new Position(0, 0));

                Assert.AreEqual(1, targetMap.Count);
                Assert.IsTrue(targetMap.ContainsKey(new Tuple<int, int>(0, 0)));

                HashSet<Position> possibleTargetPositions = targetMap[new Tuple<int, int>(0, 0)];
                Assert.AreEqual(1, possibleTargetPositions.Count);
                Assert.IsTrue(possibleTargetPositions.Contains(new Position(0, 0)));
            }
        }

        // RangeTypes.Any
        // TargetTypes.Any
        // NOT IMPLEMENTED

        // RangeTypes.Any
        // TargetTypes.NotSelf
        // NOT IMPLEMENTED
    }
}
