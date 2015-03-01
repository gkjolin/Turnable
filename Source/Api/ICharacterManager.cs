using Entropy;
using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Api
{
    public interface ICharacterManager
    {
        // ----------------
        // Public interface
        // ----------------

        // Method
        void SetUpPlayer(int startingX, int startingY);

        // Properties
        ILevel Level { get; set; }
        IList<Entity> Characters { get; set; }
        Entity Player { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
