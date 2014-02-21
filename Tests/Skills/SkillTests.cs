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
        // The sample board:
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

        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _board = LocationsFactory.BuildBoard();
        }

        [TestMethod]
        public void Skill_Construction_IsSuccessful()
        {
            Skill skill = new Skill("Melee Attack");

            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.Adjacent, skill.RangeType);
            Assert.AreEqual(1, skill.Range);
            Assert.IsNotNull(skill.Effects);
        }

        [TestMethod]
        public void Skill_Construction_CreatesAnOriginMapCalculatorOfTheCorrectType()
        {
            Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 5);

            Assert.IsNotNull(skill.OriginMapCalculator);
            Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(DirectLineOriginMapCalculator));
            Assert.AreEqual(5, skill.OriginMapCalculator.SkillRange);

            skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);

            Assert.IsNotNull(skill.OriginMapCalculator);
            Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(AdjacentOriginMapCalculator));
            Assert.AreEqual(1, skill.OriginMapCalculator.SkillRange);
        }

        [TestMethod]
        public void Skill_ConstructionWithAllValues_IsSuccessful()
        {
            Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 5);

            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.DirectLine, skill.RangeType);
            Assert.AreEqual(5, skill.Range);
        }

        // Testing the generation of a skill's Target Map
        // A Target Map is the set of nodes from which an enemy can use the skill on the player

        [TestMethod]
        public void Skill_WithARangeOfAdjacentNodes_GeneratesTheCorrectTargetMap()
        {
            Skill skill = new Skill("Melee Attack") { RangeType = RangeType.Adjacent, TargetType = TargetType.InAnotherTeam };

            TargetMap targetMap = skill.CalculateTargetMap(_board, new Position(6, 1));

            Assert.AreEqual(1, targetMap.Count);
            Assert.IsTrue(targetMap.ContainsKey(new Tuple<int,int>(7, 14)));
            HashSet<Position> originMap = targetMap[new Tuple<int, int>(7, 14)];
            //Assert.AreEqual(originMap.Count, AdjacentOriginMapCalculator.CalculateOriginMap(_board, new Position(6, 1), new Position(7, 14)).Count);
        }
    }
}
