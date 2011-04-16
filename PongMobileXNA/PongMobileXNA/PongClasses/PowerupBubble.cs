using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using PONG;
using PongClasses.PongShapes;

namespace PongClasses
{
    /// <summary>
    /// A class to represent powerups on the screen that haven't been activated yet.
    /// </summary>
    /// <remarks>
    /// They look like bubbles, so we're calling them PowerupBubbles
    /// </remarks>
    class PowerupBubble : PongObject
    {
        public PowerupBubble()
        {
            IsActive = true;
            LifeTime = 30.0f;
            Velocity = new Vector2(0, 0);
        }

        public PowerupManager.Powerup powerup;
        public bool IsActive;
        public float LifeTime;
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public Int32 Diameter
        {
            get { return Texture.Width; }
        }
        public Int32 Radius
        {
            get { return Diameter / 2; }
        }

        //Attibutes while powerup is within a bubble
        public Vector2 P_Position;
        public Vector2 P_Velocity;
        public Texture2D P_Texture;
        public float P_Rotation;
        public float P_Spin;

        public void Update(float elapsed, Random random)
        {
            LifeTime -= elapsed;
            if (LifeTime <= 0.0f)
            {
                IsActive = false;
                return;
            }

            //Create some random fluctuation in velocity
            double angle = 2 * Math.PI * random.NextDouble();
            Velocity += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            Position += Velocity * elapsed;

            P_Rotation += P_Spin * elapsed;
            P_Position = Position + new Vector2(40, 40);

            UpdateShape();
        }

        public override void UpdateShape()
        {
            shape = new Circle(new Coordinate((int)Position.X + Radius, (int)Position.Y + Radius), Radius);
        }
    }
}
