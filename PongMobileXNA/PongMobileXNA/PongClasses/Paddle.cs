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
        }

        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
        public PaddleState State;
        public Int32 Width
        {
            get
            {
                return Texture.Width;
            }
        }

        public Int32 Height
        {
            get
            {
                return Texture.Height;
            }
        }

        public override void UpdateShape()
        {
            int roundRadius = Texture.Height / 2;
            shape = new PongShapes.Rectangle(
                new Coordinate((int)Position.X + roundRadius, (int)Position.Y),
                Texture.Width, Texture.Height, 0);
        }
    }

    public class DefaultPaddle : Paddle
    {
        public DefaultPaddle()
            : base()
        { }
    }
}
