using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Tmx;

namespace TurnItUp.Interfaces
{
    public interface ILevelRandomizer
    {
        TileList BuildRandomTileList(ILevel level, Layer targetLayer, int count);
        void Randomize(ILevel level, string layerName, int count);
        void Randomize(ILevel level, string layerName, int inclusiveMinimumValue, int exclusiveMaximumValue);
    }
}
