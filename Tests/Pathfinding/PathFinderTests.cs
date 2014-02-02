using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Pathfinding
{
    // The sample board:
    // XXXXXXXXXXXXXXXX
    // X....EEE.......X
    // X..........X...X
    // X.......E......X
    // X.E.X..........X
    // X.....E....E...X
    // X........X.....X
    // X..........XXXXX
    // X..........X...X
    // X..........X...X
    // X......X.......X
    // X.X........X...X
    // X..........X...X
    // X..........X...X
    // X......P...X...X
    // XXXXXXXXXXXXXXXX
    // X - Obstacles, P - Player, E - Enemies

    [TestClass]
    public class PathFinderTests
    {
        private Node _node;
        private Board _board;
        private PathFinder _pathFinderWithDiagonalMovement;
        private PathFinder _pathFinderWithoutDiagonalMovement;

        [TestInitialize]
        public void Initialize()
        {
            _node = new Node(0, 0);
            _board = LocationsFactory.BuildBoard();
            _pathFinderWithDiagonalMovement = new PathFinder(true);
            _pathFinderWithoutDiagonalMovement = new PathFinder(false);
        }

        // Diagonal movement allowed

        [TestMethod]
        public void PathFinder_Construction_IsSuccessful()
        {
            PathFinder pathFinder = new PathFinder(true);

            Assert.IsTrue(pathFinder.AllowDiagonalMovement);
        }

        [TestMethod]
        public void PathFinder_WhereEndingAndStartingPointsAreOrthogonalAndNextToEachOther_CanFindPath()
        {
            List<Node> path = _pathFinderWithDiagonalMovement.SeekPath(new Node(1, 1), new Node(1, 2), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(new Node(1, 1), path[0]);
            Assert.AreEqual(new Node(1, 2), path[1]);
        }

        [TestMethod]
        public void PathFinder_WhereEndingAndStartingPointsAreDiagonalAndNextToEachOther_CanFindPath()
        {
            List<Node> path = _pathFinderWithDiagonalMovement.SeekPath(new Node(6, 6), new Node(5, 5), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(2, path.Count);
            Assert.AreEqual(new Node(6, 6), path[0]);
            Assert.AreEqual(new Node(5, 5), path[1]);
        }

        [TestMethod]
        public void PathFinder_WhereEndingAndStartingPointsAreOrthogonallySeparatedAndHaveNoObstaclesBetweenThem_CanFindPath()
        {
            List<Node> path = _pathFinderWithDiagonalMovement.SeekPath(new Node(1, 1), new Node(5, 1), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(5, path.Count);
            Assert.AreEqual(new Node(1, 1), path[0]);
            Assert.AreEqual(new Node(2, 1), path[1]);
            Assert.AreEqual(new Node(3, 1), path[2]);
            Assert.AreEqual(new Node(4, 1), path[3]);
            Assert.AreEqual(new Node(5, 1), path[4]);
        }

        [TestMethod]
        public void PathFinder_WhereEndingAndStartingPointsAreDiagonallySeparatedAndHaveNoObstaclesBetweenThem_CanFindPath()
        {
            List<Node> path = _pathFinderWithDiagonalMovement.SeekPath(new Node(3, 5), new Node(5, 7), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(new Node(3, 5), path[0]);
            Assert.AreEqual(new Node(4, 6), path[1]);
            Assert.AreEqual(new Node(5, 7), path[2]);
        }

        [TestMethod]
        public void PathFinder_WhereEndingAndStartingPointsAreOrthogonallySeparatedAndHaveOneObstacle_CanFindPath()
        {
            List<Node> path = _pathFinderWithDiagonalMovement.SeekPath(new Node(4, 1), new Node(4, 5), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(5, path.Count);
            Assert.AreEqual(new Node(4, 1), path[0]);
            Assert.AreEqual(new Node(4, 2), path[1]);
            Assert.AreEqual(new Node(4, 3), path[2]);
            Assert.AreEqual(new Node(3, 4), path[3]);
            Assert.AreEqual(new Node(4, 5), path[4]);
        }

        // Diagonal movement NOT allowed
        [TestMethod]
        public void PathFinder_WhereDiagonalMovementIsNotAllowedAndStartingAndEndingPointsAreDiagonalAndNextToEachOther_CanFindPath()
        {
            List<Node> path = _pathFinderWithoutDiagonalMovement.SeekPath(new Node(6, 6), new Node(5, 5), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(3, path.Count);
            Assert.AreEqual(new Node(6, 6), path[0]);
            Assert.AreEqual(new Node(6, 5), path[1]);
            Assert.AreEqual(new Node(5, 5), path[2]);
        }

        [TestMethod]
        public void PathFinder_WhereDiagonalMovementIsNotAllowedAndEndingAndStartingPointsAreOrthogonallySeparatedAndHaveOneObstacle_CanFindPath()
        {
            List<Node> path = _pathFinderWithoutDiagonalMovement.SeekPath(new Node(4, 1), new Node(4, 5), _board);

            Assert.IsNotNull(path);
            Assert.AreEqual(7, path.Count);
            Assert.AreEqual(new Node(4, 1), path[0]);
            Assert.AreEqual(new Node(4, 2), path[1]);
            Assert.AreEqual(new Node(4, 3), path[2]);
            Assert.AreEqual(new Node(3, 3), path[3]);
            Assert.AreEqual(new Node(3, 4), path[4]);
            Assert.AreEqual(new Node(3, 5), path[5]);
            Assert.AreEqual(new Node(4, 5), path[6]);
        }

        [TestMethod]
        public void PathFinder_WhenDiagonalMovementIsAllowed_CanDetermineMovementPointCostBetweenTwoNodes()
        {
            Node startingNode, endingNode;

            // Starting and Ending location are the same
            startingNode = new Node(0, 0);
            endingNode = new Node(0, 0);
            Assert.AreEqual(0, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are in the same line vertically or horizontally
            endingNode = new Node(0, 5);
            Assert.AreEqual(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(3, 0);
            Assert.AreEqual(3, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are exactly diagonal to each other
            endingNode = new Node(5, 5);
            Assert.AreEqual(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(3, 3);
            Assert.AreEqual(3, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are not exactly diagonal or in the same line horizontally or vertically
            endingNode = new Node(3, 5);
            Assert.AreEqual(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(6, 1);
            Assert.AreEqual(6, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
        }

        [TestMethod]
        public void PathFinder_WhenDiagonalMovementIsNotAllowed_CanDetermineMovementPointCostBetweenTwoNodes()
        {
            Node startingNode, endingNode;

            // Starting and Ending location are the same
            startingNode = new Node(0, 0);
            endingNode = new Node(0, 0);
            Assert.AreEqual(0, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are in the same line vertically or horizontally
            endingNode = new Node(0, 5);
            Assert.AreEqual(5, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(3, 0);
            Assert.AreEqual(3, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are exactly diagonal to each other
            endingNode = new Node(5, 5);
            Assert.AreEqual(10, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(3, 3);
            Assert.AreEqual(6, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

            // Starting and Ending location are not exactly diagonal or in the same line horizontally or vertically
            endingNode = new Node(3, 5);
            Assert.AreEqual(8, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
            endingNode = new Node(6, 1);
            Assert.AreEqual(7, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
        }

        [TestMethod]
        public void PathFinder_WhenDiagonalMovementIsAllowed_CanFindClosestNodeToAnotherNodeFromASetOfNodes()
        {
            // The sample board:
            // XXXXXXXXXXXXXXXX
            // X....EEE.......X
            // X..........X...X
            // X.......E......X
            // X.E.X..........X
            // X.....E....E...X
            // X........X.....X
            // X.....S....XXXXX
            // X.F..S.....X...X
            // X.....S....X...X
            // X......X.......X
            // X.X........X...X
            // X..........X...X
            // X..........X...X
            // X......P...X...X
            // XXXXXXXXXXXXXXXX
            // X - Obstacles, P - Player, E - Enemies, F - First Node, S - Set of nodes to choose the closest one from
            Node startingNode = new Node(2, 8);
            List<Node> candidateNodes = new List<Node>();
            candidateNodes.Add(new Node(5, 8));
            candidateNodes.Add(new Node(6, 7));
            candidateNodes.Add(new Node(6, 9));

            Assert.AreEqual(new Node(5, 8), _pathFinderWithDiagonalMovement.ClosestNode(startingNode, candidateNodes));
        }

            //        [TestMethod]
            //        public void PathFinder_WhenMovementPointsIs1_CanFindPossibleMoveLocations()
            //        {
            //            // ......rrr.
            //            // ......rCr.
            //            // ......rrr.
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
            //            HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(7, 1), _mapData, 1);

            //            Assert.AreEqual(8, possibleMoveLocations.Count);
            //            AssertPossibleMoveLocationsFor1MovementPoint(possibleMoveLocations);
            //        }

            //        [TestMethod]
            //        public void PathFinder_WhenMovementPointsIs1_CanFindPossibleMoveLocationsAndIgnoresUnwalkableLocations()
            //        {
            //            // .rrr......
            //            // .rCr......
            //            // .rXr......
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
            //            _mapData[2, 2] = 1;
            //            HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(2, 1), _mapData, 1);

            //            Assert.AreEqual(7, possibleMoveLocations.Count);
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 0)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(2, 0)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 0)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 1)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 1)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 2)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 2)));
            //        }

            //        [TestMethod]
            //        public void PathFinder_WhenMovementPointsIs2_CanFindPossibleMoveLocations()
            //        {
            //            // .....rrrrr
            //            // .....rrCrr
            //            // .....rrrrr
            //            // .....rrrrr
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // ..........
            //            // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
            //            HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(7, 1), _mapData, 2);

            //            Assert.AreEqual(19, possibleMoveLocations.Count);
            //            AssertPossibleMoveLocationsFor1MovementPoint(possibleMoveLocations);
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 0)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 0)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 1)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 1)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 2)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 2)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 3)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(6, 3)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(7, 3)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(8, 3)));
            //            Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 3)));
            //        }

            //        private void AssertPossibleMoveLocationsFor1MovementPoint(HashSet<Node> moveLocations)
            //        {
            //            Assert.IsTrue(moveLocations.Contains(new Node(6, 0)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(7, 0)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(8, 0)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(6, 1)));
            //            Assert.IsFalse(moveLocations.Contains(new Node(7, 1)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(8, 1)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(6, 2)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(7, 2)));
            //            Assert.IsTrue(moveLocations.Contains(new Node(8, 2)));
            //        }
            //    }
    }
}
