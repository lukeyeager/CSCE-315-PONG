using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;

using PongClasses.PongShapes;
using PONG;

namespace PongClasses
{
    /// <summary>
    /// Represents a Pong ball
    /// </summary>
    public class Ball : PongObject
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public float Spin;
        public float Rotation;
        public Texture2D Texture;
        public bool IsActive;

        /// <summary>
        /// This gets multiplied by the default speed (250)
        /// </summary>
        /// <remarks>So, MaxSpeed=2 means twice the normal speed, 0.5 means half speed, etc</remarks>
        public float MaxSpeed;

        public void Update(float elapsed)
        {
            if (!IsActive)
            {
                if (IsTracked)
                {
                    CollisionManager.RemoveObject("Ball", this);
                    IsTracked = false;
                }
                return;
            }

            if (!IsTracked)
            {
                CollisionManager.AddObject("Ball", this);
                IsTracked = true;
            }

            Rotation += Spin * elapsed;

            //Adjust Velocity to match the MaxSpeed magnitude
            Velocity *= (MaxSpeed * Settings.BallSpeedMultiplier / Velocity.Length());

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
            get { return Texture.Width; }
        }

        public Int32 Radius
        {
            get { return Diameter / 2; }
        }
    }
}
