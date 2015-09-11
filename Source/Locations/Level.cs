using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Api;
using Turnable.Characters;
using Turnable.Components;
using Turnable.Models;
using Turnable.Tiled;
using Turnable.Vision;

namespace Turnable.Locations
{
    public class Level : ILevel
    {
        private IMap _map;

        public IPathfinder Pathfinder { get; set; }
        public SpecialLayersCollection SpecialLayers { get; set; }
        public ICharacterManager CharacterManager { get; set; }
        public IModelManager ModelManager { get; set; }
        public IViewport Viewport { get; set; }
        public IVisionCalculator VisionCalculator { get; set; }

        public Level()
        {
            Map = new Map();
            SpecialLayers = new SpecialLayersCollection();
        }

        public Level(LevelSetupParameters parameters) : this()
        {
            Map = new Map(parameters.TmxFullFilePath);
        }

        public virtual IMap Map { 
            get
            {
                return _map;
            }
            set
            {
                _map = value;
                InitializeSpecialLayers();
            }
        }

        public virtual void InitializeSpecialLayers()
        {
            foreach (Layer layer in Map.Layers)
            {
                foreach (SpecialLayer specialLayer in Enum.GetValues(typeof(SpecialLayer)).Cast<SpecialLayer>())
                {
                    string key = Level.SpecialLayerPropertyKey(specialLayer);

                    if (layer.Properties[key] != null)
                    {
                        SpecialLayers[specialLayer] = layer;
                    }
                }
            }
        }

        public bool IsCollidable(Position position)
        {
            Layer collisionLayer = SpecialLayers[SpecialLayer.Collision];

            // No collision layer exists. 
            if (collisionLayer == null)
            {
                return false;
            }

            return (collisionLayer.Tiles[position] != null);
        }

        public static string SpecialLayerPropertyKey(SpecialLayer specialLayer)
        {
            return "Is" + specialLayer.ToString() + "Layer";
        }

        public class SpecialLayersCollection : Dictionary<SpecialLayer, Layer>
        {
            public new Layer this[SpecialLayer index]
            {
                get
                {
                    Layer returnValue;

                    base.TryGetValue(index, out returnValue);

                    return returnValue;
                }

                set
                {
                    string key = Level.SpecialLayerPropertyKey(index);

                    if (ContainsKey(index))
                    { 
                        throw new ArgumentException(); 
                    }

                    value.Properties[key] = "true";
                    base[index] = value;
                }
            }
        }

        // -------------
        // SetUp methods
        // -------------
        public void SetUpViewport()
        {
            Viewport = new Viewport(this);
        }

        public void SetUpViewport(int width, int height)
        {
            Viewport = new Viewport(this, width, height);
        }

        public void SetUpViewport(int mapOriginX, int mapOriginY, int width, int height)
        {
            Viewport = new Viewport(this, mapOriginX, mapOriginY, width, height);
        }

        public void SetUpVisionCalculator()
        {
            VisionCalculator = new VisionCalculator(this);
        }

        public void SetUpCharacterManager()
        {
            CharacterManager = new CharacterManager(this);
        }

        public void SetUpModelManager()
        {
            ModelManager = new ModelManager(this);
        }

        // The reason why ILevel has this method definition instead of Map is because the concept of a special layer is a Turnable Framework concept and
        // not a Tiled concept.
        public void SetLayer(string name, int width, int height, SpecialLayer specialLayer)
        {
            Layer layer = new Layer(name, width, height);
            SpecialLayers[specialLayer] = layer;
            Map.Layers.Add(layer);
        }
    }
}
