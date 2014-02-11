using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;

namespace Tests.Characters
{
    [TestClass]
    public class PCTests
    {
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
        }

        [TestMethod]
        public void PC_CreatingFromATemplate_IsCreatedWithTheCorrectComponents()
        {
            Entity pc = _world.CreateEntityFromTemplate<PC>();

            Assert.AreEqual(3, pc.Components.Count);
            Assert.IsTrue(pc.Components.ContainsKey(typeof(OnBoard)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(Position)));
            Assert.IsTrue(pc.Components.ContainsKey(typeof(InTeam)));
            Assert.AreEqual("PCs", pc.GetComponent<InTeam>().Name);
        }
    }
}
