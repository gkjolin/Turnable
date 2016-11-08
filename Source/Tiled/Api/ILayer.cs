using Turnable.Utilities;
using Turnable.Utilities.Api;

namespace Turnable.Tiled.Api
{
    public interface ILayer
    {
        // Methods
        bool IsTilePresent(ICoordinates coordinates);
        Tile GetTile(ICoordinates coordinates);
        void SetTile(ICoordinates coordinates, uint globalId);
        void RemoveTile(ICoordinates coordinates);
        void MoveTile(ICoordinates origin, ICoordinates destination);
        void SwapTile(ICoordinates firstCoordinates, ICoordinates secondCoordinates);
/*
        void Fill(uint globalId);
        void Fill(Rectangle bounds, uint globalId);
*/

        // Properties
        string Name { get; set; }
        double? Opacity { get; set; }
        bool? IsVisible { get; set; }
        int? Width { get; set; }
        int? Height { get; set; }
        int TileCount { get; }

        // Events
    }
}
