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

        public static List<T> Next<T>(IList<T> list, int count)
        {

            // http://stackoverflow.com/questions/48087/select-a-random-n-elements-from-listt-in-c-sharp
            List<T> returnValue = new List<T>();

            foreach (T item in list)
            {
                if (Prng.NextDouble() <= ((double)count / (double)returnValue.Count))
                {
                    returnValue.Add(item);
                    if (returnValue.Count == count)
                    {
                        return returnValue;
                    } 
                }
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
