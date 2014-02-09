using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.AI.State_Machines;
using Entropy;
using Tests.SupportingClasses;
using TurnItUp.Interfaces;
using Moq;

namespace Tests.AI.StateMachines
{
    [TestClass]
    public class StateMachineTests
    {
        private StateMachine _stateMachine;
        private World _world;
        private Entity _entity;

        [TestInitialize]
        public void Initialize()
        {
            _stateMachine = new StateMachine(_entity);

            _world = new World();
            _entity = _world.CreateEntity();
        }

        [TestMethod]
        public void StateMachine_IsAnEntropyComponent()
        {
            StateMachine stateMachine = new StateMachine(_entity);

            Assert.IsInstanceOfType(stateMachine, typeof(IComponent));
        }

        [TestMethod]
        public void StateMachine_Construction_IsSuccessful()
        {
            StateMachine stateMachine = new StateMachine(_entity);

            Assert.AreEqual(_entity, stateMachine.Owner);
            Assert.IsNull(stateMachine.CurrentState);
            Assert.IsNull(stateMachine.PreviousState);
            Assert.IsNull(stateMachine.GlobalState);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void StateMachine_TryingToChangeToANullState_ThrowsAnException()
        {
            _stateMachine.ChangeState(null);
        }

        [TestMethod]
        public void StateMachine_ChangingState_CorrectlyCallsTheExitAndEnterMethodOfEachStateWhileSavingTheCurrentStateToPreviousState()
        {
            Mock<IState> currentStateMock = new Mock<IState>();
            Mock<IState> newStateMock = new Mock<IState>();

            _stateMachine.CurrentState = currentStateMock.Object;

            _stateMachine.ChangeState(newStateMock.Object);

            currentStateMock.Verify(cs => cs.Exit(_stateMachine.Owner));
            newStateMock.Verify(ns => ns.Enter(_stateMachine.Owner));
            Assert.AreEqual(currentStateMock.Object, _stateMachine.PreviousState);
            Assert.AreEqual(newStateMock.Object, _stateMachine.CurrentState);
        }

        [TestMethod]
        public void StateMachine_CanDetermineIfItInAParticularState()
        {
            _stateMachine.CurrentState = new StateType1();

            Assert.IsTrue(_stateMachine.IsInState(typeof(StateType1)));
            Assert.IsFalse(_stateMachine.IsInState(typeof(StateType2)));
        }
    }
}
