using Entropy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TurnItUp.Interfaces;

namespace TurnItUp.AI.StateMachines
{
    public class StateMachine : IComponent
    {
        public Entity Owner { get; set; }

        public IState CurrentState { get; set; }
        public IState PreviousState { get; set; }
        public IState GlobalState { get; set; }

        public StateMachine(Entity entity)
        {
            Owner = entity;
        }

        public void ChangeState(IState newState)
        {
            if (newState == null)
            {
                throw new ArgumentNullException("<StateMachine::ChangeState> : trying to change to a null state.");
            }

            PreviousState = CurrentState;
            CurrentState.Exit(Owner);
            CurrentState = newState;
            newState.Enter(Owner);
        }

        public bool IsInState(Type stateType)
        {
            return CurrentState.GetType() == stateType;
        }
    }
}
