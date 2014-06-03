using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Components;
using TurnItUp.Locations;

namespace TurnItUp.Interfaces
{
    public interface IViewport
    {
        ILevel Level { get; set; }
        Position MapOrigin { get; set; }
        int Width { get; set; }
        int Height { get; set; }

        void Move(Direction direction);

        void CenterOn(Position center);
    }
}
