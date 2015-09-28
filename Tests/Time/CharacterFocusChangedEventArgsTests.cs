using System;
using Entropy.Core;
using NUnit.Framework;

namespace Tests.Time
{
    [TestFixture]
    public class CharacterFocusChangedEventArgsTests
    {
        [Test]
        public void Constructor_InitializesAllProperties()
        {
            Entity previousFocusedCharacter = new Entity();
            Entity currentFocusedCharacter = new Entity();

            CharacterFocusChangedEventArgs characterFocusChangedEventArgs = new CharacterFocusChangedEventArgs(previousFocusedCharacter, currentFocusedCharacter);

            Assert.That(characterFocusChangedEventArgs.PreviousFocusedCharacter, Is.SameAs(previousFocusedCharacter));
        }
    }
}
