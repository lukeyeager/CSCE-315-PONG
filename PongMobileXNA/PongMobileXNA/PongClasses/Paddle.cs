using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;

using PONG;
using PongClasses.PongShapes;

namespace PongClasses
{
    public enum PaddleState
    {
        Invalid,
        Drag,
        Release
    }
    /// <summary>
    /// Represents a Paddle
    /// </summary>
    public abstract class Paddle : PongObject
    {
        public Paddle()
        {
            State = PaddleState.Release;
            MaxSpeed = 1.0f;
        }

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public PaddleState State;

        /// <summary>
        /// This gets multiplied by the default speed (250)
        /// </summary>
        /// <remarks>So, MaxSpeed=2 means twice the normal speed, 0.5 means half speed, etc</remarks>
        public float MaxSpeed;

        /// <summary>
        /// Update the paddle's velocity and position
        /// </summary>
        /// <param name="elapsed"></param>
        public void Update(float elapsed)
        {
            //Adjust Velocity to match the MaxSpeed magnitude
            if (Velocity.Length() > MaxSpeed)
                Velocity *= MaxSpeed / Velocity.Length();

            Position += Velocity * Settings.PaddleSpeedMultiplier * elapsed;
            UpdateShape();

            //Adjust velocity to decrease due to friction each update
            Velocity *= Settings.PaddleFriction;
        }

        public override void UpdateShape()
        {
            int roundRadius = Texture.Height / 2;
            shape = new PongShapes.Rectangle(
                new Coordinate((int)Position.X + roundRadius, (int)Position.Y),
                Texture.Width, Texture.Height, 0);
        }

        public Int32 Width
        {
            get { return Texture.Width; }
        }

        public Int32 Height
        {
            get { return Texture.Height; }
        }
    }

    public class DefaultPaddle : Paddle
    {
        public DefaultPaddle()
            : base()
        { }
    }
}
