using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Time
{
    public class GameLoop : IGameLoop
    {
        public ICharacterManager CharacterManager { get; set; }

        public GameLoop(ICharacterManager characterManager)
        {
            CharacterManager = characterManager;
        }
    }
}
