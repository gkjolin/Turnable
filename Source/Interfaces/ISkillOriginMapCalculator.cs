using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;

namespace TurnItUp.Interfaces
{
    public interface ISkillOriginMapCalculator
    {
        HashSet<Position> Calculate(ILevel level, Position skillUserPosition, Position targetPosition, int range, bool allowDiagonalMovement = false);
    }
}
