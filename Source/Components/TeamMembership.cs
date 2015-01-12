using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Locations;

namespace Turnable.Components
{
    public class TeamMembership : IComponent
    {
        public string TeamName { get; private set; }

        public TeamMembership()
        {
        }

        public TeamMembership(string teamName)
        {
            TeamName = teamName;
        }
    }
}
