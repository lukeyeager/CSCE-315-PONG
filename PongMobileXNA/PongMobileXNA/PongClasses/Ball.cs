using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;

namespace PongClasses
{
    /// <summary>
    /// Represents a Pong ball
    /// </summary>
    public abstract class Ball
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public bool IsActive;

        public void Update(float elapsed)
        {
            Position += Velocity * elapsed;
        }
    }
    class DefaultBall : Ball
    {
        public float spin;
    }
}
