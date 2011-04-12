using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses.PongShapes
{
    class Line
    {
        private Coordinate p1, p2;
        private double slope, intercept;
        private bool IsVerticalLine;

        public Line(Coordinate pt1, Coordinate pt2)
        {
            IsVerticalLine = false;
            if (pt1.x < pt2.x)
            {
                p1 = pt1;
                p2 = pt2;
            }
            else if (pt1.x > pt2.x)
            {
                p1 = pt2;
                p2 = pt1;
            }
            else
            {
                IsVerticalLine = true;
                if (pt1.y > pt2.y)
                {
                    p1 = pt1;
                    p2 = pt2;
                }
                else if (pt1.y < pt2.y)
                {
                    p1 = pt2;
                    p2 = pt1;
                }
                else
                {
                    throw new System.Exception();
                }
            }
            if (IsVerticalLine)
            {
                slope = 0; //unused
                intercept = 0; //unused
            }
            else
            {
                slope = (double)(p2.y - p1.y) / (p2.x - p1.x);
                intercept = p1.y - slope * p1.x;
            }
        }

        public Coordinate GetCenter()
        {
            return new Coordinate((p1.x + p2.x) / 2, (p1.y + p2.y) / 2);
        }

        public static Coordinate GetCenter(Coordinate pt1, Coordinate pt2)
        {
            return new Coordinate((pt1.x + pt2.x) / 2, (pt1.y + pt2.y) / 2);
        }

        public bool Contains(Coordinate pt)
        {
            if (IsVerticalLine)
            {
                return (pt.x == p1.x && pt.y <= p1.y && pt.y >= p2.y);
            }
            return (pt.y == GetY(pt.x) && pt.x >= p1.x && pt.x <= p2.x);
        }

        public double GetY(double x)
        {
            if (IsVerticalLine)
            {
                throw new System.Exception();
            }
            return slope * x + intercept;
        }

        public static Coordinate GetIntersection(Line l1, Line l2)
        {
            if (l1.IsVerticalLine)
            {
                if (l2.IsVerticalLine)
                {
                    throw new System.Exception();
                }
                return new Coordinate(l1.p1.x, (int)l2.GetY(l1.p1.x));
            }
            else if (l2.IsVerticalLine)
            {
                return new Coordinate(l2.p1.x, (int)l1.GetY(l2.p1.x));
            }
            else if (l1.slope == l2.slope)
            {
                throw new System.Exception();
            }
            double x = (l2.intercept - l1.intercept) / (l1.slope - l2.slope);
            double y = l1.GetY(x);
            return new Coordinate((int)x, (int)y);
        }

        public static double GetLength(Coordinate p1, Coordinate p2)
        {
            return Math.Sqrt(Math.Pow(Math.Abs(p1.x - p2.x), 2) + Math.Pow(Math.Abs(p1.y - p2.y), 2));
        }
    }
}
