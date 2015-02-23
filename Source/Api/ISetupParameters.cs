using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Tiled;

namespace Turnable.Api
{
    public interface ISetupParameters
    {
        // ----------------
        // Public interface
        // ----------------

        // Methods
        bool IsValid();
        bool IsSufficient();
    }
}