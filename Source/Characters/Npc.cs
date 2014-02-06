using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Characters
{
    public class Npc : IEntityTemplate
    {
        public Entity Build(IWorld world)
        {
            Entity returnValue = world.CreateEntity();

            returnValue.AddComponent(new OnBoard());
            returnValue.AddComponent(new Position());
            
            return returnValue;
        }
    }
}
