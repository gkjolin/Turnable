using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Stats
{
    public class Stat
    {
        private int _initialValue;
        private int _value;
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public string Name { get; set; }

        public int Value
        {
            get
            {
                return _value;
            }

            set
            {
                if (value < MinimumValue)
                {
                    _value = MinimumValue;
                    return;
                }
                if (value > MaximumValue)
                {
                    _value = MaximumValue;
                    return;
                }

                _value = value;
            }
        }

        internal Stat(string name, int startingValue, int minimumValue = 0, int maximumValue = 100)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            Name = name;
            Value = startingValue;
            _initialValue = startingValue;
        }

        public void Reset()
        {
            Value = _initialValue;
        }
    }
}
