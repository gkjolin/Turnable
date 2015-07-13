using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Utilities;

namespace Turnable.LevelGenerators
{
    public class Room : IBounded
    {
        public Chunk ParentChunk { get; set; }
        public Rectangle Bounds { get; set; } 
        public Position TopLeft { get; set; }
        public Position BottomRight { get; set; }

        public Room(Chunk parentChunk, Rectangle bounds)
        {
            ParentChunk = parentChunk;
            Bounds = bounds;
        }
    }
}
