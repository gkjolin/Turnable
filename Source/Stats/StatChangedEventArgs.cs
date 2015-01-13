using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Stats
{
    public class StatChangedEventArgs : EventArgs
    {
        public int OldValue { get; set; }
        public int NewValue { get; set; }
        public Stat Stat { get; set; }

        public StatChangedEventArgs(Stat stat, int oldValue, int newValue)
        {
            Stat = stat;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
