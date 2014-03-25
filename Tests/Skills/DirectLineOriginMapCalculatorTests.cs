using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;
using System.Collections.Generic;
using System.Tuples;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Components;
using TurnItUp.Interfaces;

namespace Tests.Skills
{
    [TestClass]
    public class DirectLineOriginMapCalculatorTests
    {
        // The sample level:
        // XXXXXXXXXXXXXXXX
        // X....EEE.......X
        // X..........X...X
        // X.......E......X
        // X.E.X..........X
        // X.....E....E...X
        // X........X.....X
        // X..........XXXXX
        // X..........X...X
        // X..........X...X
        // X......X.......X
        // X.X........X...X
        // X..........X...X
        // X..........X...X
        // X......P...X...X
        // XXXXXXXXXXXXXXXX
        // X - Obstacles, P - Player, E - Enemies
        private Level _level;
        private DirectLineOriginMapCalculator _directLineOriginMapCalculator;
        private ISkill _skill;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _skill = new Skill("Ranged Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 2);
            _directLineOriginMapCalculator = new DirectLineOriginMapCalculator();
        }

        // An origin map is the Set of ALL Positions where a skill can be used from in order to target a certain position
        // TODO: Only testing the number of positions returned, check the actual positions themselves as well

        // Allowing diagonal movement + must have line of sight
        [TestMethod]
        public void DirectLineOriginMapCalculator_ForATargetWithNoUnwalkablePositionsOrthogonalToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(5, 1), new Position(3, 8), 2, true);

            Assert.AreEqual(16, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_ForATargetWithObstaclesAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(12, 1), 2, true);

            Assert.AreEqual(6, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_ForATargetWithCharactersAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(2, 1), new Position(6, 13), 2, true);

            Assert.AreEqual(9, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_ForATargetWithCharactersAndTheSkillUserAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(6, 13), 2, true);

            Assert.AreEqual(10, skillOriginPositions.Count);
        }

        // Disallowing diagonal movement
        [TestMethod]
        public void DirectLineOriginMapCalculator_WithoutDiagonalMovementForATargetWithNoUnwalkablePositionsAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(5, 1), new Position(3, 12), 2);

            Assert.AreEqual(8, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_WithoutDiagonalMovementForATargetWithObstaclesAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(12, 1), 2);

            Assert.AreEqual(4, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_WithoutDiagonalMovementForATargetWithCharactersAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(2, 11), new Position(6, 13), 2);

            Assert.AreEqual(6, skillOriginPositions.Count);
        }

        [TestMethod]
        public void DirectLineOriginMapCalculator_WithoutDiagonalMovementForATargetWithCharactersAndTheSkillUserAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _directLineOriginMapCalculator.Calculate(_level, new Position(6, 1), new Position(6, 2), 2);

            Assert.AreEqual(7, skillOriginPositions.Count);
        }
    }
}
