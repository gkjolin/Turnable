using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Components;

namespace Tests.Components
{
    [TestClass]
    public class PositionTests
    {
        [TestMethod]
        public void Position_Construction_IsSuccessful()
        {
            Position position = new Position(1, 2);

            Assert.AreEqual(1, position.X);
            Assert.AreEqual(2, position.Y);
        }
    }
}
