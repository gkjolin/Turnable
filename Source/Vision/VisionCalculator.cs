using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;

namespace Turnable.Vision
{
    public class VisionCalculator : IVisionCalculator
    {
        public ILevel Level { get; set; }

        public VisionCalculator(ILevel level)
        {
            Level = level;
        }
    }
}
