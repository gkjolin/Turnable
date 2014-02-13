using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Pathfinding;
using TurnItUp.Skills;

namespace TurnItUp.AI.Tactician
{
    public class UseSkillGoal : CompositeGoal
    {
        public ISkill Skill { get; private set; }
        public Position Target { get; private set; }

        public UseSkillGoal(Entity character, ISkill skill, Position target)
        {
            Owner = character;
            Skill = skill;
            Target = target;
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
