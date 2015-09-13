using System;
using NUnit.Framework;
using System.Collections.Generic;
using Tests.Factories;
using Turnable.Pathfinding;
using Turnable.Api;
using Turnable.Locations;

namespace Tests.Pathfinding
{
    // LocationsFactory has an ASCII drawing of the level used in these tests.
    // The Background Layer and Object Layer are not relevant to Pathfinding.

    [TestFixture]
    public class PathfinderTests
    {
        private Node _node;
        private ILevel _level;
        private Pathfinder _pathfinderWithDiagonalMovement;
        private Pathfinder _pathfinderWithoutDiagonalMovement;

        [SetUp]
        public void Initialize()
        {
            _node = new Node(_level, 0, 0);
            _level = LocationsFactory.BuildLevel();
            _pathfinderWithDiagonalMovement = new Pathfinder(_level);
            _pathfinderWithoutDiagonalMovement = new Pathfinder(_level, false);
        }

        // *******************************************
        // Pathfinding with diagonal movement allowed
        // *******************************************
        public void Constructor_InitializesPathfinderAndAllowsDiagonalMovementByDefault()
        {
            Pathfinder pathfinder = new Pathfinder(_level);

            Assert.That(pathfinder.Level, Is.SameAs(_level));
            Assert.That(pathfinder.AllowDiagonalMovement, Is.True);
        }

