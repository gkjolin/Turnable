using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.Components;
using TurnItUp.Pathfinding;
using TurnItUp.Skills;

namespace TurnItUp.AI.Tactician
{
    public class UseSkillGoal : CompositeGoal
    {
        public Skill Skill { get; private set; }

        public UseSkillGoal(Entity character, Skill skill)
        {
            Owner = character;
            Skill = skill;
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
