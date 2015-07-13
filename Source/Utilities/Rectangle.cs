using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;

namespace Turnable.Utilities
{
    public class Rectangle
    {
        public Position TopLeft { get; private set; }
        public Position BottomRight { get; private set; }

        public Rectangle(Position firstCorner, Position secondCorner)
        {
            TopLeft = new Position(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
            BottomRight = new Position(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));
        }

        public Rectangle(Position topLeft, int width, int height)
            : this(topLeft, new Position(topLeft.X + width - 1, topLeft.Y + height - 1))
        {
        }

        public int Width { 
            get 
            {
                return (BottomRight.X - TopLeft.X + 1);
            }
        }

        public int Height
        {
            get
            {
                return (BottomRight.Y - TopLeft.Y + 1);
            }
        }

        public static bool AreTouching(Rectangle firstRectangle, Rectangle secondRectangle)
        {
            int xOverlap = Math.Abs(Math.Min(firstRectangle.BottomRight.X, secondRectangle.BottomRight.X) - Math.Max(firstRectangle.TopLeft.X, secondRectangle.TopLeft.X));
            int yOverlap = Math.Abs(Math.Min(firstRectangle.BottomRight.Y, secondRectangle.BottomRight.Y) - Math.Max(firstRectangle.TopLeft.Y, secondRectangle.TopLeft.Y));

            // Rectangles diagonal to each other
            if (xOverlap == 1 && yOverlap == 1)
            {
                return false;
            }

            if (xOverlap == 1 || yOverlap == 1)
            {
                return true;
            }

            return false;
        }

        public bool IsValid()
        {
            return (Height >= 0 && Width >= 0);
        }
    }
}
