using Entropy;
using Entropy.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Characters;
using Turnable.Components;
using Turnable.LevelGenerators;
using Turnable.Locations;
using Turnable.Utilities;

namespace Turnable.Api
{
    public interface IDungeonGenerator
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        Level Generate(Chunk initialChunk, out BinaryTree<Chunk> tree);

        // Properties

        // Events

        // -----------------
        // Private interface
        // -----------------

        // Methods
        BinaryTree<Chunk> Chunkify(Chunk initialChunk);
        List<Chunk> CollectLeafChunks(BinaryTree<Chunk> tree, BinaryTreeNode<Chunk> startingRootNode = null);
        List<Room> CollectRooms(BinaryTree<Chunk> tree, BinaryTreeNode<Chunk> startingRootNode = null);
        List<Room> PlaceRooms(List<Chunk> chunks);
        List<Corridor> JoinRooms(BinaryTree<Chunk> tree);
        List<Room> ChooseRoomsToJoin(List<Room> firstListOfRooms, List<Room> secondListOfRooms);
        void DrawLevel(BinaryTree<Chunk> tree, Level level);

        // Properties

        // Events
    }
}
