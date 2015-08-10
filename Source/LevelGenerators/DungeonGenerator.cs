using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Utilities;

namespace Turnable.LevelGenerators
{
    // http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
    public class DungeonGenerator : IDungeonGenerator
    {
        // TODO: This code feels like it could be improved. The way that the code uses tree and passes it around feels off.
        // However, this code is not a part of the public interface so this is not high priority.
        public BinaryTree<Chunk> Chunkify(Chunk initialChunk)
        {
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>();
            tree.Root = new BinaryTreeNode<Chunk>(initialChunk);

            RecursivelyChunkFrom(tree.Root);

            return tree;
        }

        public List<Chunk> CollectChunks(BinaryTree<Chunk> tree)
        {
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes();
            List<Chunk> chunks = leafNodes.Select<BinaryTreeNode<Chunk>, Chunk>(btn => btn.Value).ToList<Chunk>();

            return chunks;
        }

        public List<Room> PlaceRooms(List<Chunk> chunks)
        {
            List<Room> rooms = new List<Room>();

            foreach (Chunk chunk in chunks)
            {
                rooms.Add(new Room(chunk, Rectangle.BuildRandomRectangle(chunk.Bounds)));
            }

            return rooms;
        }

        private void RecursivelyChunkFrom(BinaryTreeNode<Chunk> parentChunk)
        {
            // TODO: Put the ability to select randomly from an Enum into the RNG?
            Array values = Enum.GetValues(typeof(SplitDirection));
            Random random = new Random();
            SplitDirection randomSplitDirection = (SplitDirection)values.GetValue(random.Next(values.Length));

            List<Chunk> splitChunks = parentChunk.Value.Split(randomSplitDirection, random.Next(2, 11), 2);

            if (splitChunks.Count != 0)
            {
                parentChunk.Left = new BinaryTreeNode<Chunk>();
                parentChunk.Right = new BinaryTreeNode<Chunk>();

                parentChunk.Left.Value = splitChunks[0];
                parentChunk.Right.Value = splitChunks[1];

                RecursivelyChunkFrom(parentChunk.Left);
                RecursivelyChunkFrom(parentChunk.Right);
            }
        }

        public List<Corridor> JoinRooms(BinaryTree<Chunk> tree)
        {
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes();
            List<BinaryTreeNode<Chunk>> processedLeafNodes = new List<BinaryTreeNode<Chunk>>();

            RecursivelyJoinRooms(tree.Root);

            return null;
        }

        private void RecursivelyJoinRooms(BinaryTreeNode<Chunk> node)
        {
        }

        public Corridor GetCorridor(Room firstRoom, Room secondRoom)
        {
            Corridor corridor = new Corridor(firstRoom, secondRoom, null);

            if (firstRoom.Bounds.IsTouching(secondRoom.Bounds))
            {
                return null;
            }

            return corridor;
        }

        public Level Generate()
        {
            throw new NotImplementedException();
        }
    }
}
