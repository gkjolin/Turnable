using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;

namespace TurnItUp.Characters
{
    public class CharacterManager
    {
        public List<Character> Characters { get; private set; }
        public Character PlayerCharacter { get; private set; }

        public CharacterManager(Tileset characterTileset, Layer characterlayer)
        {
            Characters = new List<Character>();

            foreach (Tile tile in characterlayer.Tiles.Values)
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
        }
    }
}
