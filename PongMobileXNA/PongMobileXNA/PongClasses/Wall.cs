using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PongClasses.PongShapes;

namespace PongClasses
{
    public abstract class Wall : PongObject
    {
    }

    public class DefaultWall : Wall
    {
        public override void UpdateShape()
        {
            //no-op currently as we don't have moving walls (yet)
            //once we do we can make it a different class or something
        }
    }
}
