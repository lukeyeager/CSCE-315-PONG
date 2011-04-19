using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PONG
{
    /// <summary>
    /// This static class determines gameplay settings common to the whole game
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// This fixed value will me multiplied by some value (1 by default) to determine the ball's speed
        /// </summary>
        public static Int32 BallSpeedMultiplier = 250;
        /// <summary>
        /// This fixed value will me multiplied by some value (1 by default) to determine the paddle's speed
        /// </summary>
        public static Int32 PaddleSpeedMultiplier = 300;
        /// <summary>
        /// This fixed value lets the velocity of a paddle decay to zero over time
        /// </summary>
        public static float PaddleFriction = 0.9f;
    }
}
