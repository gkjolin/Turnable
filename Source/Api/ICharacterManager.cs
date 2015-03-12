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

        // Methods
        Movement MoveCharacter(Entity character, Position destination);
        Movement MoveCharacter(Entity character, Direction direction);
        Movement MovePlayer(Direction direction);
        void SetUpPcs();
        void SetUpNpcs();

        // Properties
        ILevel Level { get; set; }
        IList<Entity> Characters { get; set; }
        Entity Player { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
