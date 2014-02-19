using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;

namespace TurnItUp.Skills
{
    public class SkillDatabase : KeyedCollection<string, Skill>
    {
        public SkillSet CreateSkillSet(string[] skillNames)
        {
            SkillSet returnValue = new SkillSet();

            foreach (string skillName in skillNames)
            {
                returnValue.Add(this[skillName]);
            }

            return returnValue;
        }

        protected override string GetKeyForItem(Skill skill)
        {
            return skill.Name;
        }
    }
}
