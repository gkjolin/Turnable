using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.LevelGenerators
{
    // http://www.roguebasin.com/index.php?title=Basic_BSP_Dungeon_generation
    public class DungeonGenerator : IDungeonGenerator
    {
        private List<Chunk> _randomChunks;

        public List<Chunk> Chunkify(Chunk initialChunk)
        {
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>();
            tree.Root = new BinaryTreeNode<Chunk>(initialChunk);

            RecursivelyChunkFrom(tree.Root);

            _randomChunks = new List<Chunk>();

            CollectLeafNodes(tree.Root);

            return _randomChunks;
        }

        private void CollectLeafNodes(BinaryTreeNode<Chunk> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.Left == null && node.Right == null) 
            {
                _randomChunks.Add(node.Value);
            }
            CollectLeafNodes(node.Left);
            CollectLeafNodes(node.Right);
        }

        private void RecursivelyChunkFrom(BinaryTreeNode<Chunk> parentChunk)
        {
            // TODO: Put this in the RNG?
            Array values = Enum.GetValues(typeof(SplitDirection));
            Random random = new Random();
            SplitDirection randomSplitDirection = (SplitDirection)values.GetValue(random.Next(values.Length));

            List<Chunk> splitChunks = parentChunk.Value.Split(randomSplitDirection, random.Next(2, 10), 2);

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
    }
}
