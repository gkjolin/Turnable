﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Turnable.Utilities.Api
{
    public interface ICoordinates
    {
        // Methods
        ICoordinates Copy();
        string ToString();

        // Properties
        int X { get; set; }
        int Y { get; set; }

        // Events
    }
}