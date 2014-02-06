using Entropy;
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
        public List<Entity> Characters { get; set; }
        public Entity Player { get; set; }
        public Board Board { get; set; }
        public List<Entity> TurnQueue { get; set; }

        public bool IsCharacterAt(int x, int y)
        {
            return Characters.Find(c => c.GetComponent<Position>().X == x && c.GetComponent<Position>().Y == y) != null;
        }

        public CharacterManager(World world, Board board)
        {
            Tileset characterTileset = board.Map.Tilesets["Characters"];
            Layer characterLayer = board.Map.Layers["Characters"];
            Characters = new List<Entity>();
            TurnQueue = new List<Entity>();

            foreach (Tile tile in characterLayer.Tiles.Values)
            {
                Entity character = null;
                ReferenceTile referenceTile = characterTileset.FindReferenceTileByProperty("IsPlayer", "true");

                if (referenceTile != null && referenceTile.Id == ((int)tile.Gid - characterTileset.FirstGid))
                {
                    character = world.CreateEntityFromTemplate<PC>();
                    Player = character;
                }
                else
                {
                    character = world.CreateEntityFromTemplate<Npc>();
                }

                character.GetComponent<OnBoard>().Board = board;
                character.GetComponent<Position>().X = tile.X;
                character.GetComponent<Position>().Y = tile.Y;

                Characters.Add(character);
            }

            foreach (Entity character in Characters)
            {
                TurnQueue.Add(character);
            }
            // Move player to the front of the TurnQueue
            TurnQueue.Remove(Player);
            TurnQueue.Insert(0, Player);

            Board = board;
        }

        public Tuple<MoveResult, List<Position>> MovePlayer(Direction direction)
        {
            Tuple<MoveResult, List<Position>> returnValue = new Tuple<MoveResult, List<Position>>();
            List<Position> positionChanges = new List<Position>();
            positionChanges.Add(Player.GetComponent<Position>().DeepClone());
            Position newPosition = new Position(0, 0);

            switch (direction)
            {
                case Direction.Up:
                    newPosition = new Position(Player.GetComponent<Position>().X, Player.GetComponent<Position>().Y - 1);
                    break;
                case Direction.Down:
                    newPosition = new Position(Player.GetComponent<Position>().X, Player.GetComponent<Position>().Y + 1);
                    break;
                case Direction.Left:
                    newPosition = new Position(Player.GetComponent<Position>().X - 1, Player.GetComponent<Position>().Y);
                    break;
                case Direction.Right:
                    newPosition = new Position(Player.GetComponent<Position>().X + 1, Player.GetComponent<Position>().Y);
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
                Player.GetComponent<Position>().X = newPosition.X;
                Player.GetComponent<Position>().Y = newPosition.Y;
                returnValue.Element1 = MoveResult.Success;
            }

            positionChanges.Add(newPosition);
            returnValue.Element2 = positionChanges;
            return returnValue;
        }

        public void EndTurn()
        {
            Entity currentCharacter = TurnQueue[0];

            TurnQueue.Remove(currentCharacter);
            TurnQueue.Add(currentCharacter);
        }
    }
}
