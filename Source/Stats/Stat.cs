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
        public bool IsHealth { get; set; }

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
                    OnChanged(new StatChangedEventArgs(null, this));
                    return;
                }
                if (value > MaximumValue)
                {
                    _value = MaximumValue;
                    OnChanged(new StatChangedEventArgs(null, this));
                    return;
                }

                _value = value;
                OnChanged(new StatChangedEventArgs(null, this));
            }
        }

        internal Stat(string name, int startingValue, int minimumValue = 0, int maximumValue = 100, bool isHealth = false)
        {
            MinimumValue = minimumValue;
            MaximumValue = maximumValue;
            Name = name;
            Value = startingValue;
            _initialValue = startingValue;
            IsHealth = isHealth;
        }

        public void Reset()
        {
            Value = _initialValue;
        }

        public virtual event EventHandler<StatChangedEventArgs> Changed;

        protected virtual void OnChanged(StatChangedEventArgs e)
        {
            if (Changed != null)
            {
                Changed(this, e);
            }
        }
    }
}
