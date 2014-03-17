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
