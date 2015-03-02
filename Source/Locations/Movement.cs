using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.Locations
{
    public class Movement
    {
        public MovementStatus Status { get; set; }
        public List<Position> Path { get; set; }
    }
}