        [Test]
        public void FindPath_WhenEndingNodeIsUnwalkable_ThrowsException()
        {
            Assert.That(() => _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 4, 14), new Node(_level, 6, 5)), Throws.InvalidOperationException);
        }

        [Test]
        public void FindPath_WhenEndingNodeIsUnreachable_ReturnsNull()
        {
            Assert.That(_pathfinderWithDiagonalMovement.FindPath(new Node(_level, 5, 5), new Node(_level, 13, 13)), Is.Null);
        }

        [Test]
        public void FindPath_WhereStartingAndEndingNodesAreOrthogonalAndNextToEachOther_FindsPath()
        {
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 1, 1), new Node(_level, 1, 2));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(2));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 1, 1)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 1, 2)));
        }

        [Test]
        public void FindPath_WhereStartingAndEndingNodesAreDiagonalAndNextToEachOther_FindsPath()
        {
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 6, 6), new Node(_level, 5, 5));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(2));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 6, 6)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 5, 5)));
        }

        [Test]
        public void FindPath_WhereStartingAndEndingNodesAreOrthogonallySeparatedWithNoCollidablesBetweenThem_FindsPath()
        {
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 1, 1), new Node(_level, 4, 1));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(4));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 1, 1)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 2, 1)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 3, 1)));
            Assert.That(path[3], Is.EqualTo(new Node(_level, 4, 1)));
        }

        [Test]
        public void FindPath_WhereStartingAndEndingNodesAreDiagonallySeparatedWithNoCollidablesBetweenThem_FindsPath()
        {
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 3, 5), new Node(_level, 5, 7));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(3));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 3, 5)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 4, 6)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 5, 7)));
        }

        [Test]
        public void FindPath_WhereEndingAndStartingNodesAreOrthogonallySeparatedAndHaveOneCollidableBetweenThem_FindsPath()
        {
            // Collidable at (6, 5)
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 6, 3), new Node(_level, 6, 7));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(5));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 6, 3)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 6, 4)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 5, 5)));
            Assert.That(path[3], Is.EqualTo(new Node(_level, 6, 6)));
            Assert.That(path[4], Is.EqualTo(new Node(_level, 6, 7)));
        }

        [Test]
        public void FindPath_WhereEndingAndStartingNodesAreDiagonallySeparatedAndHaveOneCollidableBetweenThem_FindsPath()
        {
            // Collidable at (6, 5)
            List<Node> path = _pathfinderWithDiagonalMovement.FindPath(new Node(_level, 4, 3), new Node(_level, 8, 7));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(6));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 4, 3)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 5, 4)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 6, 4)));
            Assert.That(path[3], Is.EqualTo(new Node(_level, 7, 5)));
            Assert.That(path[4], Is.EqualTo(new Node(_level, 8, 6)));
            Assert.That(path[5], Is.EqualTo(new Node(_level, 8, 7)));
        }

        // **********************************************
        // Pathfinding with diagonal movement NOT allowed
        // **********************************************
        [Test]
        public void FindPath_NoDiagonalMovementAllowedAndStartingAndEndingNodesAreDiagonalAndNextToEachOther_FindsPath()
        {
            List<Node> path = _pathfinderWithoutDiagonalMovement.FindPath(new Node(_level, 6, 9), new Node(_level, 5, 10));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(3));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 6, 9)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 5, 9)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 5, 10)));
        }

        [Test]
        public void FindPath_NoDiagonalMovementAllowedAndEndingAndStartingNodesAreOrthogonallySeparatedAndHaveOneObstacleBetweenThem_FindsPath()
        {
            List<Node> path = _pathfinderWithoutDiagonalMovement.FindPath(new Node(_level, 6, 3), new Node(_level, 6, 7));

            Assert.That(path, Is.Not.Null);
            Assert.That(path.Count, Is.EqualTo(7));
            Assert.That(path[0], Is.EqualTo(new Node(_level, 6, 3)));
            Assert.That(path[1], Is.EqualTo(new Node(_level, 6, 4)));
            Assert.That(path[2], Is.EqualTo(new Node(_level, 5, 4)));
            Assert.That(path[3], Is.EqualTo(new Node(_level, 5, 5)));
            Assert.That(path[4], Is.EqualTo(new Node(_level, 5, 6)));
            Assert.That(path[5], Is.EqualTo(new Node(_level, 5, 7)));
            Assert.That(path[6], Is.EqualTo(new Node(_level, 6, 7)));
        }
    }



    //    [Test]
    //    public void PathFinder_WhereDiagonalMovementIsNotAllowedAndThereAreBothObstaclesAndCharactersInTheWay_CanFindPath()
    //    {
    //         The sample level:
    //         XXXXXXXXXXXXXXXX
    //         X....1EE.......X
    //         X....ooooooX2..X
    //         X.......E.ooo..X
    //         X.E.X..........X
    //         X.....E....E...X
    //         X........X.....X
    //         X..........XXXXX
    //         X.F........X...X
    //         X..........X...X
    //         X......X.......X
    //         X.X........X...X
    //         X..........X...X
    //         X..........X...X
    //         X......P...X...X
    //         XXXXXXXXXXXXXXXX
    //         X - Obstacles, P - Player, E - Enemies, F - First Node, o - Expected path, 1 - Starting node, 2 - Ending node

    //        List<Node> path = _pathFinderWithoutDiagonalMovement.SeekPath(new Node(_level, 5, 14), new Node(_level, 12, 13));

    //        Assert.IsNotNull(path);
    //        Assert.That(11, path.Count);
    //        Assert.That(new Node(_level, 5, 14), path[0]);
    //        Assert.That(new Node(_level, 5, 13), path[1]);
    //        Assert.That(new Node(_level, 6, 13), path[2]);
    //        Assert.That(new Node(_level, 7, 13), path[3]);
    //        Assert.That(new Node(_level, 8, 13), path[4]);
    //        Assert.That(new Node(_level, 9, 13), path[5]);
    //        Assert.That(new Node(_level, 10, 13), path[6]);
    //        Assert.That(new Node(_level, 10, 12), path[7]);
    //        Assert.That(new Node(_level, 11, 12), path[8]);
    //        Assert.That(new Node(_level, 12, 12), path[9]);
    //        Assert.That(new Node(_level, 12, 13), path[10]);
    //    }

    //    [Test]
    //    public void PathFinder_WhenDiagonalMovementIsAllowed_CanDetermineMovementPointCostBetweenTwoNodes()
    //    {
    //        Node startingNode, endingNode;

    //         Starting and Ending location are the same
    //        startingNode = new Node(_level, 0, 0);
    //        endingNode = new Node(_level, 0, 0);
    //        Assert.That(0, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are in the same line vertically or horizontally
    //        endingNode = new Node(_level, 0, 5);
    //        Assert.That(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 3, 0);
    //        Assert.That(3, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are exactly diagonal to each other
    //        endingNode = new Node(_level, 5, 5);
    //        Assert.That(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 3, 3);
    //        Assert.That(3, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are not exactly diagonal or in the same line horizontally or vertically
    //        endingNode = new Node(_level, 3, 5);
    //        Assert.That(5, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 6, 1);
    //        Assert.That(6, _pathFinderWithDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //    }

    //    [Test]
    //    public void PathFinder_WhenDiagonalMovementIsNotAllowed_CanDetermineMovementPointCostBetweenTwoNodes()
    //    {
    //        Node startingNode, endingNode;

    //         Starting and Ending location are the same
    //        startingNode = new Node(_level, 0, 0);
    //        endingNode = new Node(_level, 0, 0);
    //        Assert.That(0, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are in the same line vertically or horizontally
    //        endingNode = new Node(_level, 0, 5);
    //        Assert.That(5, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 3, 0);
    //        Assert.That(3, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are exactly diagonal to each other
    //        endingNode = new Node(_level, 5, 5);
    //        Assert.That(10, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 3, 3);
    //        Assert.That(6, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));

    //         Starting and Ending location are not exactly diagonal or in the same line horizontally or vertically
    //        endingNode = new Node(_level, 3, 5);
    //        Assert.That(8, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //        endingNode = new Node(_level, 6, 1);
    //        Assert.That(7, _pathFinderWithoutDiagonalMovement.MovementPointCost(startingNode, endingNode));
    //    }

    //    [Test]
    //    public void PathFinder_WhenDiagonalMovementIsAllowed_CanFindClosestNodeToAnotherNodeFromASetOfNodes()
    //    {
    //         The sample level:
    //         XXXXXXXXXXXXXXXX
    //         X....EEE.......X
    //         X..........X...X
    //         X.......E......X
    //         X.E.X..........X
    //         X.....E....E...X
    //         X........X.....X
    //         X.....S....XXXXX
    //         X.F..S.....X...X
    //         X.....S....X...X
    //         X......X.......X
    //         X.X........X...X
    //         X..........X...X
    //         X..........X...X
    //         X......P...X...X
    //         XXXXXXXXXXXXXXXX
    //         X - Obstacles, P - Player, E - Enemies, F - First Node, S - Set of nodes to choose the closest one from
    //        Node startingNode = new Node(_level, 2, 8);
    //        List<Node> candidateNodes = new List<Node>();
    //        candidateNodes.Add(new Node(_level, 5, 8));
    //        candidateNodes.Add(new Node(_level, 6, 7));
    //        candidateNodes.Add(new Node(_level, 6, 9));

    //        Assert.That(new Node(_level, 5, 8), _pathFinderWithDiagonalMovement.GetClosestNode(startingNode, candidateNodes));
    //    }

    //    [Test]
    //    public void PathFinder_WhenDiagonalMovementIsNotAllowed_CanFindClosestNodeToAnotherNodeFromASetOfNodes()
    //    {
    //         The sample level:
    //         XXXXXXXXXXXXXXXX
    //         X....EEE.......X
    //         X..........X...X
    //         X.......E......X
    //         X.E.X..........X
    //         X.....E....E...X
    //         X........X.....X
    //         X.....S....XXXXX
    //         X.F..S.....X...X
    //         X.....S....X...X
    //         X......X.......X
    //         X.X........X...X
    //         X..........X...X
    //         X..........X...X
    //         X......P...X...X
    //         XXXXXXXXXXXXXXXX
    //         X - Obstacles, P - Player, E - Enemies, F - First Node, S - Set of nodes to choose the closest one from
    //        Node startingNode = new Node(_level, 2, 8);
    //        List<Node> candidateNodes = new List<Node>();
    //        candidateNodes.Add(new Node(_level, 5, 8));
    //        candidateNodes.Add(new Node(_level, 6, 7));
    //        candidateNodes.Add(new Node(_level, 6, 9));

    //        Assert.That(new Node(_level, 5, 8), _pathFinderWithoutDiagonalMovement.GetClosestNode(startingNode, candidateNodes));
    //    }

    //    [Test]
    //    public void PathFinder_WhenDiagonalMovementIsNotAllowed_CanFindClosestNodeToAnotherPositionFromASetOfPositions()
    //    {
    //         The sample level:
    //         XXXXXXXXXXXXXXXX
    //         X....EEE.......X
    //         X..........X...X
    //         X.......E......X
    //         X.E.X..........X
    //         X.....E....E...X
    //         X........X.....X
    //         X.....S....XXXXX
    //         X.F..S.....X...X
    //         X.....S....X...X
    //         X......X.......X
    //         X.X........X...X
    //         X..........X...X
    //         X..........X...X
    //         X......P...X...X
    //         XXXXXXXXXXXXXXXX
    //         X - Obstacles, P - Player, E - Enemies, F - First Node, S - Set of nodes to choose the closest one from
    //        Position startingPosition = new Position(2, 8);
    //        HashSet<Position> candidatePositions = new HashSet<Position>();
    //        candidatePositions.Add(new Position(5, 8));
    //        candidatePositions.Add(new Position(6, 7));
    //        candidatePositions.Add(new Position(6, 9));

    //        Assert.That(new Node(_level, 5, 8), _pathFinderWithoutDiagonalMovement.GetClosestNode(startingPosition, candidatePositions));
    //    }

    //            [Test]
    //            public void PathFinder_WhenMovementPointsIs1_CanFindPossibleMoveLocations()
    //            {
    //                // ......rrr.
    //                // ......rCr.
    //                // ......rrr.
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
    //                HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(7, 1), _mapData, 1);

    //                Assert.That(8, possibleMoveLocations.Count);
    //                AssertPossibleMoveLocationsFor1MovementPoint(possibleMoveLocations);
    //            }

    //            [Test]
    //            public void PathFinder_WhenMovementPointsIs1_CanFindPossibleMoveLocationsAndIgnoresUnwalkableLocations()
    //            {
    //                // .rrr......
    //                // .rCr......
    //                // .rXr......
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
    //                _mapData[2, 2] = 1;
    //                HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(2, 1), _mapData, 1);

    //                Assert.That(7, possibleMoveLocations.Count);
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 0)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(2, 0)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 0)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 1)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 1)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(1, 2)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(3, 2)));
    //            }

    //            [Test]
    //            public void PathFinder_WhenMovementPointsIs2_CanFindPossibleMoveLocations()
    //            {
    //                // .....rrrrr
    //                // .....rrCrr
    //                // .....rrrrr
    //                // .....rrrrr
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // ..........
    //                // X - Obstacles, C - Character, r - Possible move locations (1 Movement Point)
    //                HashSet<Node> possibleMoveLocations = PathFinder.GetPossibleMoveLocations(new Node(7, 1), _mapData, 2);

    //                Assert.That(19, possibleMoveLocations.Count);
    //                AssertPossibleMoveLocationsFor1MovementPoint(possibleMoveLocations);
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 0)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 0)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 1)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 1)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 2)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 2)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(5, 3)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(6, 3)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(7, 3)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(8, 3)));
    //                Assert.IsTrue(possibleMoveLocations.Contains(new Node(9, 3)));
    //            }

    //            private void AssertPossibleMoveLocationsFor1MovementPoint(HashSet<Node> moveLocations)
    //            {
    //                Assert.IsTrue(moveLocations.Contains(new Node(6, 0)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(7, 0)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(8, 0)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(6, 1)));
    //                Assert.IsFalse(moveLocations.Contains(new Node(7, 1)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(8, 1)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(6, 2)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(7, 2)));
    //                Assert.IsTrue(moveLocations.Contains(new Node(8, 2)));
    //            }
    //        }
    //}
}
