using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Locations;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface IVisionCalculator
    {
        // ----------------
        // Public interface
        // ----------------

        //Methods
        List<Position> CalculateVisiblePositions(int originX, int originY, int visualRange);

        // Properties
        ILevel Level { get; set; }

        // -----------------
        // Private interface
        // -----------------

        // Methods
        double CalculateSlope(double x1, double y1, double x2, double y2, bool inverse = false);
        int CalculateVisibleDistance(int x1, int y1, int x2, int y2);
    }
}
