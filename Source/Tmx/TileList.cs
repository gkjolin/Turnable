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
    public class TileList
    {
        // Layers in a TileMap tend to be sparse arrays. A dictionary is a much better data store than a 2-dimensional array.
        private Dictionary<Tuple<int, int>, uint> _tiles;

        public int Count 
        { 
            get 
            {
                return _tiles.Count;
            } 
        }

        public TileList(Data data, int width, int height)
        {
            _tiles = new Dictionary<Tuple<int, int>, uint>();
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
                            _tiles.Add(new Tuple<int, int>(col, row), tileGid);
                        }
                    }
                }
            }
        }
    }
}
