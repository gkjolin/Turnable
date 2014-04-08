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
    public class SkillTree : IComponent
    {
        public Entity Owner { get; set; }
        public List<SkillTree> Subtrees { get; set; }

        public SkillTree()
        {
            Subtrees = new List<SkillTree>();
        }
    }
}