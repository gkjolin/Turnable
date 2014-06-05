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
        public ILevelRandomizer LevelRandomizer { get; set; }

        public LevelFactory()
        {
            LevelRandomizer = new LevelRandomizer();
        }

        public void SetUp(ILevel level, LevelSetUpParams setUpParams)
        {
            if (setUpParams.TmxPath != null)
            {
                level.SetUpMap(setUpParams.TmxPath);
                level.SetUpTransitionPoints();

                if (setUpParams.IsPlayerPositionSet())
                {
                    level.SetUpCharacters(setUpParams.PlayerModel, setUpParams.PlayerX, setUpParams.PlayerY);
                }
                else
                {
                    level.SetUpCharacters(setUpParams.PlayerModel, level.TransitionPointManager.Entrance.X, level.TransitionPointManager.Entrance.Y);
                }
            }

            level.SetUpPathfinder(setUpParams.AllowDiagonalMovement);
        }

        public void Randomize(ILevel level, LevelRandomizationParams randomizationParams)
        {
            if (randomizationParams.LayerName == null || randomizationParams.TileCount == null)
            {
                throw new ArgumentException("<LevelFactory::Randomize> : the randomizationParams have not been initialized with any values.");
            }

            if (randomizationParams.TileMaximum == null)
            {
                LevelRandomizer.Randomize(level, randomizationParams.LayerName, randomizationParams.TileCount.Value);
            }
            else
            {
                LevelRandomizer.Randomize(level, randomizationParams.LayerName, randomizationParams.TileCount.Value, randomizationParams.TileMaximum.Value + 1);
            }

            level.SetUpNpcs();
        }

        public ILevel BuildLevel(IWorld world)
        {
            return new Level(world);
        }

        public ILevel BuildLevel(IWorld world, LevelSetUpParams setUpParams)
        {
            ILevel returnValue = BuildLevel(world);

            SetUp(returnValue, setUpParams);

            return returnValue;
        }

        public ILevel BuildLevel(IWorld world, LevelSetUpParams setUpParams, LevelRandomizationParams randomizationParams)
        {
            ILevel returnValue = BuildLevel(world, setUpParams);

            Randomize(returnValue, randomizationParams);

            return returnValue;
        }
    }
}
