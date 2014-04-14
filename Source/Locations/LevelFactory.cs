using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Randomization;

namespace TurnItUp.Locations
{
    public class LevelFactory : ILevelFactory
    {
        public void Initialize(ILevel level, LevelInitializationParams initializationParams)
        {
            if (initializationParams.TmxPath != null)
            {
                level.SetUpMap(initializationParams.TmxPath);
                level.SetUpCharacters();
            }

            level.SetUpPathfinder(initializationParams.AllowDiagonalMovement);
        }

        public void Randomize(ILevel level)
        {
            level.Randomize();
            level.SetUpCharacters();
        }
    }
}
