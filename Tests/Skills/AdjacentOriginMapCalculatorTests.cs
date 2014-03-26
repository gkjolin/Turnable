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
    public class AdjacentOriginMapCalculatorTests
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
        private AdjacentOriginMapCalculator _adjacentOriginMapCalculator;
        private ISkill _skill;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel();
            _skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            _adjacentOriginMapCalculator = new AdjacentOriginMapCalculator();
        }

        // An origin map is the Set of ALL Positions where a skill can be used from in order to target a certain position
        // TODO: Only testing the number of positions returned, check the actual positions themselves as well

        // Allowing diagonal movement
        [TestMethod]
        public void AdjacentOriginMapCalculator_ForATargetWithNoUnwalkablePositionsAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(5, 1), new Position(2, 13), 1, true);

            Assert.AreEqual(8, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_ForATargetWithObstaclesAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(12, 1), 1, true);

            Assert.AreEqual(3, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_ForATargetWithCharactersAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(2, 11), new Position(6, 13), 1, true);

            Assert.AreEqual(5, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_ForATargetWithCharactersAndTheSkillUserAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(6, 13), 1, true);

            Assert.AreEqual(6, skillOriginPositions.Count);
        }

        // Disallowing diagonal movement
        [TestMethod]
        public void AdjacentOriginMapCalculator_WithoutDiagonalMovementForATargetWithNoUnwalkablePositionsAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(5, 1), new Position(2, 13), 1);

            Assert.AreEqual(4, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_WithoutDiagonalMovementForATargetWithObstaclesAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(5, 14), new Position(12, 1), 1);

            Assert.AreEqual(2, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_WithoutDiagonalMovementForATargetWithCharactersAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(2, 11), new Position(6, 13), 1);

            Assert.AreEqual(3, skillOriginPositions.Count);
        }

        [TestMethod]
        public void AdjacentOriginMapCalculator_WithoutDiagonalMovementForATargetWithCharactersAndTheSkillUserAdjacentToIt_CalculatesOriginMapCorrectly()
        {
            HashSet<Position> skillOriginPositions = _adjacentOriginMapCalculator.Calculate(_level, new Position(6, 1), new Position(6, 2), 1);

            Assert.AreEqual(4, skillOriginPositions.Count);
        }
    }
}
