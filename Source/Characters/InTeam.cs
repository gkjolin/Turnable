using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Characters
{
    public class InTeam : IComponent
    {
        public Entity Owner { get; set; }
        public string Name { get; private set; }

        public InTeam(string name)
        {
            Name = name;
        }
    }
}
