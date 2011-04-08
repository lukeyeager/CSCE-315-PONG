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
    public enum PaddleState
    {
        Invalid,
        Drag,
        Release
    }
    /// <summary>
    /// Represents a Paddle
    /// </summary>
    public abstract class Paddle
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
            
    }

    public class DefaultPaddle : Paddle
    {
        public DefaultPaddle()
            :base()
        {}
    }
}
