using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace TurnItUp.Tmx
{
    public class Layer
    {
        public string Name { get; private set; }
        public double Opacity { get; private set; }
        public bool IsVisible { get; private set; }

        public Layer(XElement xLayer)
        {
            Name = (string)xLayer.Attribute("name");
            Opacity = (double?)xLayer.Attribute("opacity") ?? 1.0;
            IsVisible = (bool?)xLayer.Attribute("visible") ?? true;
        }
    }
}
