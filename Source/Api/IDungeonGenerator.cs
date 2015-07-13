using Entropy;
using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Characters;
using Turnable.Components;
using Turnable.LevelGenerators;
using Turnable.Locations;

namespace Turnable.Api
{
    public interface IDungeonGenerator
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods

        // Properties

        // Events

        // -----------------
        // Private interface
        // -----------------

        // Methods
        List<Chunk> Chunkify(Chunk initialChunk);
        List<Room> PlaceRooms(List<Chunk> chunks);
        Corridor JoinRooms(Room firstRoom, Room secondRoom);
        List<Position> GetCorridorOrigins(Room firstRoom, Room secondRoom);

        // Properties

        // Events

    }
}
