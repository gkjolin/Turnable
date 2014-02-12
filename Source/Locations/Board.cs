using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;
using System.Tuples;
using TurnItUp.Characters;
using TurnItUp.Components;
using TurnItUp.Interfaces;
using Entropy;
using TurnItUp.Pathfinding;

namespace TurnItUp.Locations
{
    public class Board
    {
        // Facade pattern

        public Map Map { get; private set; }
        public ICharacterManager CharacterManager { get; set; }
        public PathFinder PathFinder { get; private set; }

        // Initialization methods
        public void Initialize(World world, string tmxPath)
        {
            Map = new Map(tmxPath);
            Layer charactersLayer = Map.FindLayerByProperty("IsCharacters", "true");

            if (charactersLayer != null)
            {
                CharacterManager = new CharacterManager(world, this);
            }
        }

        public void InitializePathFinding(bool allowDiagonalMovement = false)
        {
            PathFinder = new PathFinder(this, allowDiagonalMovement);
        }

        // TODO: Test this!
        public virtual List<Node> FindBestPathToMoveAdjacentToPlayer(Position position)
        {
            Position playerPosition = CharacterManager.Player.GetComponent<Position>();

            Node startingNode = new Node(this, position.X, position.Y);
            List<Node> candidateNodes = new Node(this, playerPosition.X, playerPosition.Y).GetAdjacentNodes(false);

            //TODO: Test this!
            candidateNodes.RemoveAll(cn => !(cn.IsWalkable()));
            // If there are no candidate nodes available, there is no path to the player
            if (candidateNodes.Count == 0)
            {
                return null;
            }

            Node closestNode = PathFinder.ClosestNode(startingNode, candidateNodes);
            return PathFinder.SeekPath(startingNode, closestNode, this);
        }

        public bool IsObstacle(int x, int y)
        {
            Layer obstacleLayer = Map.FindLayerByProperty("IsCollision", "true");

            // No obstacle layer exists. Currently the only way to mark obstacles is to use a layer in Tiled that has a IsCollision propert with the value "true"
            if (obstacleLayer == null) return false;

            return (obstacleLayer.Tiles.ContainsKey(new Tuple<int,int>(x, y)));
        }

        public bool IsCharacterAt(int x, int y)
        {
            return CharacterManager.IsCharacterAt(x, y);
        }

        public virtual Tuple<MoveResult, List<Position>> MovePlayer(Direction direction)
        {
            return CharacterManager.MovePlayer(direction);
        }

        public virtual Tuple<MoveResult, List<Position>> MoveCharacterTo(Entity character, Position destination)
        {
            return CharacterManager.MoveCharacterTo(character, destination);
        }
    }
}
