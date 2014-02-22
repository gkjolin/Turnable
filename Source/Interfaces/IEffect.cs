using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Interfaces
{
    public interface IEffect
    {
        void Apply(Entity skillUser, Entity target);
    }
}
