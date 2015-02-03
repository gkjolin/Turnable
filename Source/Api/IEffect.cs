using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Pathfinding;

namespace Turnable.Api
{
    public interface IEffect
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        void Apply(Entity source, Entity target);
    }
}
