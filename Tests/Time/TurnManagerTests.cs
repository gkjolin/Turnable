using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Turnable.Api;
using Tests.Factories;
using Turnable.Characters;
using Turnable.Time;
using Entropy.Core;

namespace Tests.Time
{
    [TestFixture]
    public class TurnManagerTests
    {
        private ILevel _level;
        private ICharacterManager _characterManager;
        private ITurnManager _turnManager;
        private CharacterFocusChangedEventArgs _characterFocusChangedEventArgs;
        private bool _eventTriggeredFlag;

        [SetUp]
        public void SetUp()
        {
            _level = LocationsFactory.BuildLevel();
            _characterManager = new CharacterManager(_level);
            _characterManager.SetUpPcs();
            _turnManager = new TurnManager(_characterManager);
            _eventTriggeredFlag = false;
        }

        [Test]
        public void Constructor_InitializesAllProperties()
        {
            TurnManager turnManager = new TurnManager(_characterManager);

            Assert.That(turnManager.CharacterManager, Is.SameAs(_characterManager));

            // Assert that all the Pcs have been added up in the same order as they are in the CharacterManager
            Assert.That(turnManager.Queue, Is.Not.Null);
            for (int index = 0; index <_characterManager.Pcs.Count; index++)
            {
                Assert.That(turnManager.Queue[index], Is.SameAs(_characterManager.Pcs[index]));
            }
            Assert.That(turnManager.FocusedCharacter, Is.SameAs(_characterManager.Pcs[0]));
        }

        [Test]
        public void SwitchFocus_SwitchesTheFocusedCharacterToTheNextCharacterInQueue()
        {
            _turnManager.SwitchFocus();

            Assert.That(_turnManager.FocusedCharacter, Is.SameAs(_characterManager.Pcs[1]));
        }

        [Test]
        public void SwitchFocus_WhenCalledManyTimesInSuccession_CyclesThroughTheCharactersInTheQueueInOrder()
        {
            for (int cycle = 0; cycle < 2; cycle++)
            {
                for (int index = 0; index < _characterManager.Pcs.Count; index++)
                {
                    Assert.That(_turnManager.FocusedCharacter, Is.SameAs(_characterManager.Pcs[index]));

                    _turnManager.SwitchFocus();
                }
            }
        }

        [Test]
        public void SwitchFocus_WhenCalled_RaisesACharacterFocusChangedEvent()
        {
            Entity previousFocusedCharacter = _turnManager.FocusedCharacter;

            _turnManager.CharacterFocusChanged += SetEventTriggeredFlag;
            _turnManager.SwitchFocus();

            Assert.That(_eventTriggeredFlag, Is.True);
            Assert.That(_characterFocusChangedEventArgs.CurrentFocusedCharacter, Is.SameAs(_turnManager.FocusedCharacter));
            Assert.That(_characterFocusChangedEventArgs.PreviousFocusedCharacter, Is.SameAs(previousFocusedCharacter));
        }

        [Test]
        public void SwitchFocus_WhenCalledWithAQueueThatHasOnlyOneCharacterInIt_DoesNotRaiseACharacterFocusChangedEvent()
        {
            // Remove all the characters from the queue except the first one
            _turnManager.Queue.RemoveRange(1, _turnManager.Queue.Count - 1);

            _turnManager.SwitchFocus();

            Assert.That(_eventTriggeredFlag, Is.False);
        }

        private void SetEventTriggeredFlag(object sender, CharacterFocusChangedEventArgs e)
        {
            _eventTriggeredFlag = true;
            _characterFocusChangedEventArgs = e;
        }
    }
}