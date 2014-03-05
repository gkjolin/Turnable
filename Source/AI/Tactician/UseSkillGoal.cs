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
    public class UseSkillGoal : CompositeGoal
    {
        public ISkill Skill { get; private set; }
        public Position Target { get; private set; }
        public IBoard Board { get; private set; }

        public UseSkillGoal(Entity character, IBoard board, ISkill skill, Position target)
        {
            Owner = character;
            Board = board;
            Skill = skill;
            Target = target;
        }

        public override void Activate()
        {
            base.Activate();

            TargetMap targetMap = Skill.CalculateTargetMap(Board, Owner.GetComponent<Position>());
            HashSet<Position> candidatePositions = targetMap[new System.Tuples.Tuple<int, int>(Target.X, Target.Y)];
            Position startingPosition = Owner.GetComponent<Position>();

            // If the skill user is already in a position to the skill, simply apply the skill
            if (candidatePositions.Contains(startingPosition))
            {
                Subgoals.Add(new ApplySkillGoal(Owner, Skill, Board.World.EntitiesWhere<Position>(p => p.X == Target.X && p.Y == Target.Y).Single()));
                return;
            }

            // If there are no candidate positions where the skill can be used, fail the goal
            if (candidatePositions.Count == 0)
            {
                Status = GoalStatus.Failed;
            }
            else
            {
                Node startingNode = new Node(Board, startingPosition.X, startingPosition.Y);
                Node endingNode = Board.PathFinder.GetClosestNode(Owner.GetComponent<Position>(), candidatePositions);

                List<Node> bestPath = Board.PathFinder.SeekPath(startingNode, endingNode);

                // Fail the goal if there is no path to the target
                if (bestPath == null)
                {
                    Status = GoalStatus.Failed;
                }
                else
                {
                    Subgoals.Add(new FollowPathGoal(Owner, bestPath));
                }
            }
        }

        public override void Process()
        {
            base.Process();
        }
    }
}
