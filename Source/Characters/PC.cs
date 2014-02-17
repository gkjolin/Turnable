using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Skills;
using TurnItUp.Stats;

namespace TurnItUp.Characters
{
    public class PC : IEntityTemplate
    {
        public Entity Build(IWorld world)
        {
            Entity returnValue = world.CreateEntity();

            returnValue.AddComponent(new OnBoard());
            returnValue.AddComponent(new Position());
            returnValue.AddComponent(new InTeam("PCs"));
            returnValue.AddComponent(new SkillSet());

            Skill skill= new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            returnValue.GetComponent<SkillSet>().Add(skill);

            returnValue.AddComponent(new StatManager());
            returnValue.GetComponent<StatManager>().CreateStat("Health", 100);

            return returnValue;
        }
    }
}
