using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.LevelGenerators
{
    public class Corridor
    {
        public List<Segment> Segments { get; set; }

        public Corridor(List<Segment> segments)
        {
            Segments = segments;
        }
    }
}
