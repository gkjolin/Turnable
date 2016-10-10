namespace Turnable.Tiled.Api
{
    public interface IMap
    {
        // Methods

        // Properties
        string Version { get; set; }
        Orientation Orientation { get; set; }
        RenderOrder RenderOrder { get; set; }
        int? Width { get; set; }
        int? Height { get; set; }
        int? TileWidth { get; set; }
        int? TileHeight { get; set; }

        // Events
    }
}
