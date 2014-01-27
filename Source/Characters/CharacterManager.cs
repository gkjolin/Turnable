using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Characters
{
    public class CharacterManager : ICharacterManager
    {
        public List<Character> Characters { get; set; }
        public Character PlayerCharacter { get; set; }
        public Board Board { get; set; }

        public bool IsCharacterAt(int x, int y)
        {
            return Characters.Find(c => c.Position == new Position(x, y)) != null;
        }

        public CharacterManager(Board board)
        {
            Tileset characterTileset = board.Map.Tilesets["Characters"];
            Layer characterLayer = board.Map.Layers["Characters"];
            Characters = new List<Character>();

            foreach (Tile tile in characterLayer.Tiles.Values)
            {
                Character character = new Character(tile.X, tile.Y);
                ReferenceTile referenceTile = characterTileset.FindReferenceTileByProperty("IsPlayer", "true");

                if (referenceTile != null && referenceTile.Id == ((int)tile.Gid - characterTileset.FirstGid))
                {
                    PlayerCharacter = character;
                    PlayerCharacter.IsPlayer = true;
                }

                Characters.Add(character);
            }

            Board = board;
        }

        public Tuple<MoveResult, List<Position>> MovePlayer(Direction direction)
        {
            Tuple<MoveResult, List<Position>> returnValue = new Tuple<MoveResult, List<Position>>();
            List<Position> positionChanges = new List<Position>();
            positionChanges.Add(PlayerCharacter.Position.DeepClone());
            Position newPosition = new Position(0, 0);

            switch (direction)
            {
                case Direction.Up:
                    newPosition = new Position(PlayerCharacter.Position.X, PlayerCharacter.Position.Y - 1);
                    break;
                case Direction.Down:
                    newPosition = new Position(PlayerCharacter.Position.X, PlayerCharacter.Position.Y + 1);
                    break;
                case Direction.Left:
                    newPosition = new Position(PlayerCharacter.Position.X - 1, PlayerCharacter.Position.Y);
                    break;
                case Direction.Right:
                    newPosition = new Position(PlayerCharacter.Position.X + 1, PlayerCharacter.Position.Y);
                    break;
            }

            if (Board.IsObstacle(newPosition.X, newPosition.Y))
            {
                returnValue.Element1 = MoveResult.HitObstacle;
            }
            else if (Board.IsCharacterAt(newPosition.X, newPosition.Y))
            {
                returnValue.Element1 = MoveResult.HitCharacter;
            }
            else
            {
                PlayerCharacter.Position.X = newPosition.X;
                PlayerCharacter.Position.Y = newPosition.Y;
                returnValue.Element1 = MoveResult.Success;
            }

            positionChanges.Add(newPosition);
            returnValue.Element2 = positionChanges;
            return returnValue;
        }
    }
}
