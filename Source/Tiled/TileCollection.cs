using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Turnable.Tiled.Api;
using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Turnable.Tiled
{
    public class TileCollection : Dictionary<ICoordinates, Tile>, ITileCollection
    {
        public TileCollection()
        {
        }

        public TileCollection(int layerWidth, int layerHeight, Data data)
        {
            if (data != null)
            {
                using (BinaryReader reader = new BinaryReader(data.Contents))
                {
                    for (int row = 0; row < layerHeight; row++)
                    {
                        for (int col = 0; col < layerWidth; col++)
                        {
                            uint tileGlobalId = reader.ReadUInt32();

                            // The .tmx format uses 0 to indicate a tile that hasn't been set in the editor
                            if (tileGlobalId != 0)
                            {
                                Coordinates location = new Coordinates(col, row);
                                this[location] = new Tile(tileGlobalId);
                            }
                            else
                            {
                                Console.WriteLine("Null");
                                Console.WriteLine(row);
                                Console.WriteLine(col);
                            }
                        }
                    }
                }
            }
        }

        public Tile this[Coordinates coordinates]
        {
            get
            {
                if (ContainsKey(coordinates))
                {
                    return base[coordinates];
                }

                return null;
            }

            set
            {
                Remove(coordinates); // Always remove a tile at an existing coordinates
                Add(coordinates, value);
            }
        }
    }
}
