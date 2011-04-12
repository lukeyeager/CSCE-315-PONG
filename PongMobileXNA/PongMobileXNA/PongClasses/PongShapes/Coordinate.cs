using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses.PongShapes
{
    public class Coordinate
    {
        public int x, y;

        public Coordinate(int X, int Y)
        {
            x = X;
            y = Y;
        }

        public override string ToString()
        {
            return "(" + x.ToString() + ", " + y.ToString() + ")";
        }
    }
}
