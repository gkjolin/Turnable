using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Components;
using Entropy;
using TurnItUp.Locations;
using Tests.Factories;

namespace Tests.Components
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void Model_Construction_IsSuccessful()
        {
            Model model = new Model("Skeleton");

            Assert.AreEqual("Skeleton", model.Name);
        }

        [TestMethod]
        public void Model_IsAnEntropyComponent()
        {
            Model model = new Model("Skeleton");

            Assert.IsInstanceOfType(model, typeof(IComponent));
        }
    }
}
