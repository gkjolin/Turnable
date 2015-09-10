using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Tiled;

namespace Turnable.Characters
{
    public class CharacterManager : ICharacterManager
    {
        public ILevel Level { get; set; }
        public IList<Entity> Pcs { get; set; }
        public IList<Entity> Npcs { get; set; }
        public Entity Player { get; set; }

        public CharacterManager(ILevel level)
        {
            Level = level;
            Pcs = new List<Entity>();
            Npcs = new List<Entity>();
        }

        public Movement MoveCharacter(Entity character, Position destination)
        {
            Movement movement = new Movement();
            movement.Path = new List<Position>();
            Position characterOrigin = character.Get<Position>();

            if (Level.IsCollidable(destination))
            {
                movement.Status = MovementStatus.HitObstacle;
            }
            else
            {
                character.Remove<Position>();
                character.Add(destination);
                movement.Status = MovementStatus.Success;
                Level.SpecialLayers[SpecialLayer.Character].MoveTile(characterOrigin, destination);
            }

            //if (!(new Node(Level, destination.X, destination.Y).IsWithinBounds()))
            //{
            //    returnValue.Status = MoveResultStatus.OutOfBounds;
            //}
            //else if (Level.IsCharacterAt(destination.X, destination.Y))
            //{
            //    returnValue.Status = MoveResultStatus.HitCharacter;
            //}

            // This is the complete path that the character moved OR attempted
            movement.Path.Add(characterOrigin);
            movement.Path.Add(destination);
            
            OnCharacterMoved(new CharacterMovedEventArgs(character, movement));

            return movement;
        }

        public Movement MoveCharacter(Entity character, Direction direction)
        {
            return MoveCharacter(character, character.Get<Position>().NeighboringPosition(direction));
        }

        public Movement MovePlayer(Direction direction)
        {
            return MoveCharacter(Player, Player.Get<Position>().NeighboringPosition(direction));
        }

        public void SetUpPcs()
        {
            List<SpecialTile> pcTiles = null;

            foreach (Tileset tileset in Level.Map.Tilesets) 
            {
                pcTiles = tileset.FindSpecialTiles("IsPC", "true");
            }

            // TODO: Test that missing playerTiles still allows SetUpPcs to succeed.
            foreach (Tile tile in Level.SpecialLayers[SpecialLayer.Character].Tiles.Values)
            {
                foreach (SpecialTile pcTile in pcTiles)
                {
                    if (tile.GlobalId == pcTile.GlobalId)
                    {
                        SetUpPc(new Position(tile.X, tile.Y));
                    }
                }
            }
        }

        public void SetUpNpcs()
        {
            throw new NotImplementedException();
        }

        private void SetUpPc(Position startingPosition)
        {
            Entity pc = new Entity();
            pc.Add(startingPosition);
            Pcs.Add(pc);
        }
        
        protected virtual void OnCharacterMoved(CharacterMovedEventArgs e)
        {
            if (CharacterMoved != null)
            {
                CharacterMoved(this, e);
            }
        }

        public event EventHandler<CharacterMovedEventArgs> CharacterMoved;
    }
}
