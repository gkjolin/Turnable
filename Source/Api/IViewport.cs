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
        bool IsMapOriginValid();
        void Move(Direction direction);
        void CenterAt(Position center);

        // Properties
        ILevel Level { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        Position MapOrigin { get; set; }

        // -----------------
        // Private interface
        // -----------------
    }
}
