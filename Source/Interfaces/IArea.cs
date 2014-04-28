using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
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

        void SetUp(IWorld world, LevelSetUpParams setUpParams);
        void SetUp(IWorld world, LevelSetUpParams setUpParams, LevelRandomizationParams randomizationParams);
        void Enter(Connection connection);
        void Enter(Connection connection, LevelSetUpParams setUpParams);
        void Enter(Connection connection, LevelSetUpParams setUpParams, LevelRandomizationParams randomizationParams);

        Connection FindConnection(Position position);

        // Events
        event EventHandler<EventArgs> AfterSetUp;
        event EventHandler<EventArgs> AfterEnteringLevel;
    }
}
