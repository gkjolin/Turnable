using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Turnable.Components;
using Turnable.LevelGenerators;
using Turnable.Vision;

namespace Turnable.Utilities
{
    public class Rectangle
    {
        public Position BottomLeft { get; private set; }
        public Position TopRight { get; private set; }
        public List<LineSegment> Edges { get; private set; }

        public Rectangle(Position firstCorner, Position secondCorner)
        {
            BottomLeft = new Position(Math.Min(firstCorner.X, secondCorner.X), Math.Min(firstCorner.Y, secondCorner.Y));
            TopRight = new Position(Math.Max(firstCorner.X, secondCorner.X), Math.Max(firstCorner.Y, secondCorner.Y));

            // Intialize the edges
            Edges = new List<LineSegment>();
            Edges.Add(new LineSegment(new Position(BottomLeft.X, BottomLeft.Y), new Position(TopRight.X, BottomLeft.Y)));
            Edges.Add(new LineSegment(new Position(TopRight.X, BottomLeft.Y), new Position(TopRight.X, TopRight.Y)));
            Edges.Add(new LineSegment(new Position(TopRight.X, TopRight.Y), new Position(BottomLeft.X, TopRight.Y)));
            Edges.Add(new LineSegment(new Position(BottomLeft.X, TopRight.Y), new Position(BottomLeft.X, BottomLeft.Y)));
        }

        public Rectangle(Position topLeft, int width, int height)
            : this(topLeft, new Position(topLeft.X + width - 1, topLeft.Y + height - 1))
        {
        }

        public int Width { 
            get 
            {
                return (TopRight.X - BottomLeft.X + 1);
            }
        }

        public int Height
        {
            get
            {
                return (TopRight.Y - BottomLeft.Y + 1);
            }
        }

        public bool IsTouching(Rectangle other)
        {
            bool isTouching = false;

            // If any two edges of the rectangles are touching, the rectangles are touching.
            foreach (LineSegment edge in Edges)
            {
                foreach (LineSegment otherEdge in other.Edges)
                {
                    if (edge.IsTouching(otherEdge))
                    {
                        isTouching = true;
                        break;
                    }
                }

                if (isTouching)
                {
                    break;
                }
            }

            return isTouching;
        }

        public bool Contains(Position position)
        {
            return (position.X >= BottomLeft.X && position.Y >= BottomLeft.Y && position.X <= TopRight.X && position.Y <= TopRight.Y);
        }

        public bool Contains(Rectangle other)
        {
            return (Contains(other.BottomLeft) && Contains(other.TopRight));
        }

        public static Rectangle BuildRandomRectangle(Rectangle bounds)
        {
            Random random = new Random();

            Position firstCorner = new Position(random.Next(bounds.BottomLeft.X, bounds.TopRight.X), random.Next(bounds.BottomLeft.Y, bounds.TopRight.Y));
            Position secondCorner = new Position(random.Next(bounds.BottomLeft.X, bounds.TopRight.X), random.Next(bounds.BottomLeft.Y, bounds.TopRight.Y));

            return new Rectangle(firstCorner, secondCorner);
        }

        public List<LineSegment> GetClosestEdges(Rectangle other)
        {
            // This method finds the closest edges for two rectangles.
            // This is mostly used to join the two rectangles with a corridor. 
            LineSegment intersectionLine;
            List<LineSegment> facingEdges = new List<LineSegment>();
            int shortestDistance = Int16.MaxValue;

            // Are there two parallel edges that can work?
            foreach (LineSegment edge in Edges)
            {
                foreach (LineSegment otherEdge in other.Edges)
                {
                    if (edge.IsParallelTo(otherEdge)) // Only check the distance between two parallel edges.
                    {
                        int parallelDistance = edge.DistanceBetween(otherEdge);

                        if (parallelDistance < shortestDistance && parallelDistance != 0) // parallelDistance = 0 indicates that the two line segments are on the same line which makes the two edges bad candidates for the closest edges.
                        {
                            // If the line between the midpoints of the edges intersects either rectangle, then this isn't a good candidate for the closest edges.
                            intersectionLine = new LineSegment(edge.GetMidpoint(), otherEdge.GetMidpoint());

                            if (parallelDistance == 1 && this.IsTouching(other)) // If the two rectangles are touching, then return the two touching edges
                            {
                                shortestDistance = parallelDistance;
                                facingEdges.Clear();
                                facingEdges.Add(edge);
                                facingEdges.Add(otherEdge);
                            }
                            else
                            {
                                if (intersectionLine.Intersects(this, true) || intersectionLine.Intersects(other, true)) // If the parallelDistance is equal to 1, these two edges are touching each other which is a suitable candidate for the closest edges.
                                {
                                }
                                else
                                {
                                    shortestDistance = parallelDistance;
                                    facingEdges.Clear();
                                    facingEdges.Add(edge);
                                    facingEdges.Add(otherEdge);
                                }
                            }
                        }
                    }
                }
            }

            if (facingEdges.Count == 0) // No two parallel edges were found that would work, try edges that are not parallel to each other
            {
            }

            return facingEdges;
        }
    }
}
