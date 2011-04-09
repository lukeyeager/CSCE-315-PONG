using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses.PongShapes
{
    class RoundedRectangle : Rectangle
    {
        protected Circle End1, End2;

        public RoundedRectangle(Coordinate corner, double width, double height, double direction)
            : base(corner, width, height, direction)
        {
            Line l1 = new Line(Corner1, Corner4);
            Line l2 = new Line(Corner2, Corner3);
            End1 = new Circle(l1.GetCenter(), height / 2);
            End2 = new Circle(l2.GetCenter(), height / 2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode() + End1.GetHashCode() + End2.GetHashCode();
        }

        public override string ToString()
        {
            return "RoundedRectangle [" + base.ToString() + ", " + End1.ToString() + ", " + End2.ToString() + "]";
        }

        public override Coordinate GetExtremityInDirection(Coordinate dir)
        {
            Coordinate ext = base.GetExtremityInDirection(dir);
            if (End1.Contains(ext))
            {
                return End1.GetExtremityInDirection(dir);
            }
            if (End2.Contains(ext))
            {
                return End2.GetExtremityInDirection(dir);
            }
            return ext;
        }

        public override int GetTopY()
        {
            return Math.Max(End1.GetTopY(), End2.GetTopY());
        }

        public override int GetBottomY()
        {
            return Math.Min(End1.GetBottomY(), End2.GetBottomY());
        }

        public override int GetLeftX()
        {
            return Math.Min(End1.GetLeftX(), End2.GetLeftX());
        }

        public override int GetRightX()
        {
            return Math.Max(End1.GetRightX(), End2.GetRightX());
        }

        public override bool Contains(Coordinate pos)
        {
            return base.Contains(pos) || End1.Contains(pos) || End2.Contains(pos);
        }
    }
}
