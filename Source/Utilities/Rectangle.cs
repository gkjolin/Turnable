using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.LevelGenerators;

namespace Turnable.Utilities
{
    public class Rectangle
    {
        public Position TopLeft { get; private set; }
        public Position BottomRight { get; private set; }
        public List<Segment> Edges { get; private set; }

        public Rectangle(Position firstCorner, Position secondCorner)
        {
            TopLeft = new Position(Math.Min(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));
            BottomRight = new Position(Math.Max(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));

            // Intialize the edges
            Edges = new List<Segment>();
            Edges.Add(new Segment(new Position(TopLeft.X, TopLeft.Y), new Position(BottomRight.X, TopLeft.Y)));
            Edges.Add(new Segment(new Position(BottomRight.X, TopLeft.Y), new Position(BottomRight.X, BottomRight.Y)));
            Edges.Add(new Segment(new Position(BottomRight.X, BottomRight.Y), new Position(TopLeft.X, BottomRight.Y)));
            Edges.Add(new Segment(new Position(TopLeft.X, BottomRight.Y), new Position(TopLeft.X, TopLeft.Y)));
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

        public bool IsTouching(Rectangle other)
        {
            int xOverlap = Math.Abs(Math.Min(BottomRight.X, other.BottomRight.X) - Math.Max(TopLeft.X, other.TopLeft.X));
            int yOverlap = Math.Abs(Math.Min(BottomRight.Y, other.BottomRight.Y) - Math.Max(TopLeft.Y, other.TopLeft.Y));

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

        public bool Contains(Position position)
        {
            return (position.X >= TopLeft.X && position.Y >= TopLeft.Y && position.X <= BottomRight.X && position.Y <= BottomRight.Y);
        }

        public bool Contains(Rectangle other)
        {
            return (Contains(other.TopLeft) && Contains(other.BottomRight));
        }

        public static Rectangle BuildRandomRectangle(Rectangle bounds)
        {
            Random random = new Random();

            Position firstCorner = new Position(random.Next(bounds.TopLeft.X, bounds.BottomRight.X), random.Next(bounds.TopLeft.Y, bounds.BottomRight.Y));
            Position secondCorner = new Position(random.Next(bounds.TopLeft.X, bounds.BottomRight.X), random.Next(bounds.TopLeft.Y, bounds.BottomRight.Y));

            return new Rectangle(firstCorner, secondCorner);
        }

        public List<Segment> GetFacingEdges(Rectangle other)
        {
            List<Segment> facingEdges = new List<Segment>();
            int shortestDistance = Int16.MaxValue;

            foreach (Segment edge in Edges)
            {
                foreach (Segment otherEdge in other.Edges)
                {
                    if (edge.IsParallelTo(otherEdge)) // Only check the distance between two parallel edges.
                    {
                        int parallelDistance = edge.DistanceBetween(otherEdge);

                        if (parallelDistance < shortestDistance)
                        {
                            shortestDistance = parallelDistance;
                            facingEdges.Clear();
                            facingEdges.Add(edge);
                            facingEdges.Add(otherEdge);
                        }
                    }
                }
            }

            return facingEdges;
        }
    }
}
