using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;

namespace TurnItUp.Components
{
    public class Model : IComponent
    {
        public Entity Owner { get; set; }
        public string Name { get; set; }

        public Model(string name)
        {
            Name = name;
        }

    }
}