using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using PongClasses.PongShapes;

namespace PongClasses
{
    /// <summary>
    /// Represents a Pong ball
    /// </summary>
    public class Ball : PongObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float spin;
        public Texture2D Texture;
        public bool IsActive;

        public void Update(float elapsed)
        {
            Position += Velocity * elapsed;
            UpdateShape();
        }

        public override void UpdateShape()
        {
            int ballRadius = Texture.Width / 2;
            shape = new Circle(new Coordinate((int)Position.X + ballRadius, (int)Position.Y + ballRadius), ballRadius);
        }

        public Int32 Diameter
        {
            get
            {
                return Texture.Width;
            }
        }
    }
}
