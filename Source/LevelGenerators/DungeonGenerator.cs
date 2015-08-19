using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Utilities;
using Turnable.Vision;

namespace Turnable.LevelGenerators
{
    // http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
    public class DungeonGenerator : IDungeonGenerator
    {
        // TODO: This code feels like it could be improved. The way that the code uses tree and passes it around feels off.
        // However, this code is not a part of the public interface so this is not high priority.
        public virtual BinaryTree<Chunk> Chunkify(Chunk initialChunk)
        {
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>();
            tree.Root = new BinaryTreeNode<Chunk>(initialChunk);

            RecursivelyChunkFrom(tree.Root);

            return tree;
        }

        public virtual List<Chunk> CollectLeafChunks(BinaryTree<Chunk> tree, BinaryTreeNode<Chunk> startingRootNode = null)
        {
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes(startingRootNode);
            List<Chunk> chunks = leafNodes.Select<BinaryTreeNode<Chunk>, Chunk>(btn => btn.Value).ToList<Chunk>();

            return chunks;
        }

        public virtual List<Room> PlaceRooms(List<Chunk> chunks)
        {
            List<Room> rooms = new List<Room>();

            foreach (Chunk chunk in chunks)
            {
                rooms.Add(new Room(chunk, Rectangle.BuildRandomRectangle(chunk.Bounds)));
            }

            return rooms;
        }

        private void RecursivelyChunkFrom(BinaryTreeNode<Chunk> parentNode)
        {
            // TODO: Put the ability to select randomly from an Enum into the RNG?
            Array values = Enum.GetValues(typeof(SplitDirection));
            Random random = new Random();
            SplitDirection randomSplitDirection = (SplitDirection)values.GetValue(random.Next(values.Length));
            List<Chunk> splitChunks = null;

            if (randomSplitDirection == SplitDirection.Vertical)
            {
                splitChunks = parentNode.Value.Split(randomSplitDirection, random.Next(2, parentNode.Value.Bounds.Width), 2);
            }
            if (randomSplitDirection == SplitDirection.Horizontal)
            {
                splitChunks = parentNode.Value.Split(randomSplitDirection, random.Next(2, parentNode.Value.Bounds.Height), 2);
            }

            if (splitChunks.Count != 0)
            {
                parentNode.Left = new BinaryTreeNode<Chunk>();
                parentNode.Right = new BinaryTreeNode<Chunk>();

                parentNode.Left.Value = splitChunks[0];
                parentNode.Right.Value = splitChunks[1];

                RecursivelyChunkFrom(parentNode.Left);
                RecursivelyChunkFrom(parentNode.Right);
            }
        }

        public virtual List<Corridor> JoinRooms(BinaryTree<Chunk> tree)
        {
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes();
            List<BinaryTreeNode<Chunk>> processedLeafNodes = new List<BinaryTreeNode<Chunk>>();
            List<Corridor> corridors = new List<Corridor>();

            RecursivelyJoinRooms(tree, tree.Root, corridors);

            return corridors;
        }

        private void RecursivelyJoinRooms(BinaryTree<Chunk> tree, BinaryTreeNode<Chunk> node, List<Corridor> corridors)
        {
            if (tree.IsLeaf(node))
            {
                return;
            }

            RecursivelyJoinRooms(tree, node.Left, corridors);
            RecursivelyJoinRooms(tree, node.Right, corridors);

            List<Room> firstListOfRooms = CollectLeafChunks(tree, node.Left).Select<Chunk, Room>(c => c.Room).ToList<Room>();
            List<Room> secondListOfRooms = CollectLeafChunks(tree, node.Right).Select<Chunk, Room>(c => c.Room).ToList<Room>();
            List<Room> roomsToJoin = ChooseRoomsToJoin(firstListOfRooms, secondListOfRooms);
            Corridor corridor = roomsToJoin[0].Join(roomsToJoin[1]);
            corridors.Add(corridor);
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

        public virtual Level Generate(Chunk initialChunk, out BinaryTree<Chunk> tree)
        {
            tree = Chunkify(initialChunk);
            List<Chunk> randomChunks = CollectLeafChunks(tree);
            PlaceRooms(randomChunks);
            JoinRooms(tree);

            return null;
        }

        public List<Room> CollectRooms(BinaryTree<Chunk> tree, BinaryTreeNode<Chunk> startingRootNode)
        {
            List<BinaryTreeNode<Chunk>> leafNodes = tree.CollectLeafNodes(startingRootNode);
            List<Room> rooms = leafNodes.Select<BinaryTreeNode<Chunk>, Room>(btn => btn.Value.Room).ToList<Room>();

            return rooms;
        }

        public List<Room> ChooseRoomsToJoin(List<Room> firstListOfRooms, List<Room> secondListOfRooms)
        {
            List<Room> roomsToJoin = new List<Room>();
            List<LineSegment> closestEdges;
            int shortestDistance = int.MaxValue;

            foreach (Room roomFromFirstList in firstListOfRooms)
            {
                foreach (Room roomFromSecondList in secondListOfRooms)
                {
                    closestEdges = roomFromFirstList.Bounds.GetClosestEdges(roomFromSecondList.Bounds);
                    if (closestEdges[0].DistanceBetween(closestEdges[1]) <= shortestDistance)
                    {
                        shortestDistance = closestEdges[0].DistanceBetween(closestEdges[1]);
                        roomsToJoin.Clear();
                        roomsToJoin.Add(roomFromFirstList);
                        roomsToJoin.Add(roomFromSecondList);
                    }
                }
            }

            return roomsToJoin;
        }

        public void DrawLevel(BinaryTree<Chunk> tree, out Level level)
        {
            throw new NotImplementedException();
        }
    }
}
