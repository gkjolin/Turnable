using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using Turnable.Api;
using Turnable.Components;
using Turnable.Tiled;

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

        public Level()
        {
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
                foreach (Level.SpecialLayer specialLayer in Enum.GetValues(typeof(Level.SpecialLayer)).Cast<Level.SpecialLayer>())
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

            return (collisionLayer.Tiles.ContainsKey(new Tuple<int, int>(position.X, position.Y)));
        }

        public enum SpecialLayer
        {
            Background,
            Collision,
            Object,
            Character
        }

        public static string SpecialLayerPropertyKey(SpecialLayer specialLayer)
        {
            return "Is" + specialLayer.ToString() + "Layer";
        }

        public class SpecialLayersCollection : Dictionary<Level.SpecialLayer, Layer>
        {
            public new Layer this[Level.SpecialLayer index]
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
    }
}
