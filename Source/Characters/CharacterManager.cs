using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Characters
{
    public class CharacterManager : ICharacterManager
    {
        public ILevel Level { get; set; }
        public IList<Entity> Characters { get; set; }
        public Entity Player { get; set; }

        public CharacterManager(ILevel level)
        {
            Level = level;
            Characters = new List<Entity>();
        }

        public void SetUpPlayer(int startingX, int startingY)
        {
            Player = new Entity();
            Player.Add(new Position(startingX, startingY));
        }

        public Movement MoveCharacterTo(Entity character, Position destination)
        {
            Movement movement = new Movement();
            List<Position> path = new List<Position>();

            path.Add(character.Get<Position>());

            character.Remove<Position>();
            character.Add(destination);
            movement.Status = MovementStatus.Success;

            path.Add(destination);

            return movement;
        }
    }
}
