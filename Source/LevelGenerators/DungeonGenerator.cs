using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.LevelGenerators
{
    public class DungeonGenerator
    {
        private List<Chunk> _randomChunks;

        public List<Chunk> GenerateFrom(Chunk initialChunk)
        {
            BinaryTree<Chunk> tree = new BinaryTree<Chunk>();
            tree.Root = new BinaryTreeNode<Chunk>(initialChunk);

            RecursivelyChunkFrom(tree.Root);

            _randomChunks = new List<Chunk>();

            CollectLeafNodes(tree.Root);


            randomChunks
                  if(node == NULL)       
    return 0;
  if(node->left == NULL && node->right==NULL)      
    return 1;            
  else
    return getLeafCount(node->left)+
           getLeafCount(node->right); 

            return _randomChunks;
        }

        public void CollectLeafNodes(BinaryTreeNode<Chunk> node)
        {
            if (node == null)
            {
                return;
            }
            if (node.

              if(t == NULL)       
                return;
               if(t.left == NULL && t.right==NULL)      
                  System.out.println(t.element); 
               printLeafNodes(t.left); 
               printLeafNodes(t.right);      
        }

        private void RecursivelyChunkFrom(BinaryTreeNode<Chunk> parentChunk)
        {
            List<Chunk> splitChunks = parentChunk.Value.Split(SplitDirection.Horizontal, 1);

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
