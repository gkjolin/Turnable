using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entropy.Core;
using Turnable.Api;
using Tests.Time;

namespace Turnable.Time
{
    public class TurnManager : ITurnManager
    {
        public ICharacterManager CharacterManager { get; set; }
        public List<Entity> Queue { get; }

        public Entity FocusedCharacter {
            get
            {
                return Queue[_focusedIndex];
            }
        }

        private int _focusedIndex = 0;

        public TurnManager(ICharacterManager characterManager)
        {
            CharacterManager = characterManager;

            Queue = new List<Entity>();
            for (int index = 0; index < CharacterManager.Pcs.Count; index++)
            {
                Queue.Add(CharacterManager.Pcs[index]);
            }
        }

        public event EventHandler<CharacterFocusChangedEventArgs> CharacterFocusChanged;

        public void SwitchFocus()
        {
            Entity previousFocusedCharacter = FocusedCharacter;

            _focusedIndex++;

            if (_focusedIndex >= Queue.Count)
            {
                _focusedIndex = 0;
            }

            // Raise the event only if the SwitchFocus call resulted in a new character having the focus.
            if (previousFocusedCharacter != FocusedCharacter)
            {
                CharacterFocusChangedEventArgs characterFocusedEventArgs = new CharacterFocusChangedEventArgs(previousFocusedCharacter, FocusedCharacter);
                OnCharacterFocusChanged(this, characterFocusedEventArgs);
            }
        }

        public void SwitchFocus(Entity newFocusedCharacter)
        {
            throw new NotImplementedException();
        }

        protected virtual void OnCharacterFocusChanged(object sender, CharacterFocusChangedEventArgs e)
        {
            if (CharacterFocusChanged != null)
            {
                CharacterFocusChanged(this, e);
            }
        }
    }
}
