using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Tuples;
using TurnItUp.Components;
using TurnItUp.Locations;
using TurnItUp.Tmx;

namespace TurnItUp.Interfaces
{
    public interface ILevelFactory
    {
        void Initialize(ILevel level, LevelInitializationParams initializationParams);
        void Randomize(ILevel level);
    }
}
