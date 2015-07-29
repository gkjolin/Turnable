using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.Tiled;
using Turnable.Utilities;

namespace Turnable.Api
{
    public interface ILayer
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        void SetTile(Position position, uint globalId);
        void RemoveTile(Position position);
        void MoveTile(Position currentPosition, Position newPosition);
        void SwapTile(Position position1, Position position2);
        void Fill(uint globalId);
        void Fill(Rectangle bounds, uint globalId);

        // -----------------
        // Private interface
        // -----------------

        // Properties
        string Name { get; set; }
        double Opacity { get; set; }
        bool IsVisible { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        TileList Tiles { get; set; }
        PropertyDictionary Properties { get; set; }
    }
}
