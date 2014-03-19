using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Tuples;

namespace TurnItUp.Tmx
{
    public class TileList : Dictionary<Tuple<int, int>, Tile> 
    {
        // Layers in a TileMap tend to be sparse arrays. A dictionary is a much better data store than a 2-dimensional array.

        public TileList()
        {
        }

        public TileList(Data data, int width, int height)
        {
            uint tileGid = 0;

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                for (int row = 0; row < height; row++)
                {
                    for (int col = 0; col < width; col++) 
                    {
                        tileGid = reader.ReadUInt32();

                        // The .tmx format uses 0 to indicate a tile that hasn't been set in the editor
                        if (tileGid != 0)
                        {
                            Add(new Tuple<int, int>(col, row), new Tile(tileGid, col, row));
                        }
                    }
                }
            }
        }

        public void Merge(TileList tileList)
        {
            // http://stackoverflow.com/questions/294138/merging-dictionaries-in-c-sharp
            tileList.ToList().ForEach(x => this.Add(x.Key, x.Value));
        }
    }
}
