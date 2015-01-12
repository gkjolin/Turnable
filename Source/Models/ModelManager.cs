using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Tiled;

namespace Turnable.Models
{
    public class ModelManager : IModelManager
    {
        public ILevel Level { get; set; }
        public Dictionary<string, SpecialTile> Models { get; set; }

        public ModelManager(ILevel level)
        {
            Level = level;
            Models = new Dictionary<string, SpecialTile>();

            var specialTilesWithModelProperty = (from t in Level.Map.Tilesets select (from st in t.SpecialTiles.Values where st.Properties["Model"] != null select st));
            foreach (SpecialTile specialTile in specialTilesWithModelProperty)
            {
                Models.Add(specialTile.Properties["Model"], specialTile);
            }
        }
    }
}
