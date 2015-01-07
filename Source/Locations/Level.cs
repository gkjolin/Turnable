using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Locations
{
    public class Level : ILevel
    {
        public IMap Map { get; set; }

        public Level()
        {
        }

        public Level(IMap map)
            : this()
        {
            Map = map;
        }
    }
}
