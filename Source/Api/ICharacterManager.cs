using Entropy;
using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Characters;
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
        void SetUpPcs();
        void SetUpNpcs();

        // Properties
        ILevel Level { get; set; }
        IList<Entity> Pcs { get; set; }
        IList<Entity> Npcs { get; set; }

        // Events
        event EventHandler<CharacterMovedEventArgs> CharacterMoved;

        // -----------------
        // Private interface
        // -----------------
    }
}
