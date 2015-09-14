using System;
using NUnit.Framework;
using System.Collections.Generic;
using Tests.Factories;
using Moq;
using Entropy;
using Turnable.Skills;
using Turnable.Api;

namespace Tests.Skills
{
    [TestFixture]
    public class SkillTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Skill skill = new Skill("Melee Attack");

            Assert.That(skill.Name, Is.EqualTo("Melee Attack"));
            Assert.That(skill.TargetType, Is.EqualTo(TargetType.InAnotherTeam));
            Assert.That(skill.RangeType, Is.EqualTo(RangeType.Adjacent));
            Assert.That(skill.Effects, Is.Not.Null);
            Assert.That(skill.Effects, Is.InstanceOf<IEnumerable<IEffect>>());
            Assert.That(skill.Range, Is.EqualTo(0));
            Assert.That(skill.Cost, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_GivenInitialValues_InitializesAllProperties()
        {
            Skill skill = new Skill("Melee Attack", RangeType.DirectLine, TargetType.InAnotherTeam, 1, 2);

            Assert.That(skill.Name, Is.EqualTo("Melee Attack"));
            Assert.That(skill.TargetType, Is.EqualTo(TargetType.InAnotherTeam));
            Assert.That(skill.RangeType, Is.EqualTo(RangeType.DirectLine));
            Assert.That(skill.Range, Is.EqualTo(1));
            Assert.That(skill.Cost, Is.EqualTo(2));
        }

        //[Test]
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
        //[Test]
        //public void Skill_WithARangeOfAdjacentNodes_GeneratesTheCorrectTargetMap()
        //{
        //    Skill skill = new Skill("Melee Attack") { RangeType = RangeType.Adjacent, TargetType = TargetType.InAnotherTeam };

        //    TargetMap targetMap = skill.CalculateTargetMap(_level, new Position(6, 14));

        //    Assert.That(1, targetMap.Count);
        //    Assert.IsTrue(targetMap.ContainsKey(new <int, int>(7, 1)));
        //    HashSet<Position> originMap = targetMap[new <int, int>(7, 1)];
        //    Assert.That(originMap.Count, skill.OriginMapCalculator.Calculate(_level, new Position(6, 14), new Position(7, 1), skill.Range).Count);
        //}

        //[Test]
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

        //[Test]
        //public void Skill_Applying_RaisesAnAppliedEvent()
        //{
        //    _skill.Applied += SetEventTriggeredFlag;
        //    _skill.Apply(_skillUser, _target);

        //    Assert.IsTrue(_eventTriggeredFlag);
        //    Assert.That(_skillUser, _eventArgs.Entity);
        //    Assert.That(_skill, _eventArgs.Skill);
        //    Assert.That(_target, _eventArgs.Target);
        //}

        //private void SetEventTriggeredFlag(object sender, EventArgs e)
        //{
        //    _eventTriggeredFlag = true;
        //    _eventArgs = (SkillAppliedEventArgs)e;
        //}
    }
}
