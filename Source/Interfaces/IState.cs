using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TurnItUp.Interfaces
{
    public interface IState
    {
        void Enter(Entity entity);
        void Execute(Entity entity);
        void Exit(Entity entity);
    }
}
