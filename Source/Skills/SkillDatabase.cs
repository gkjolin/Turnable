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
            
            AddSkillsToSkillSet(returnValue, skillNames);

            return returnValue;
        }

        protected override string GetKeyForItem(Skill skill)
        {
            return skill.Name;
        }

        public void AddSkillsToSkillSet(SkillSet skillSet, string[] skillNames)
        {
            foreach (string skillName in skillNames)
            {
                skillSet.Add(this[skillName]);
            }
        }
    }
}
