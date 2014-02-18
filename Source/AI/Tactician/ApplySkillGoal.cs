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
    public class ApplySkillGoal : AtomicGoal
    {
        public ISkill Skill { get; private set; }
        public Entity Target { get; private set; }

        public ApplySkillGoal(ISkill skill, Entity target)
        {
            Skill = skill;
            Target = target;
        }

        public override void Activate()
        {
            base.Activate();
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
