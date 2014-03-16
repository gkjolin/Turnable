using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;

namespace TurnItUp.Randomization
{
    public static class Prng
    {
        private static Random _random;

        static Prng()
        {
            _random = new Random();
        }

        public static int Next(int inclusiveMinimumValue, int exclusiveMaximumValue)
        {
            return _random.Next(inclusiveMinimumValue, exclusiveMaximumValue);
        }
    }
}
