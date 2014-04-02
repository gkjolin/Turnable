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
    public class Viewport : IViewport
    {
        public Level Level { get; set; }
        public Position MapOrigin { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Position> AnchorPoints { get; set; }

        public Viewport(Level level, int mapOriginX, int mapOriginY, int width, int height)
        {
            Level = level;
            MapOrigin = new Position(mapOriginX, mapOriginY);
            Width = width;
            Height = height;
            AnchorPoints = new List<Position>();
            CalculateAnchorPoints();
        }

        private void CalculateAnchorPoints()
        {
            if (Width % 2 == 1 && Height % 2 == 1)
            {
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + Height / 2));
            }

            if (Width % 2 == 0 && Height % 2 == 0)
            {
                AnchorPoints.Add(new Position(MapOrigin.X + (Width / 2) - 1, MapOrigin.Y + Height / 2));
                AnchorPoints.Add(new Position(MapOrigin.X + (Width / 2) - 1, MapOrigin.Y + (Height / 2) - 1));
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + (Height / 2) - 1));
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + Height / 2));
                return;
            }

            if (Width % 2 == 0)
            {
                AnchorPoints.Add(new Position(MapOrigin.X + (Width / 2) - 1, MapOrigin.Y + Height / 2));
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + Height / 2));
            }

            if (Height % 2 == 0)
            {
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + (Height / 2) - 1));
                AnchorPoints.Add(new Position(MapOrigin.X + Width / 2, MapOrigin.Y + Height / 2));
            }
        }

        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    MapOrigin = new Position(MapOrigin.X, MapOrigin.Y + 1);
                    break;
                case Direction.South:
                    MapOrigin = new Position(MapOrigin.X, MapOrigin.Y - 1);
                    break;
                case Direction.West:
                    MapOrigin = new Position(MapOrigin.X - 1, MapOrigin.Y);
                    break;
                case Direction.East:
                    MapOrigin = new Position(MapOrigin.X + 1, MapOrigin.Y);
                    break;
                case Direction.NorthEast:
                    MapOrigin = new Position(MapOrigin.X + 1, MapOrigin.Y + 1);
                    break;
                case Direction.NorthWest:
                    MapOrigin = new Position(MapOrigin.X - 1, MapOrigin.Y + 1);
                    break;
                case Direction.SouthEast:
                    MapOrigin = new Position(MapOrigin.X + 1, MapOrigin.Y - 1);
                    break;
                case Direction.SouthWest:
                    MapOrigin = new Position(MapOrigin.X - 1, MapOrigin.Y - 1);
                    break;
                default:
                    return;
            }
        }
    }
}
