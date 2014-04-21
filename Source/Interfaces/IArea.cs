using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;
using TurnItUp.Randomization;

namespace TurnItUp.Interfaces
{
    public interface IArea
    {
        List<ILevel> Levels { get; set; }
        List<Connection> Connections { get; set; }
        ILevel CurrentLevel { get; set; }
        IWorld World { get; set; }
        ILevelFactory LevelFactory { get; set; }

        void Initialize(IWorld world, LevelInitializationParams initializationParams);
        void Initialize(IWorld world, LevelInitializationParams initializationParams, LevelRandomizationParams randomizationParams);
        void Enter(Connection connection, LevelInitializationParams initializationParams);
        void Enter(Connection connection, LevelInitializationParams initializationParams, LevelRandomizationParams randomizationParams);
    }
}
