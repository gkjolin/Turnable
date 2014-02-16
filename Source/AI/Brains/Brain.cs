using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.AI.Goals;
using TurnItUp.AI.Tactician;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Skills;

namespace TurnItUp.AI.Brains
{
    public class Brain : CompositeGoal
    {
        public Goal CurrentGoal { get; private set; }
        public IBoard Board { get; private set; }

        public Brain() : this(null, null)
        {
        }

        public Brain(Entity character, IBoard board)
        {
            Owner = character;
            Board = board;
        }

        public void Think()
        {
            Skill skill = new Skill("Melee Attack", RangeType.Adjacent, TargetType.InAnotherTeam, 1);
            CurrentGoal = new UseSkillGoal(Owner, Board, skill, Board.CharacterManager.Player.GetComponent<Position>());
        }
    }
}
