using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses.PongShapes
{
    class Circle : Shape
    {
        protected double Radius;

        public Circle(Coordinate center, double radius)
        {
            Radius = Math.Abs(radius);
            Center = center;
        }

        public override int GetHashCode()
        {
            return (int)(Radius * 1000000 + Center.x * 1000 + Center.y);
        }

        public override string ToString()
        {
            return "Circle [" + Center.ToString() + ", " + Radius.ToString() + "]";
        }

        public override Coordinate GetExtremityInDirection(Coordinate dir)
        {
            if (dir == Center)
            {
                return Center;
            }
            double vx = dir.x - Center.x;
            double vy = dir.y - Center.y;
            double len = Math.Sqrt(Math.Pow(vx, 2) + Math.Pow(vy, 2));
            return new Coordinate((int)(Center.x + vx / len * Radius), (int)(Center.y + vy / len * Radius));
        }

        public override int GetTopY()
        {
            return (int)(Center.y + Radius);
        }

        public override int GetBottomY()
        {
            return (int)(Center.y - Radius);
        }

        public override int GetLeftX()
        {
            return (int)(Center.x - Radius);
        }

        public override int GetRightX()
        {
            return (int)(Center.x + Radius);
        }

        public override bool Contains(Coordinate pos)
        {
            return Line.GetLength(Center, pos) <= Radius;
        }
    }
}
