using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Tests.Factories;
using Moq;
using Entropy;
using Turnable.Skills;
using Turnable.Api;

namespace Tests.Skills
{
    [TestClass]
    public class SkillTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void Constructor_InitializesAllProperties()
        {
            Skill skill = new Skill("Melee Attack");

            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.Adjacent, skill.RangeType);
            Assert.IsNotNull(skill.Effects);
            Assert.IsInstanceOfType(skill.Effects, typeof(IEnumerable<IEffect>));
            Assert.AreEqual(0, skill.Range);
            Assert.AreEqual(0, skill.Cost);
        }

        [TestMethod]
        public void Constructor_GivenInitialValues_InitializesAllProperties()
        {
            Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 1, 2);

            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.DirectLine, skill.RangeType);
            Assert.AreEqual(1, skill.Range);
            Assert.AreEqual(2, skill.Cost);
        }


        //[TestMethod]
        //public void Skill_Construction_CreatesAnOriginMapCalculatorOfTheCorrectType()
        //{
        //    Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 5);

        //    Assert.IsNotNull(skill.OriginMapCalculator);
        //    Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(DirectLineOriginMapCalculator));

        //    skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);

        //    Assert.IsNotNull(skill.OriginMapCalculator);
        //    Assert.IsInstanceOfType(skill.OriginMapCalculator, typeof(AdjacentOriginMapCalculator));
        //}


        // Testing the generation of a skill's Target Map
        // A Target Map is the set of nodes from which an enemy can use the skill on the player
        //[TestMethod]
        //public void Skill_WithARangeOfAdjacentNodes_GeneratesTheCorrectTargetMap()
        //{
        //    Skill skill = new Skill("Melee Attack") { RangeType = RangeType.Adjacent, TargetType = TargetType.InAnotherTeam };

        //    TargetMap targetMap = skill.CalculateTargetMap(_level, new Position(6, 14));

        //    Assert.AreEqual(1, targetMap.Count);
        //    Assert.IsTrue(targetMap.ContainsKey(new Tuple<int, int>(7, 1)));
        //    HashSet<Position> originMap = targetMap[new Tuple<int, int>(7, 1)];
        //    Assert.AreEqual(originMap.Count, skill.OriginMapCalculator.Calculate(_level, new Position(6, 14), new Position(7, 1), skill.Range).Count);
        //}

        //[TestMethod]
        //public void Skill_Applying_AppliesAllEffectsToTheTarget()
        //{
        //    Mock<IEffect> effect1 = new Mock<IEffect>();
        //    Mock<IEffect> effect2 = new Mock<IEffect>();
        //    Mock<IEffect> effect3 = new Mock<IEffect>();

        //    _skill.Effects.Add(effect1.Object);
        //    _skill.Effects.Add(effect2.Object);
        //    _skill.Effects.Add(effect3.Object);

        //    _skill.Apply(_skillUser, _target);

        //    effect1.Verify(e => e.Apply(_skillUser, _target));
        //    effect2.Verify(e => e.Apply(_skillUser, _target));
        //    effect3.Verify(e => e.Apply(_skillUser, _target));
        //}

        //[TestMethod]
        //public void Skill_Applying_RaisesAnAppliedEvent()
        //{
        //    _skill.Applied += SetEventTriggeredFlag;
        //    _skill.Apply(_skillUser, _target);

        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.AreEqual(_skillUser, _eventArgs.Entity);
        //    Assert.AreEqual(_skill, _eventArgs.Skill);
        //    Assert.AreEqual(_target, _eventArgs.Target);
        //}

        //private void SetEventTriggeredFlag(object sender, EventArgs e)
        //{
        //    _eventTriggeredFlag = true;
        //    _eventArgs = (SkillAppliedEventArgs)e;
        //}
    }
}
