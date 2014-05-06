using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using TurnItUp.Locations;
using TurnItUp.Pathfinding;
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

        public CharacterManager(ILevel level)
        {
            Level = level;
        }

        public void SetUpCharacters()
        {
            Tileset characterTileset = Level.Map.Tilesets["Characters"];
            Layer characterLayer = Level.Map.Layers["Characters"];
            Characters = new List<Entity>();
            TurnQueue = new List<Entity>();

            foreach (Tile tile in characterLayer.Tiles.Values)
            {
                Entity character = null;
                ReferenceTile referenceTile = null;

                // Is there a reference tile for this character?
                referenceTile = characterTileset.FindReferenceTileByTile(tile);

                if (referenceTile != null)
                {
                    character = Level.World.CreateEntityFromTemplate<Npc>();

                    // Set the model of this character
                    if (referenceTile.Properties.ContainsKey("Model"))
                    {
                        character.AddComponent(new Model(referenceTile.Properties["Model"]));
                    }
                }
                else
                {
                    character = Level.World.CreateEntityFromTemplate<Npc>();
                }

                character.GetComponent<OnLevel>().Level = Level;
                character.GetComponent<Position>().X = tile.X;
                character.GetComponent<Position>().Y = tile.Y;

                Characters.Add(character);
            }

            foreach (Entity character in Characters)
            {
                TurnQueue.Add(character);
            }
        }

        public void SetUpPlayer(string model, int X, int Y)
        {
            ReferenceTile referenceTile = Level.Map.Tilesets["Characters"].FindReferenceTileByProperty("Model", model);
            //referenceTile.Gid

            //Level.Map.Layers["Characters"].
            //CharacterManager characterManager = new CharacterManager(_level);
            //characterManager.SetUpPlayer("Knight M", 7, 1);

            //Assert.IsNotNull(characterManager.Player);
            //Assert.AreEqual(9, characterManager.Characters.Count);
            //Assert.AreEqual(new Position(7, 1), characterManager.Player.GetComponent<Position>());
            //Assert.AreEqual("Knight M", characterManager.Player.GetComponent<Model>());
            //Assert.AreEqual(_level, characterManager.Player.GetComponent<OnLevel>().Level);

            //// Is a TurnQueue setup with the Player taking the first turn?
            //Assert.IsNotNull(characterManager.TurnQueue);
            //Assert.AreEqual(characterManager.Player, characterManager.TurnQueue[0]);
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

            if (!(new Node(Level, destination.X, destination.Y).IsWithinBounds()))
            {
                returnValue.Status = MoveResultStatus.OutOfBounds;
            }
            else if (Level.IsObstacle(destination.X, destination.Y))
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

            // Change the location of the character tiles in the map
            if (returnValue.Status == MoveResultStatus.Success)
            {
                Level.Map.Layers["Characters"].MoveTile(currentPosition, destination);
            }

            OnCharacterMoved(new CharacterMovedEventArgs(character, returnValue));
            return returnValue;
        }

        public virtual MoveResult MoveCharacter(Entity character, Direction direction)
        {
            return MoveCharacterTo(character, character.GetComponent<Position>().InDirection(direction));
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
