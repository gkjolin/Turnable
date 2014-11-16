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
            // TODO: Unit test the next line
            if (!TransitionPointManager.DoesLevelMeetRequirements(level))
            {
                return;
            }

            foreach (Position transitionPoint in level.TransitionPointManager.Exits)
            {
                Connections.Add(new Connection(new Node(level, transitionPoint.X, transitionPoint.Y), null));
            }
        }

        public void SetUp(IWorld world, LevelSetUpParams setUpParams)
        {
            if (setUpParams == null)
            {
                throw new ArgumentException("<Area::SetUp> : setUpParams cannot be null.");
            } 

            World = world;
            CurrentLevel = LevelFactory.BuildLevel(World, setUpParams);
            Levels.Add(CurrentLevel);
            SetupConnections(CurrentLevel);

            OnAfterSetUp(new EventArgs());
        }

        public void SetUp(IWorld world, LevelSetUpParams setUpParams, LevelRandomizationParams randomizationParams)
        {
            throw new NotImplementedException();
        }

        public void Enter(Connection connection)
        {
            // Entering from the StartNode side
            if (CurrentLevel == connection.StartNode.Level)
            {
                CurrentLevel = connection.EndNode.Level;
                OnAfterEnteringLevel(new EventArgs());
                return;
            }

            CurrentLevel = connection.StartNode.Level;
            OnAfterEnteringLevel(new EventArgs());
        }

        public void Enter(Connection connection, LevelSetUpParams setUpParams)
        {
            if (setUpParams == null)
            {
                throw new ArgumentException("<Area::Enter> : setUpParams cannot be null.");
            }

            if (connection.EndNode != null)
            {
                throw new InvalidOperationException("<Area::Enter> : a level has already been set up at the EndNode of this connection and cannot be set up again. Use <Area::Enter(Connection connection)> instead.");
            }

            CurrentLevel = LevelFactory.BuildLevel(World, setUpParams);
            Levels.Add(CurrentLevel);
            // Set up the endNode for the connection (this is set as null which indicates that the Level is still not built)
            connection.EndNode = new Node(CurrentLevel, CurrentLevel.TransitionPointManager.Entrance.X, CurrentLevel.TransitionPointManager.Entrance.Y);
            SetupConnections(CurrentLevel);

            OnAfterEnteringLevel(new EventArgs());
        }

        public void Enter(Connection connection, LevelSetUpParams setUpParams, LevelRandomizationParams randomizationParams)
        {
            throw new NotImplementedException();
        }

        public virtual event EventHandler<EventArgs> AfterSetUp;
        public virtual event EventHandler<EventArgs> AfterEnteringLevel;

        protected virtual void OnAfterSetUp(EventArgs e)
        {
            if (AfterSetUp != null)
            {
                AfterSetUp(this, e);
            }
        }

        protected virtual void OnAfterEnteringLevel(EventArgs e)
        {
            if (AfterEnteringLevel != null)
            {
                AfterEnteringLevel(this, e);
            }
        }

        public Connection FindConnection(Position position)
        {
            return Connections.FirstOrDefault<Connection>(c => c.StartNode.Position == position || (c.EndNode != null && c.EndNode.Position == position));
        }
    }
}
