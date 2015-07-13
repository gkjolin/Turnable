using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Utilities;

namespace Turnable.LevelGenerators
{
    public class Chunk : IBounded
    {
        public Rectangle Bounds { get; set; }

        public Chunk(Rectangle bounds)
        {
            Bounds = bounds;
        }

        public List<Chunk> Split(SplitDirection splitDirection, int splitDistance, int minimumSplitChunkSize = 1)
        {
            List<Chunk> splitChunks = new List<Chunk>();

            if (splitDistance < minimumSplitChunkSize) throw new ArgumentException();

            switch(splitDirection)
            {
                case SplitDirection.Vertical:
                    // The splitDistance is equal to the Width of the chunk to be split, so return an empty list of split chunks
                    if (splitDistance == Bounds.Width) return splitChunks;

                    Rectangle firstChunkBounds = new Rectangle(Bounds.TopLeft, splitDistance, Bounds.Height);
                    splitChunks.Add(new Chunk(firstChunkBounds));
                    Rectangle secondChunkBounds = new Rectangle(new Position(Bounds.TopLeft.X + splitDistance, Bounds.TopLeft.Y), Bounds.Width - splitDistance, Bounds.Height);
                    splitChunks.Add(new Chunk(secondChunkBounds));

                    // If any of the new chunks does not meet the minimumSplitChunkSize requirement, return an empty list.
                    if (splitChunks[0].Bounds.Width < minimumSplitChunkSize || splitChunks[1].Bounds.Width < minimumSplitChunkSize)
                    {
                        splitChunks.Clear();
                        return splitChunks;
                    }

                    break;
                case SplitDirection.Horizontal:
                    // The splitDistance is equal to the Height of the chunk to be split, so return an empty list of split chunks
                    if (splitDistance == Bounds.Height) return splitChunks;

                    firstChunkBounds = new Rectangle(Bounds.TopLeft, Bounds.Width, splitDistance);
                    splitChunks.Add(new Chunk(firstChunkBounds));
                    secondChunkBounds = new Rectangle(new Position(Bounds.TopLeft.X, Bounds.TopLeft.Y + splitDistance), Bounds.Width, Bounds.Height - splitDistance);
                    splitChunks.Add(new Chunk(secondChunkBounds));

                    // If any of the new chunks does not meet the minimumSplitChunkSize requirement, return an empty list.
                    if (splitChunks[0].Bounds.Height < minimumSplitChunkSize || splitChunks[1].Bounds.Height < minimumSplitChunkSize)
                    {
                        splitChunks.Clear();
                        return splitChunks;
                    }

                    break;
            }

            return splitChunks;
        }
    }
}
