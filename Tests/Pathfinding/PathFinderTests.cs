using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Pathfinding;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Pathfinding
{
    // The sample board:
    // XXXXXXXXXXXXXXX
    // X.............X
    // X..........X..X
    // X.............X
    // X...X.........X
    // X.............X
    // X........X....X
    // X..........XXXX
    // X..........X..X
    // X..........X..X
    // X......X...X..X
    // X.X........X..X
    // X..........X..X
    // X..........X..X
    // XXXXXXXXXXXXXXX

    [TestClass]
    public class PathFinderTests
    {
        private Node _node;
        private Board _board;

        [TestInitialize]
        public void Initialize()
        {
            _node = new Node(0, 0);
            _board = LocationsFactory.BuildBoard();
        }

        //[TestMethod]
        //public void PathFinder_WhereEndingAndStartingPointsAreOrthogonalAndNextToEachOther_CanFindPath()
        //{
        //    List<Node> path = PathFinder.SeekPath(new Node(1, 1), new Node(1, 2), _board);

        //    Assert.IsNotNull(path);
        //    Assert.AreEqual(2, path.Count);
        //    Assert.AreEqual(new Node(1, 1), path[0]);
        //    Assert.AreEqual(new Node(1, 2), path[1]);
        //}

        //[TestMethod]
        //public void PathFinder_WhereEndingAndStartingPointsAreDiagonalAndNextToEachOther_CanFindPath()
        //{
        //    List<Node> path = PathFinder.SeekPath(new Node(5, 5), new Node(4, 4), _board);

        //    Assert.IsNotNull(path);
        //    Assert.AreEqual(2, path.Count);
        //    Assert.AreEqual(new Node(5, 5), path[0]);
        //    Assert.AreEqual(new Node(4, 4), path[1]);
        //}

        //[TestMethod]
        //public void PathFinder_WhereEndingAndStartingPointsAreOrthogonallySeparatedAndHaveNoObstaclesBetweenThem_CanFindPath()
        //{
        //    List<Node> path = PathFinder.SeekPath(new Node(1, 1), new Node(5, 1), _board);

        //    Assert.IsNotNull(path);
        //    Assert.AreEqual(5, path.Count);
        //    Assert.AreEqual(new Node(1, 1), path[0]);
        //    Assert.AreEqual(new Node(2, 1), path[1]);
        //    Assert.AreEqual(new Node(3, 1), path[2]);
        //    Assert.AreEqual(new Node(4, 1), path[3]);
        //    Assert.AreEqual(new Node(5, 1), path[4]);
        //}

        //[TestMethod]
        //public void PathFinder_WhereEndingAndStartingPointsAreDiagonallySeparatedAndHaveNoObstaclesBetweenThem_CanFindPath()
        //{
        //    List<Node> path = PathFinder.SeekPath(new Node(3, 3), new Node(5, 5), _board);

        //    Assert.IsNotNull(path);
        //    Assert.AreEqual(3, path.Count);
        //    Assert.AreEqual(new Node(3, 3), path[0]);
        //    Assert.AreEqual(new Node(4, 4), path[1]);
        //    Assert.AreEqual(new Node(5, 5), path[2]);
        //}

        //[TestMethod]
        //public void PathFinder_WhereEndingAndStartingPointsAreOrthogonalSeparatedAndHaveOneObstacle_CanFindPath()
        //{
        //    // XXXXXXXXXXXXXXX
        //    // X.............X
        //    // X..........X..X
        //    // X.............X
        //    // X...X.........X
        //    // X.............X
        //    // X........X....X
        //    // X..........XXXX
        //    // X..........X..X
        //    // X..........X..X
        //    // X......X...X..X
        //    // X.X........X..X
        //    // X..........X..X
        //    // X..........X..X
        //    // XXXXXXXXXXXXXXX
        //    //// X - Obstacles, S - Starting Point, E - Ending Point, o - Expected path
        //    //List<Node> path = PathFinder.SeekPath(new Node(2, 1), new Node(2, 5), _board);

        //    //Assert.IsNotNull(path);
        //    //Assert.AreEqual(5, path.Count);
        //    //Assert.AreEqual(new Node(2, 1), path[0]);
        //    //Assert.AreEqual(new Node(3, 2), path[1]);
        //    //Assert.AreEqual(new Node(2, 3), path[2]);
        //    //Assert.AreEqual(new Node(2, 4), path[3]);
        //    //Assert.AreEqual(new Node(2, 5), path[4]);
        //}

            //        [TestMethod]
            //        public void PathFinder_CanDetermineMovementPointCostBetweenTwoNodes()
            //        {
            //            Node startingNode, endingNode;

            //            // Starting and Ending location are the same
            //            startingNode = new Node(0, 0);
            //            endingNode = new Node(0, 0);
            //            Assert.AreEqual(0, PathFinder.MovementPointCost(startingNode, endingNode));

            //            // Starting and Ending location are in the same line vertically or horizontally
            //            endingNode = new Node(0, 5);
            //            Assert.AreEqual(5, PathFinder.MovementPointCost(startingNode, endingNode));
            //            endingNode = new Node(3, 0);
            //            Assert.AreEqual(3, PathFinder.MovementPointCost(startingNode, endingNode));

            //            // Starting and Ending location are exactly diagonal to each other
            //            endingNode = new Node(5, 5);
            //            Assert.AreEqual(5, PathFinder.MovementPointCost(startingNode, endingNode));
            //            endingNode = new Node(3, 3);
            //            Assert.AreEqual(3, PathFinder.MovementPointCost(startingNode, endingNode));

            //            // Starting and Ending location are not exactly diagonal or in the same line horizontally or vertically
            //            endingNode = new Node(3, 5);
            //            Assert.AreEqual(5, PathFinder.MovementPointCost(startingNode, endingNode));
            //            endingNode = new Node(6, 1);
            //            Assert.AreEqual(6, PathFinder.MovementPointCost(startingNode, endingNode));
            //        }

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
