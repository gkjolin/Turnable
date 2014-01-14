using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace TurnItUp.Tmx
{
    public class ElementList<T> : KeyedCollection<string, T> where T : IElement
    {
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}
