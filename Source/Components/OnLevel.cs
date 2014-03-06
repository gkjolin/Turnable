using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Components
{
    public class OnLevel : IComponent
    {
        public Entity Owner { get; set; }
        public ILevel Level { get; set; }

        public OnLevel() : this(null)
        {
        }

        public OnLevel(ILevel level)
        {
            Level = level;
        }
    }
}