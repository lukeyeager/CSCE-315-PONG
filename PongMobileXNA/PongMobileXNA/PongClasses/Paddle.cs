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
        public Paddle(Vector2 pos, Vector2 vel)
        {
            State = PaddleState.Release;
            Position = pos;
            Velocity = vel;
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
            
    }

    public class DefaultPaddle : Paddle
    {
        public DefaultPaddle(Vector2 pos, Vector2 vel)
            :base(pos,vel)
        {}
    }
}
