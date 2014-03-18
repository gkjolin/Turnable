using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TurnItUp.Randomization;
using System.Linq;

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
        public void RandomSelector_RandomlySelectingASubsetOfItemsFromAList_IsSuccessful()
        {
            List<int> list = new List<int>();

            list.Add(10);
            list.Add(5);
            list.Add(24);
            list.Add(57);
            list.Add(19);
            list.Add(1010);

            List<int> randomSubsetFromList = RandomSelector.Next<int>(list, 3);

            Assert.AreEqual(3, randomSubsetFromList.Count);
            foreach (int item in randomSubsetFromList)
            {
                Assert.IsTrue(list.Contains(item));
            }
            Assert.IsTrue(randomSubsetFromList.Distinct().Count() == randomSubsetFromList.Count());
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

            int randomItemFromDictionary = RandomSelector.Next<string, List<int>, int>(dictionary);

            Assert.IsTrue(list.Contains(randomItemFromDictionary));
        }
    }
}
