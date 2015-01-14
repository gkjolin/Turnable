using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Characters
{
    public class CharacterManager : ICharacterManager
    {
        public ILevel Level { get; set; }
        public IList<Entity> Characters { get; set; }

        public CharacterManager(ILevel level)
        {
            Level = level;
            Characters = new List<Entity>();
        }
    }
}
