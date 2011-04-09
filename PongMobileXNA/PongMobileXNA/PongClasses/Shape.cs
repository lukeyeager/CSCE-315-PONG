using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PongClasses.PongShapes;

namespace PongClasses
{
    public abstract class Shape
    {
        protected Coordinate Center;

        //gets the center of the Shape
        public Coordinate GetCenter()
        {
            return Center;
        }
        //gets an edge or corner of the shape in a given direction
        public abstract Coordinate GetExtremityInDirection(Coordinate dir);
        //these 4 methods return info for the bounding box on a particular shape
        //since bounding boxes are easy to check for collisions versus complex shapes
        public abstract int GetTopY();
        public abstract int GetBottomY();
        public abstract int GetLeftX();
        public abstract int GetRightX();
        //see if a point is contained in a shape
        public abstract bool Contains(Coordinate pos);

        public override bool Equals(Object obj)
        {
            return ToString() == obj.ToString();
        }
        public abstract override int GetHashCode();
    }
}
