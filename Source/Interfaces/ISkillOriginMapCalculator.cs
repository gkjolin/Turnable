using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Interfaces
{
    public interface ISkillOriginMapCalculator
    {
        ISkill Skill { get; set; }

        HashSet<Position> Calculate(IBoard board, Position skillUserPosition, Position targetPosition, bool allowDiagonalMovement = false);
    }
}
