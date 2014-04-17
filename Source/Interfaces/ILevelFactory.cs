using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Randomization;
using TurnItUp.Tmx;

namespace TurnItUp.Interfaces
{
    public interface ILevelFactory
    {
        ILevelRandomizer LevelRandomizer { get; set; }

        ILevel BuildLevel(IWorld world);
        ILevel BuildLevel(IWorld world, LevelInitializationParams initializationParams);
        ILevel BuildLevel(IWorld world, LevelInitializationParams initializationParams, LevelRandomizationParams randomizationParams);
        void Initialize(ILevel level, LevelInitializationParams initializationParams);
        void Randomize(ILevel level, LevelRandomizationParams randomizationParams);
    }
}
