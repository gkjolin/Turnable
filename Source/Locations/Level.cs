using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Locations
{
    public class Level : ILevel
    {
        public IMap Map { get; private set; }

        public Level()
        {
        }
    }
}
