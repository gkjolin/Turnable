using System;
using NUnit.Framework;
using Entropy;
using Tests.Factories;
using Turnable.Api;
using Turnable.Locations;
using Turnable.Components;
using Entropy.Core;

namespace Tests.Components
{
    [TestFixture]
    public class TeamMembershipTests
    {
        [Test]
        public void TeamMembership_IsAnEntropyComponent()
        {
            TeamMembership teamMembership = new TeamMembership();

            Assert.That(teamMembership, Is.InstanceOf<IComponent>());
        }

        [Test]
        public void DefaultConstructor_InitializesTeamNameToNull()
        {
            TeamMembership teamMembership = new TeamMembership();

            Assert.That(teamMembership.TeamName, Is.Null);
        }

        [Test]
        public void Constructor_InitializesTeamName()
        {
            TeamMembership teamMembership = new TeamMembership("Team Name");

            Assert.That(teamMembership.TeamName, Is.EqualTo("Team Name"));
        }
    }
}
