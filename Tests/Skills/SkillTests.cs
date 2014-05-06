using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;
using System.Collections.Generic;
using System.Tuples;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Components;
using Moq;
using TurnItUp.Interfaces;
using Entropy;

namespace Tests.Skills
{
    [TestClass]
    public class SkillTests
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

        private ILevel _level;
        private Entity _skillUser;
        private Entity _target;
        private Skill _skill;
        private bool _eventTriggeredFlag;
        private SkillAppliedEventArgs _eventArgs;

        [TestInitialize]
        public void Initialize()
        {
            World world = new World();
            _level = LocationsFactory.BuildLevel();
            _skillUser = world.CreateEntity();
            _target = world.CreateEntity();
            _eventTriggeredFlag = false;
            _skill = new Skill("Melee Attack");
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
            Assert.AreEqual(0, skill.Points);
        }

        [TestMethod]
        public void Skill_Construction_CreatesAnOriginMapCalculatorOfTheCorrectType()
        {
            Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 5);

            Assert.IsNotNull(skill.OriginMapCalculator);
            Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(DirectLineOriginMapCalculator));

            skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);

            Assert.IsNotNull(skill.OriginMapCalculator);
            Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(AdjacentOriginMapCalculator));
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

            TargetMap targetMap = skill.CalculateTargetMap(_level, new Position(6, 14));

            Assert.AreEqual(1, targetMap.Count);
            Assert.IsTrue(targetMap.ContainsKey(new Tuple<int,int>(7, 1)));
            HashSet<Position> originMap = targetMap[new Tuple<int, int>(7, 1)];
            Assert.AreEqual(originMap.Count, skill.OriginMapCalculator.Calculate(_level, new Position(6, 14), new Position(7, 1), skill.Range).Count);
        }

        [TestMethod]
        public void Skill_Applying_AppliesAllEffectsToTheTarget()
        {
            Mock<IEffect> effect1 = new Mock<IEffect>();
            Mock<IEffect> effect2 = new Mock<IEffect>();
            Mock<IEffect> effect3 = new Mock<IEffect>();

            _skill.Effects.Add(effect1.Object);
            _skill.Effects.Add(effect2.Object);
            _skill.Effects.Add(effect3.Object);

            _skill.Apply(_skillUser, _target);

            effect1.Verify(e => e.Apply(_skillUser, _target));
            effect2.Verify(e => e.Apply(_skillUser, _target));
            effect3.Verify(e => e.Apply(_skillUser, _target));
        }

        [TestMethod]
        public void Skill_Applying_RaisesAnAppliedEvent()
        {
            _skill.Applied += SetEventTriggeredFlag;
            _skill.Apply(_skillUser, _target);

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(_skillUser, _eventArgs.Entity);
            Assert.AreEqual(_skill, _eventArgs.Skill);
            Assert.AreEqual(_target, _eventArgs.Target);
        }

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = (SkillAppliedEventArgs)e;
        }
    }
}
