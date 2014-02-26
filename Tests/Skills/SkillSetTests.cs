using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Skills;
using System.Collections.Generic;
using System.Tuples;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Components;
using Entropy;

namespace Tests.Skills
{
    [TestClass]
    public class SkillSetTests
    {
        private SkillSet _skillSet;
        private bool _eventTriggeredFlag;
        private SkillAppliedEventArgs _eventArgs;
        private Entity _entity;
        private Entity _target;

        [TestInitialize]
        public void Initialize()
        {
            World world = new World();
            _entity = world.CreateEntity();
            _target = world.CreateEntity();
            _skillSet = new SkillSet();
            _eventTriggeredFlag = false;
        }

        [TestMethod]
        public void SkillSet_Construction_IsSuccessful()
        {
            SkillSet skillSet = new SkillSet();
        }

        [TestMethod]
        public void SkillSet_IsAnEntropyComponent()
        {
            SkillSet skillSet = new SkillSet();

            Assert.IsInstanceOfType(skillSet, typeof(IComponent));
        }

        [TestMethod]
        public void SkillSet_AddingASkill_IsSuccessful()
        {
            _skillSet.Add(new Skill("Melee Attack"));

            Assert.AreEqual(1, _skillSet.Count);
            Assert.IsNotNull(_skillSet["Melee Attack"]);
        }

        [TestMethod]
        public void StatManager_WhenAStatIsChanged_RaisesAnEvent()
        {
            _entity.AddComponent(_skillSet);
            _skillSet.Add(new Skill("Melee Attack"));

            _skillSet.SkillApplied += SetEventTriggeredFlag;
            _skillSet[0].Apply(_entity, _target);

            Assert.IsTrue(_eventTriggeredFlag);
            Assert.AreEqual(_entity, _eventArgs.Entity);
            Assert.AreEqual(_skillSet[0], _eventArgs.Skill);
            Assert.AreEqual(_target, _eventArgs.Target);
        }

        private void SetEventTriggeredFlag(object sender, EventArgs e)
        {
            _eventTriggeredFlag = true;
            _eventArgs = (SkillAppliedEventArgs)e;
        }

    }
}
