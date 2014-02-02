using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;

namespace TurnItUp.AI.State_Machines
{
    public class StateMachine : IComponent
    {
        public Entity Entity { get; set; }

        public IState CurrentState { get; set; }
        public IState PreviousState { get; set; }
        public IState GlobalState { get; set; }

        public StateMachine(Entity entity)
        {
            Entity = entity;
        }

        public void ChangeState(IState newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException("<StateMachine::ChangeState> : trying to change to a null state.");
            }

            PreviousState = CurrentState;
            CurrentState.Exit(Entity);
            CurrentState = newState;
            newState.Enter(Entity);
        }

        public bool IsInState(Type stateType)
        {
            return CurrentState.GetType() == stateType;
        }
    }
}
