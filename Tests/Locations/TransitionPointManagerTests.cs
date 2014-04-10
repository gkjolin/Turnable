using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Moq;
using TurnItUp.Components;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Tmx;
using System.Tuples;
using Tests.Factories;

namespace Tests.Locations
{
    [TestClass]
    public class TransitionPointManagerTests
    {
        private Level _level;
        private TransitionPointManager _transitionPointManager;

        [TestInitialize]
        public void Initialize()
        {
            _level = LocationsFactory.BuildLevel("../../Fixtures/HubExample.tmx");
            _transitionPointManager = new TransitionPointManager(_level);
        }

        [TestMethod]
        public void TransitionPointManager_Construction_IsSuccessful()
        {
            TransitionPointManager transitionPointManager = new TransitionPointManager(_level);

            Assert.AreEqual(_level, transitionPointManager.Level);
        }

        [TestMethod]
        public void TransitionPointManager_WhenConstructedWithALevelWithNoEntranceAndAFewExits_SetsTheExits()
        {
            Assert.IsNull(_transitionPointManager.Entrance);
            Assert.AreEqual(4, _transitionPointManager.Exits.Count);
        }
    }
}
