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

        public void Reset()
        {
            throw new NotImplementedException();
        }

        internal Stat(string name, int initialValue, int maximumValue, int minimumValue)
        {
            Name = name;
            Value = initialValue;
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
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
