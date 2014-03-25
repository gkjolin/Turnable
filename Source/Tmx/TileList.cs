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

        public TileList(Layer layer, Data data)
        {
            uint tileGid = 0;

            using (BinaryReader reader = new BinaryReader(data.Contents))
            {
                for (int row = 0; row < layer.Height; row++)
                {
                    for (int col = 0; col < layer.Width; col++) 
                    {
                        tileGid = reader.ReadUInt32();

                        // The .tmx format uses 0 to indicate a tile that hasn't been set in the editor
                        if (tileGid != 0)
                        {
                            // The .tmx format uses an origin that starts at the top left with Y increasing going South
                            // However most libraries use an origin that starts at the bottom left with Y increasing going North
                            // So Y is "flipped" using (height - row - 1)
                            Add(new Tuple<int, int>(col, (layer.Height - row - 1)), new Tile(tileGid, col, (layer.Height - row - 1)));
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
