using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Stats
{
    public class Stat : IStat
    {
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public string Name { get; set; }
        private int _value;
        private int _initialValue;

        public void Reset()
        {
            Value = _initialValue;
        }

        internal Stat(string name, int initialValue, int maximumValue, int minimumValue)
        {
            _initialValue = initialValue;
            // NOTE: The MinimumValue and MaximumValue MUST be set before Value since Value clamps its values to MaximumValue and MonimumValue
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            Name = name;
            Value = initialValue;
        }

        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;

                if (value < MinimumValue)
                {
                    _value = MinimumValue;
                }
                if (value > MaximumValue)
                {
                    _value = MaximumValue;
                } 
            }
        }
    }
}
