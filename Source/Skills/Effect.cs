using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Skills
{
    public abstract class Effect
    {
        public abstract void Apply(Entity skillUser, Entity target);
    }
}
