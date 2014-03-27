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
        public ILevel Level { get; set; }
        public List<Entity> TurnQueue { get; set; }

        public bool IsCharacterAt(int x, int y)
        {
            return Characters.Find(c => c.GetComponent<Position>().X == x && c.GetComponent<Position>().Y == y) != null;
        }

        public CharacterManager()
        {
        }

        public CharacterManager(IWorld world, ILevel level)
        {
            Tileset characterTileset = level.Map.Tilesets["Characters"];
            Layer characterLayer = level.Map.Layers["Characters"];
            Characters = new List<Entity>();
            TurnQueue = new List<Entity>();

            foreach (Tile tile in characterLayer.Tiles.Values)
            {
                Entity character = null;
                ReferenceTile referenceTile = null;

                // Is there a reference tile for this character?
                if (characterTileset.ReferenceTiles.ContainsKey((int)tile.Gid - characterTileset.FirstGid))
                {
                    referenceTile = characterTileset.ReferenceTiles[(int)tile.Gid - characterTileset.FirstGid];
                }

                if (referenceTile != null && referenceTile.Properties.ContainsKey("IsPlayer") && referenceTile.Properties["IsPlayer"] == "true")
                {
                    character = world.CreateEntityFromTemplate<PC>();
                    Player = character;
                }
                else
                {
                    character = world.CreateEntityFromTemplate<Npc>();
                }

                // Set the model of this character
                if (referenceTile != null && referenceTile.Properties.ContainsKey("Model"))
                {
                    character.AddComponent(new Model(referenceTile.Properties["Model"]));
                }

                character.GetComponent<OnLevel>().Level = level;
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

            Level = level;
        }

        public virtual MoveResult MovePlayer(Direction direction)
        {
            return MoveCharacter(Player, direction);
        }

        public virtual MoveResult MoveCharacterTo(Entity character, Position destination)
        {
            MoveResult returnValue = new MoveResult();
            List<Position> positionChanges = new List<Position>();
            Position currentPosition = character.GetComponent<Position>().DeepClone();
            positionChanges.Add(currentPosition);

            if (Level.IsObstacle(destination.X, destination.Y))
            {
                returnValue.Status = MoveResultStatus.HitObstacle;
            }
            else if (Level.IsCharacterAt(destination.X, destination.Y))
            {
                returnValue.Status = MoveResultStatus.HitCharacter;
            }
            else
            {
                character.GetComponent<Position>().X = destination.X;
                character.GetComponent<Position>().Y = destination.Y;
                returnValue.Status = MoveResultStatus.Success;
            }

            positionChanges.Add(destination);
            returnValue.Path = positionChanges;

            OnCharacterMoved(new CharacterMovedEventArgs(character, returnValue));
            return returnValue;
        }

        public virtual MoveResult MoveCharacter(Entity character, Direction direction)
        {
            Position newPosition = new Position();

            switch (direction)
            {
                case Direction.North:
                    newPosition = new Position(character.GetComponent<Position>().X, character.GetComponent<Position>().Y + 1);
                    break;
                case Direction.South:
                    newPosition = new Position(character.GetComponent<Position>().X, character.GetComponent<Position>().Y - 1);
                    break;
                case Direction.West:
                    newPosition = new Position(character.GetComponent<Position>().X - 1, character.GetComponent<Position>().Y);
                    break;
                case Direction.East:
                    newPosition = new Position(character.GetComponent<Position>().X + 1, character.GetComponent<Position>().Y);
                    break;
                case Direction.NorthEast:
                    newPosition = new Position(character.GetComponent<Position>().X + 1, character.GetComponent<Position>().Y + 1);
                    break;
                case Direction.NorthWest:
                    newPosition = new Position(character.GetComponent<Position>().X - 1, character.GetComponent<Position>().Y + 1);
                    break;
                case Direction.SouthEast:
                    newPosition = new Position(character.GetComponent<Position>().X + 1, character.GetComponent<Position>().Y - 1);
                    break;
                case Direction.SouthWest:
                    newPosition = new Position(character.GetComponent<Position>().X - 1, character.GetComponent<Position>().Y - 1);
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

            OnCharacterTurnEnded(new EntityEventArgs(currentCharacter));
        }

        public virtual event EventHandler<EntityEventArgs> CharacterTurnEnded;
        public virtual event EventHandler<CharacterMovedEventArgs> CharacterMoved;
        public virtual event EventHandler<EntityEventArgs> CharacterDestroyed;

        protected virtual void OnCharacterDestroyed(EntityEventArgs e)
        {
            if (CharacterDestroyed != null)
            {
                CharacterDestroyed(this, e);
            }
        }

        protected virtual void OnCharacterTurnEnded(EntityEventArgs e)
        {
            if (CharacterTurnEnded != null)
            {
                CharacterTurnEnded(this, e);
            }
        }

        protected virtual void OnCharacterMoved(CharacterMovedEventArgs e)
        {
            if (CharacterMoved != null)
            {
                CharacterMoved(this, e);
            }
        }

        public void DestroyCharacter(Entity character)
        {
            Characters.Remove(character);
            TurnQueue.Remove(character);
            Level.World.DestroyEntity(character);
            OnCharacterDestroyed(new EntityEventArgs(character));
        }
    }
}
