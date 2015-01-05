using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface IMap
    {
        int TileWidth { get; set; }
        int TileHeight { get; set; }
        int Width { get; set; }
        int Height { get; set; }
        RenderOrder RenderOrder { get; set; }
        Orientation Orientation { get; set; }
        string Version { get; set; }
        ElementList<Layer> Layers { get; set; }

        void SetSpecialLayer(Layer layer, Map.SpecialLayer value);
        void InitializeSpecialLayer(Map.SpecialLayer value);
    }
}