using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using Tests.Factories;
using TurnItUp.Characters;
using System.Collections.Generic;
using TurnItUp.Components;
using System.Tuples;
using Entropy;
using Moq;
using TurnItUp.Interfaces;

namespace Tests.Characters
{
    [TestClass]
    public class InTeamTests
    {
        [TestMethod]
        public void Team_Construction_IsSuccessful()
        {
            InTeam inTeam = new InTeam("PCs");

            Assert.AreEqual("PCs", inTeam.Name);
        }

        [TestMethod]
        public void Team_IsAnEntropyComponent()
        {
            InTeam inTeam = new InTeam("PCs");

            Assert.IsInstanceOfType(inTeam, typeof(IComponent));
        }
    }
}
