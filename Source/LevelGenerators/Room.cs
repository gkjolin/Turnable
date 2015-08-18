using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Utilities;
using Turnable.Vision;

namespace Turnable.LevelGenerators
{
    public class Room : IBounded
    {
        public Chunk ParentChunk { get; set; }
        public Rectangle Bounds { get; set; }
        public List<Corridor> Corridors { get; private set; }

        public Room(Chunk parentChunk, Rectangle bounds)
        {
            ParentChunk = parentChunk;
            Bounds = bounds;
            parentChunk.Room = this;
            Corridors = new List<Corridor>();
        }

        public Corridor Join(Room secondRoom)
        {
            if (Bounds.IsTouching(secondRoom.Bounds))
            {
                return null;
            }

            List<LineSegment> closestEdges = Bounds.GetClosestEdges(secondRoom.Bounds);
            Corridor corridor = Corridor.Build(closestEdges[0].GetMidpoint(), closestEdges[1].GetMidpoint());
            corridor.ConnectedRooms.Add(this);
            corridor.ConnectedRooms.Add(secondRoom);
            Corridors.Add(corridor);
            secondRoom.Corridors.Add(corridor);

            return corridor;
        }

        public override string ToString()
        {
            return string.Format("BottomLeft: {0}; TopRight: {1}", Bounds.BottomLeft.ToString(), Bounds.TopRight.ToString());
        }
    }
}
