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

        public List<Chunk> Split(SplitDirection splitDirection, int splitDistance, int minimumSplitChunkSize = 1)
        {
            List<Chunk> splitChunks = new List<Chunk>();

            if (splitDistance < minimumSplitChunkSize) throw new ArgumentException();

            switch(splitDirection)
            {
                case SplitDirection.Vertical:
                    splitChunks.Add(new Chunk(Position, splitDistance, Height));
                    splitChunks.Add(new Chunk(new Position(Position.X + splitDistance, Position.Y), Width - splitDistance, Height));

                    // If any of the new chunks does not meet the minimumSplitChunkSize requirement, return an empty list.
                    if (splitChunks[0].Width < minimumSplitChunkSize || splitChunks[1].Width < minimumSplitChunkSize)
                    {
                        splitChunks.Clear();
                        return splitChunks;
                    }

                    break;
                case SplitDirection.Horizontal:
                    if (splitDistance == Height) return splitChunks;
                    splitChunks.Add(new Chunk(Position, Width, splitDistance));
                    splitChunks.Add(new Chunk(new Position(Position.X, Position.Y + splitDistance), Width, Height - splitDistance));

                    // If any of the new chunks does not meet the minimumSplitChunkSize requirement, return an empty list.
                    if (splitChunks[0].Height < minimumSplitChunkSize || splitChunks[1].Height < minimumSplitChunkSize)
                    {
                        splitChunks.Clear();
                        return splitChunks;
                    }

                    break;
            }

            // If any of the new chunks are invalid (have a width <= 0) return an empty list.
            if (splitChunks[0].Width <= 0 || splitChunks[0].Height <= 0 || splitChunks[1].Height <= 0 || splitChunks[1].Width <= 0)
            {
                splitChunks.Clear();
            }


            return splitChunks;
        }
    }
}
