using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PongClasses
{
    public abstract class PongObject
    {
        public Shape shape;
        public abstract void UpdateShape();
    }
}
