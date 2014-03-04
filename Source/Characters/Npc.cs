using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Brains;
using TurnItUp.Components;
using TurnItUp.Skills;
using TurnItUp.Stats;

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
            returnValue.AddComponent(new StatManager());
            returnValue.GetComponent<StatManager>().CreateStat("Health", 100, 0, 100, true);
            returnValue.AddComponent(new SkillSet());

            return returnValue;
        }
    }
}
