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
        public virtual Entity Player { get; set; }
        public Board Board { get; set; }
        public List<Entity> TurnQueue { get; set; }

        public bool IsCharacterAt(int x, int y)
        {
            return Characters.Find(c => c.GetComponent<Position>().X == x && c.GetComponent<Position>().Y == y) != null;
        }

        public CharacterManager()
        {
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

        public virtual Tuple<MoveResult, List<Position>> MovePlayer(Direction direction)
        {
            return MoveCharacter(Player, direction);
        }

        public virtual Tuple<MoveResult, List<Position>> MoveCharacterTo(Entity character, Position destination)
        {
            Tuple<MoveResult, List<Position>> returnValue = new Tuple<MoveResult, List<Position>>();
            List<Position> positionChanges = new List<Position>();
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            positionChanges.Add(currentPosition);

            if (Board.IsObstacle(destination.X, destination.Y))
            {
                returnValue.Element1 = MoveResult.HitObstacle;
            }
            else if (Board.IsCharacterAt(destination.X, destination.Y))
            {
                returnValue.Element1 = MoveResult.HitCharacter;
            }
            else
            {
                character.GetComponent<Position>().X = destination.X;
                character.GetComponent<Position>().Y = destination.Y;
                returnValue.Element1 = MoveResult.Success;
            }

            positionChanges.Add(destination);
            returnValue.Element2 = positionChanges;

            OnCharacterMoved(new CharacterMovedEventArgs(character, returnValue));
            return returnValue;
        }

        public virtual Tuple<MoveResult, List<Position>> MoveCharacter(Entity character, Direction direction)
        {
            Position newPosition = new Position();

            switch (direction)
            {
                case Direction.Up:
                    newPosition = new Position(character.GetComponent<Position>().X, character.GetComponent<Position>().Y - 1);
                    break;
                case Direction.Down:
                    newPosition = new Position(character.GetComponent<Position>().X, character.GetComponent<Position>().Y + 1);
                    break;
                case Direction.Left:
                    newPosition = new Position(character.GetComponent<Position>().X - 1, character.GetComponent<Position>().Y);
                    break;
                case Direction.Right:
                    newPosition = new Position(character.GetComponent<Position>().X + 1, character.GetComponent<Position>().Y);
                    break;
                default:
                    return null;
            }

            return MoveCharacterTo(character, newPosition);
        }

        public void EndTurn()
        {
            Entity currentCharacter = TurnQueue[0];

            TurnQueue.Remove(currentCharacter);
            TurnQueue.Add(currentCharacter);

            OnTurnEnded(new EntityEventArgs(currentCharacter));
        }

        public virtual event EventHandler<EntityEventArgs> TurnEnded;
        public virtual event EventHandler<CharacterMovedEventArgs> CharacterMoved;

        protected virtual void OnTurnEnded(EntityEventArgs e)
        {
            if (TurnEnded != null)
            {
                TurnEnded(this, e);
            }
        }

        protected virtual void OnCharacterMoved(CharacterMovedEventArgs e)
        {
            if (CharacterMoved != null)
            {
                CharacterMoved(this, e);
            }
        }
    }
}
