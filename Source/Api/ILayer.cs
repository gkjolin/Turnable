using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface ILayer
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        void SetTile(Position position, uint globalId);
        void EraseTile(Position position);
        void MoveTile(Position currentPosition, Position newPosition);
        void SwapTile(Position position1, Position position2);

        // -----------------
        // Private interface
        // -----------------

        // Properties
        string Name { get; set; }
        double Opacity { get; private set; }
        bool IsVisible { get; private set; }
        int Width { get; private set; }
        int Height { get; private set; }
        TileList Tiles { get; private set; }
        PropertyDictionary Properties { get; private set; }
    }
}
