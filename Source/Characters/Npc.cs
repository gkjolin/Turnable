using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Brains;
using TurnItUp.Components;
using TurnItUp.Skills;

namespace TurnItUp.Characters
{
    public class Npc : IEntityTemplate
    {
        public Entity Build(IWorld world)
        {
            Entity returnValue = world.CreateEntity();

            returnValue.AddComponent(new Brain());
            returnValue.AddComponent(new OnBoard());
            returnValue.AddComponent(new Position());
            returnValue.AddComponent(new InTeam("NPCs"));
            returnValue.AddComponent(new SkillSet());

            Skill skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            returnValue.GetComponent<SkillSet>().Add(skill);

            return returnValue;
        }
    }
}
