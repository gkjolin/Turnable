using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Randomization;

namespace Tests.Randomization
{
    [TestClass]
    public class RandomSelectorTests
    {
        [TestMethod]
        public void RandomSelector_RandomlySelectingFromAList_IsSuccessful()
        {
            List<int> list = new List<int>();

            list.Add(10);
            list.Add(5);
            list.Add(24);
            list.Add(57);

            int randomItemFromList = RandomSelector.Next<int>(list);

            Assert.IsTrue(list.Contains(randomItemFromList));
        }

        [TestMethod]
        public void RandomSelector_RandomlySelectingFromADictionary_IsSuccessful()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            dictionary.Add("Key1", "Value1");
            dictionary.Add("Key2", "Value2");
            dictionary.Add("Key3", "Value3");
            dictionary.Add("Key4", "Value4");

            string randomItemFromDictionary = RandomSelector.Next<string, string>(dictionary);

            Assert.IsTrue(dictionary.ContainsValue(randomItemFromDictionary));
        }

        [TestMethod]
        public void RandomSelector_RandomlySelectingFromADictionaryWhoseValuesAreAList_ReturnsARandomItemFromARandomListInTheDictionary()
        {
            Dictionary<string, List<int>> dictionary = new Dictionary<string, List<int>>();
            List<int> list = new List<int>();

            list.Add(5);
            list.Add(15);
            list.Add(47);

            dictionary.Add("Key1", list);
            dictionary.Add("Key2", list);
            dictionary.Add("Key3", list);
            dictionary.Add("Key4", list);

            string randomItemFromDictionary = RandomSelector.Next<string, List<int>, int>(dictionary);

        }
    }
}
