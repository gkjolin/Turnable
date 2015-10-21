using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Api
{
    public interface IViewport
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        bool IsMapLocationValid();
        void Move(Direction direction);
        void CenterAt(Position center);

        // Properties
        ILevel Level { get; set; }
        Position MapLocation { get; }
        int Width { get; }
        int Height { get; }

        // -----------------
        // Private interface
        // -----------------
    }
}
