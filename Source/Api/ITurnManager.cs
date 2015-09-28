using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tests.Time;

namespace Turnable.Api
{
    public interface ITurnManager
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        void SwitchFocus();
        void SwitchFocus(Entity newFocusedCharacter);

        // Properties
        ICharacterManager CharacterManager { get; set; }
        List<Entity> Queue { get; }
        Entity FocusedCharacter { get; }

        // Events
        event EventHandler<CharacterFocusChangedEventArgs> CharacterFocusChanged;

        // -----------------
        // Private interface
        // -----------------

        // Methods

        // Properties

        // Events
    }
}
