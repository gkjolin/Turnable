namespace Turnable.Tiled.Api
{
    public interface ILayer
    {
        // Methods

        // Properties
        string Name { get; set; }
        double? Opacity { get; set; }
        bool? IsVisible { get; set; }

        // Events
    }
}
