using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses
{
    public abstract class PongObject
    {
        /// <summary>
        /// Is this object tracked in the collision manager?
        /// </summary>
        public bool IsTracked;

        public PongObject()
        {
            IsTracked = false;
        }
        public Shape shape;
        public abstract void UpdateShape();
    }
}
