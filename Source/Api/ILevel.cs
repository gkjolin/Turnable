using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Components;
using Turnable.Locations;

namespace Turnable.Api
{
    public interface ILevel
    {
        // ----------------
        // Public interface
        // ----------------
        bool IsCollision(Position position);

        // ----------
        // Properties
        // ----------
        IMap Map { get; set; }
        IPathfinder Pathfinder { get; set; }
        Level.SpecialLayersCollection SpecialLayers { get; set; }

        // -----------------
        // Private interface
        // -----------------
        void InitializeSpecialLayers();
    }
}
