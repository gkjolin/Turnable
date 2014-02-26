using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Skills
{
    // TODO: Test this class
    public class SkillAppliedEventArgs : EntityEventArgs
    {
        public ISkill Skill { get; private set; }
        public Entity Target { get; private set; }

        public SkillAppliedEventArgs(Entity character, ISkill skill, Entity target)
            : base(character)
        {
            Skill = skill;
            Target = target;
        }
    }
}
