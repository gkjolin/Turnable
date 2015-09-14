using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Api
{
    public interface IGameLoop
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods

        // Properties
        ICharacterManager CharacterManager { get; set; }

        // Events

        // -----------------
        // Private interface
        // -----------------

        // Methods

        // Properties

        // Events
    }
}
