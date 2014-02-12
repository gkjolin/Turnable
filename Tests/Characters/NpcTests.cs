using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;
using TurnItUp.AI.Brains;
using TurnItUp.Skills;

namespace Tests.Characters
{
    [TestClass]
    public class NpcTests
    {
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
        }

        [TestMethod]
        public void Npc_CreatingFromATemplate_IsCreatedWithTheCorrectComponents()
        {
            Entity npc = _world.CreateEntityFromTemplate<Npc>();

            Assert.AreEqual(5, npc.Components.Count);
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Brain)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(OnBoard)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(Position)));
            Assert.IsTrue(npc.Components.ContainsKey(typeof(InTeam)));
            Assert.AreEqual("NPCs", npc.GetComponent<InTeam>().Name);

            // Is there a basic melee attack for the NPC?
            SkillSet skillSet = npc.GetComponent<SkillSet>();

            Assert.IsNotNull(skillSet["Melee Attack"]);
            Skill skill = skillSet["Melee Attack"];
            Assert.AreEqual("Melee Attack", skill.Name);
            Assert.AreEqual(TargetType.InAnotherTeam, skill.TargetType);
            Assert.AreEqual(RangeType.Adjacent, skill.RangeType);
            Assert.AreEqual(1, skill.Range);
        }
    }
}
