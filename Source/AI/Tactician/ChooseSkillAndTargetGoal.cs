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
    public class ChooseSkillAndTargetGoal : CompositeGoal
    {
        public IBoard Board { get; private set; }

        public ChooseSkillAndTargetGoal(Entity character, IBoard board)
        {
            Owner = character;
            Board = board;
        }

        public override void Activate()
        {
            base.Activate();

            Subgoals.Add(new UseSkillGoal(Owner, Board, Owner.GetComponent<SkillSet>()[0], Board.CharacterManager.Player.GetComponent<Position>()));
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
