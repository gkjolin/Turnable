using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.Goals;
using Tests.SupportingClasses;
using Moq;
using Entropy;
using TurnItUp.Skills;

namespace Tests.AI.Goals
{
    [TestClass]
    public class SkillTreeTests
    {
        private SkillTree _skillTree;

        [TestInitialize]
        public void Initialize()
        {
            _skillTree = new SkillTree();
        }

        [TestMethod]
        public void SkillTree_Construction_IsSuccessful()
        {
            SkillTree skillTree = new SkillTree();

            Assert.IsNotNull(skillTree.Subtrees);
        }

        [TestMethod]
        public void SkillTree_IsAnEntropyComponent()
        {
            Assert.IsInstanceOfType(_skillTree, typeof(IComponent));
        }
    }
}
