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
    public class LevelSetupParameters : ISetupParameters
    {
        public string TmxFullFilePath { get; set; }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }

        public bool IsSufficient()
        {
            throw new NotImplementedException();
        }
    }
}
