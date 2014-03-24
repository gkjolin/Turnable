using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Randomization
{
    public static class RandomSelector
    {
        public static T Next<T>(IList<T> list)
        {
            int randomIndex = Prng.Next(0, list.Count);

            return list[randomIndex];
        }

        public static List<T> Next<T>(IList<T> list, int neededCount)
        {
            // http://stackoverflow.com/questions/48087/select-a-random-n-elements-from-listt-in-c-sharp
            List<T> returnValue = new List<T>();

            int index = 0;

            foreach (T item in list)
            {
                if (returnValue.Count == neededCount)
                {
                    return returnValue;
                }

                double selectionProbablity = (double)(neededCount - returnValue.Count) / (double)(list.Count - index);
                if (Prng.NextDouble() < selectionProbablity)
                {
                    returnValue.Add(item);
                }

                index++;
            }

            return returnValue;
        }

        public static TValue Next<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        {
            int randomIndex = Prng.Next(0, dictionary.Count);

            return dictionary[dictionary.Keys.ElementAt<TKey>(randomIndex)];
        }

        public static TReturn Next<TKey, TValue, TReturn>(IDictionary<TKey, TValue> dictionary)
        {
            int randomIndex = Prng.Next(0, dictionary.Count);

            return Next<TReturn>((IList<TReturn>)dictionary[dictionary.Keys.ElementAt<TKey>(randomIndex)]);
        }

    }
}
