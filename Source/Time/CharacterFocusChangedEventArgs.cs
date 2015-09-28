using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entropy.Core;

namespace Tests.Time
{
    public class CharacterFocusChangedEventArgs : EventArgs
    {
        public Entity PreviousFocusedCharacter;
        public Entity CurrentFocusedCharacter;

        public CharacterFocusChangedEventArgs(Entity previousFocusedCharacter, Entity currentFocusedCharacter)
        {
            PreviousFocusedCharacter = previousFocusedCharacter;
            CurrentFocusedCharacter = currentFocusedCharacter;
        }
    }
}