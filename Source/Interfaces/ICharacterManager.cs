using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Locations;

namespace TurnItUp.Interfaces
{
    public interface ICharacterManager
    {
        List<Entity> Characters { get; set; }
        List<Entity> TurnQueue { get; set; }
        ILevel Level { get; set; }
        Entity Player { get; set; }

        void SetUpCharacters();
        void SetUpPlayer(string model, int X, int Y);
        bool IsCharacterAt(int x, int y);
        MoveResult MovePlayer(Direction direction);
        MoveResult MoveCharacter(Entity character, Direction direction);
        MoveResult MoveCharacterTo(Entity character, Position destination);
        void EndTurn();
        void DestroyCharacter(Entity character);

        event EventHandler<EntityEventArgs> CharacterTurnEnded;
        event EventHandler<CharacterMovedEventArgs> CharacterMoved;
        event EventHandler<EntityEventArgs> CharacterDestroyed;
    }
}

