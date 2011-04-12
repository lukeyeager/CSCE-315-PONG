using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses.PongShapes
{
    class Rectangle : Shape
    {
        protected Coordinate Corner1, Corner2, Corner3, Corner4;
        protected double Width, Height, Direction;

        public Rectangle(Coordinate corner, double width, double height, double direction)
        {
            Corner1 = corner;
            Corner2 = new Coordinate(Corner1.x + (int)(width * Math.Cos(direction)),
                Corner1.y + (int)(width * Math.Sin(direction)));
            Corner3 = new Coordinate(Corner2.x + (int)(height * Math.Cos(direction - Math.PI / 2)),
                Corner2.y - (int)(height * Math.Sin(direction - Math.PI / 2)));
            Corner4 = new Coordinate(Corner1.x + (int)(height * Math.Cos(direction - Math.PI / 2)),
                Corner1.y - (int)(height * Math.Sin(direction - Math.PI / 2)));
            Width = Math.Abs(width);
            Height = Math.Abs(height);
            Direction = direction - (Math.IEEERemainder(direction, (2 * Math.PI)) * (2 * Math.PI));
            Line l1 = new Line(Corner1, Corner3);
            Line l2 = new Line(Corner2, Corner4);
            Center = Line.GetIntersection(l1, l2);
        }

        public override int GetHashCode()
        {
            return (Corner1.x + Corner1.y) * 100 + (Corner2.x + Corner2.y) * 1000 +
                (Corner3.x + Corner3.y) * 10000 + (Corner4.x + Corner4.y) * 100000;
        }

        public override string ToString()
        {
            return "Rectangle [" + Corner1.ToString() + ", " + Corner2.ToString() + ", "
                + Corner3.ToString() + ", " + Corner4.ToString() + "]";
        }

        public override Coordinate GetExtremityInDirection(Coordinate dir)
        {
            //returns a point on the line
            if (dir == Center)
            {
                return Center;
            }
            Line l = new Line(Center, dir);
            Line l12 = new Line(Corner1, Corner2);
            Line l23 = new Line(Corner2, Corner3);
            Line l34 = new Line(Corner3, Corner4);
            Line l41 = new Line(Corner4, Corner1);
            Coordinate c12 = new Coordinate(0, 0);
            Coordinate c23 = new Coordinate(0, 0);
            Coordinate c34 = new Coordinate(0, 0);
            Coordinate c41 = new Coordinate(0, 0);
            double smallest = 1000000000;
            double temp;
            int c = 0;
            try
            {
                c12 = Line.GetIntersection(l, l12);
                temp = Line.GetLength(Center, c12);
                if (temp < smallest)
                {
                    smallest = temp;
                    c = 1;
                }
            }
            catch (System.Exception)
            { }
            try
            {
                c23 = Line.GetIntersection(l, l23);
                temp = Line.GetLength(Center, c23);
                if (temp < smallest)
                {
                    smallest = temp;
                    c = 2;
                }
            }
            catch (System.Exception)
            { }
            try
            {
                c34 = Line.GetIntersection(l, l34);
                temp = Line.GetLength(Center, c34);
                if (temp < smallest)
                {
                    smallest = temp;
                    c = 3;
                }
            }
            catch (System.Exception)
            { }
            try
            {
                c41 = Line.GetIntersection(l, l41);
                temp = Line.GetLength(Center, c41);
                if (temp < smallest)
                {
                    smallest = temp;
                    c = 4;
                }
            }
            catch (System.Exception)
            { }
            switch (c)
            {
                case 1:
                    return c12;
                case 2:
                    return c23;
                case 3:
                    return c34;
                case 4:
                    return c41;
            }
            return Center; //won't ever get here, but need to have this so the compiler doesn't complain, so *shrug*
        }

        public override int GetTopY()
        {
            if (Direction >= 0 && Direction <= Math.PI / 2)
            {
                return Corner2.y;
            }
            else if (Direction > Math.PI / 2 && Direction <= Math.PI)
            {
                return Corner3.y;
            }
            else if (Direction > Math.PI && Direction <= 3 * Math.PI / 2)
            {
                return Corner4.y;
            }
            return Corner1.y;
        }

        public override int GetBottomY()
        {
            if (Direction >= 0 && Direction <= Math.PI / 2)
            {
                return Corner4.y;
            }
            else if (Direction > Math.PI / 2 && Direction <= Math.PI)
            {
                return Corner1.y;
            }
            else if (Direction > Math.PI && Direction <= 3 * Math.PI / 2)
            {
                return Corner2.y;
            }
            return Corner3.y;
        }

        public override int GetLeftX()
        {
            if (Direction >= 0 && Direction <= Math.PI / 2)
            {
                return Corner1.x;
            }
            else if (Direction > Math.PI / 2 && Direction <= Math.PI)
            {
                return Corner2.x;
            }
            else if (Direction > Math.PI && Direction <= 3 * Math.PI / 2)
            {
                return Corner3.x;
            }
            return Corner4.x;
        }

        public override int GetRightX()
        {
            if (Direction >= 0 && Direction <= Math.PI / 2)
            {
                return Corner3.x;
            }
            else if (Direction > Math.PI / 2 && Direction <= Math.PI)
            {
                return Corner4.x;
            }
            else if (Direction > Math.PI && Direction <= 3 * Math.PI / 2)
            {
                return Corner1.x;
            }
            return Corner2.x;
        }

        public override bool Contains(Coordinate pos)
        {
            Line l1, l2;
            if (Corner1.y > Corner3.y)
            {
                l1 = new Line(Corner1, Corner2);
                l2 = new Line(Corner3, Corner4);
            }
            else
            {
                l1 = new Line(Corner3, Corner4);
                l2 = new Line(Corner1, Corner2);
            }
            try
            {
                if (pos.y > l1.GetY(pos.x) || pos.y < l2.GetY(pos.x))
                {
                    return false;
                }
            }
            catch (System.Exception)
            {
                if (pos.x > Math.Max(Corner1.x, Corner3.x) || pos.x < Math.Min(Corner1.x, Corner3.x))
                {
                    return false;
                }
            }
            if (Corner2.y > Corner4.y)
            {
                l1 = new Line(Corner2, Corner3);
                l2 = new Line(Corner4, Corner1);
            }
            else
            {
                l1 = new Line(Corner4, Corner1);
                l2 = new Line(Corner2, Corner3);
            }
            try
            {
                if (pos.y > l1.GetY(pos.x) || pos.y < l2.GetY(pos.x))
                {
                    return false;
                }
            }
            catch (System.Exception)
            {
                if (pos.x > Math.Max(Corner2.x, Corner4.x) || pos.x < Math.Min(Corner2.x, Corner4.x))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
