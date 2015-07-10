using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Room
    {
        public Chunk ParentChunk { get; set; }
        public Position TopLeft { get; set; }
        public Position BottomRight { get; set; }

        public Room(Chunk parentChunk, Position firstCorner = null, Position secondCorner = null)
        {
            ParentChunk = parentChunk;
            Random random = new Random();

            if (firstCorner == null)
            {
                firstCorner = new Position(random.Next(0, parentChunk.Width), random.Next(0, parentChunk.Height));
            }
            if (secondCorner == null)
            {
                secondCorner = new Position(random.Next(0, parentChunk.Width), random.Next(0, parentChunk.Height));
            }

            TopLeft = new Position(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
            BottomRight = new Position(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));
        }
    }
}
