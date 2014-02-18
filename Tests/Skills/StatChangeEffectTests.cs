using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Entropy;
using System.Collections.Generic;
using TurnItUp.Skills;
using TurnItUp.Stats;

namespace Tests.Skills
{
    [TestClass]
    public class StatChangeEffectTests
    {
        private StatChangeEffect _statChangeEffect;
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            World world = new World();

            _statChangeEffect = new StatChangeEffect();
            _entity = world.CreateEntity();

            _entity.AddComponent(new StatManager());
            _entity.GetComponent<StatManager>().CreateStat("Mana", 50);
            _entity.GetComponent<StatManager>().CreateStat("Health", 100);
        }

        [TestMethod]
        public void StatChangeEffect_Construction_IsSuccessful()
        {
            StatChangeEffect statChangeEffect = new StatChangeEffect();

            Assert.IsNotNull(statChangeEffect.StatChanges);
        }

        [TestMethod]
        public void StatChangeSkillEffect_CanAddStatChanges()
        {
            _statChangeEffect.AddStatChange("Health", "-10");

            Assert.AreEqual(1, _statChangeEffect.StatChanges.Count);
            Assert.AreEqual("-10", _statChangeEffect.StatChanges["Health"]);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StatChangeSkillEffect_TryingToAddChangesToTheSameStat_IsUnsuccessful()
        {
            _statChangeEffect.AddStatChange("Health", "-10");
            _statChangeEffect.AddStatChange("Health", "-10");
        }

        [TestMethod]
        public void StatChangeSkillEffect_WhenApplyingValueReductions_IsSuccessful()
        {
            _statChangeEffect.AddStatChange("Mana", "-5");
            _statChangeEffect.AddStatChange("Health", "-50");

            _statChangeEffect.Apply(null, _entity);

            Assert.AreEqual(45, _entity.GetComponent<StatManager>().GetStat("Mana").Value);
            Assert.AreEqual(50, _entity.GetComponent<StatManager>().GetStat("Health").Value);
        }
    }
}
