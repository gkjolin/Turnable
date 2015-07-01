using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Chunk
    {
        public Position Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Chunk(Position position, int width, int height)
        {
            Position = position;
            Width = width;
            Height = height;
        }

        public List<Chunk> Split(SplitDirection splitDirection, int distance)
        {
            List<Chunk> splitChunks = new List<Chunk>();

            switch(splitDirection)
            {
                case SplitDirection.Vertical:
                    splitChunks.Add(new Chunk(Position, distance, Height));
                    splitChunks.Add(new Chunk(new Position(Position.X + distance, Position.Y), Width - distance, Height));
                    break;
                case SplitDirection.Horizontal:
                    splitChunks.Add(new Chunk(Position, Width, distance));
                    splitChunks.Add(new Chunk(new Position(Position.X, Position.Y + distance), Width, Height - distance));
                    break;
            }

            return splitChunks;
        }
    }
}
