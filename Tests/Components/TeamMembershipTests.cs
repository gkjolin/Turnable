using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Entropy;
using Tests.Factories;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Components;
using Entropy.Core;

namespace Tests.Components
{
    [TestClass]
    public class TeamMembershipTests
    {
        [TestMethod]
        public void TeamMembership_IsAnEntropyComponent()
        {
            TeamMembership teamMembership = new TeamMembership();

            Assert.IsInstanceOfType(teamMembership, typeof(IComponent));
        }

        [TestMethod]
        public void DefaultConstructor_InitializesTeamNameToNull()
        {
            TeamMembership teamMembership = new TeamMembership();

            Assert.IsNull(teamMembership.TeamName);
        }

        [TestMethod]
        public void Constructor_InitializesTeamName()
        {
            TeamMembership teamMembership = new TeamMembership("Team Name");

            Assert.AreEqual("Team Name", teamMembership.TeamName);
        }
    }
}
