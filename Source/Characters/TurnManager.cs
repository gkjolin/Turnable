using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Characters
{
    public class TurnManager
    {
        public List<Character> TurnQueue { get; private set; }

        public TurnManager(Tileset characterTileset, Layer characterlayer)
        {
            TurnQueue = new List<Character>();
            Character playerCharacter = null;

            foreach (Tile tile in characterlayer.Tiles.Values)
            {
                Character character = new Character(tile.X, tile.Y);
                ReferenceTile referenceTile = characterTileset.FindReferenceTileByProperty("IsPlayer", "true");

                if (referenceTile != null && referenceTile.Id == ((int)tile.Gid - characterTileset.FirstGid))
                {
                    character.IsPlayer = true;
                    playerCharacter = character;
                }

                TurnQueue.Add(character);
            }

            TurnQueue.Remove(playerCharacter);
            TurnQueue.Insert(0, playerCharacter);
        }
    }
}
