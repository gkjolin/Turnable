using System;
using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Tmx;
using System.IO;
using Tests.Factories;

namespace Tests.Tmx
{
    [TestClass]
    public class ReferenceTileTests
    {
        [TestMethod]
        public void ReferenceTile_Construction_IsSuccessful()
        {
            ReferenceTile referenceTile = new ReferenceTile(1);

            Assert.AreEqual(1, referenceTile.Id);
        }

        [TestMethod]
        public void ReferenceTile_ConstructionUsingReferenceTileDataWithProperties_IsSuccessful()
        {
            ReferenceTile referenceTile = new ReferenceTile(TmxFactory.BuildReferenceTileXElementWithProperties());

            Assert.AreEqual(0, referenceTile.Id);
            // Properties have been loaded?
            Assert.IsNotNull(referenceTile.Properties);
            Assert.AreEqual(1, referenceTile.Properties.Count);
            Assert.AreEqual("true", referenceTile.Properties["IsPlayer"]);
        }
    }
}
