using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Locations;

namespace Turnable.Inspectors
{
    public class LevelInspector
    {
        public ILevel Level { get; set; }

        public LevelInspector(ILevel level)
        {
            Level = level;
        }
    }
}
