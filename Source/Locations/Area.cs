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
    public class Area : IArea
    {
        public List<ILevel> Levels { get; set; }
        public List<Connection> Connections { get; set; }
        public ILevel CurrentLevel { get; set; }
        public IWorld World { get; set; }
        public ILevelFactory LevelFactory { get; set; }

        public Area()
        {
            Levels = new List<ILevel>();
            Connections = new List<Connection>();
            CurrentLevel = null;
            LevelFactory = new LevelFactory();
        }

        private void SetupConnections(ILevel level)
        {
            foreach (Position transitionPoint in level.TransitionPointManager.Exits)
            {
                Connections.Add(new Connection(new Node(level, transitionPoint.X, transitionPoint.Y), null));
            }
        }

        public void Initialize(IWorld world, LevelInitializationParams initializationParams)
        {
            World = world;
            CurrentLevel = LevelFactory.BuildLevel(World, initializationParams);
            Levels.Add(CurrentLevel);
            SetupConnections(CurrentLevel);
        }

        public void Initialize(IWorld world, LevelInitializationParams initializationParams, LevelRandomizationParams randomizationParams)
        {
            throw new NotImplementedException();
        }

        public void Enter(Connection connection, LevelInitializationParams initializationParams)
        {
            CurrentLevel = LevelFactory.BuildLevel(World, initializationParams);
            Levels.Add(CurrentLevel);
            // Set up the endNode for the connection (this is set as null which indicates that the Level is still not built)
            connection.EndNode = new Node(CurrentLevel, CurrentLevel.TransitionPointManager.Entrance.X, CurrentLevel.TransitionPointManager.Entrance.Y);
            SetupConnections(CurrentLevel);
        }

        public void Enter(Connection connection, LevelInitializationParams initializationParams, LevelRandomizationParams randomizationParams)
        {
            throw new NotImplementedException();
        }
    }
}
