using Entropy;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;

namespace TurnItUp.Skills
{
    public class SkillSet : KeyedCollection<string, Skill>, IComponent
    {
        public Entity Owner { get; set; }

        public SkillSet()
        {
        }

        protected override string GetKeyForItem(Skill skill)
        {
            return skill.Name;
        }
    }
}