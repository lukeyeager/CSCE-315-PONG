using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework.Content;

using PongClasses;

namespace PONG
{
    public class BallManager
    {
        #region Fields

        //Textures
        public Texture2D defaultBallTexture;
        public Texture2D fastBallTexture;
        SpriteBatch spriteBatch;

        //Data
        public List<Ball> balls;

        PongGameScreen screen;

        #endregion
        
        #region Initialization

        public BallManager(ContentManager content, PongGameScreen screen, SpriteBatch spriteBatch)
        {
            defaultBallTexture = content.Load<Texture2D>("Images/defaultBall35px");
            //fastBallTexture = content.Load<Texture2D>("Images/fastBall");

            balls = new List<Ball>();

            this.screen = screen;
            this.spriteBatch = spriteBatch;
        }
        
        #endregion

        #region Update

        /// <summary>
        /// Update all active balls.
        /// </summary>
        /// <param name="elapsed">The amount of time elapsed since last Update.</param>
        public int Update(float elapsed)
        {
            int activeBalls = 0;
            foreach (Ball ball in balls)
            {
                ball.Update(elapsed);

                if (ball.IsActive == false) //Ignore inactive balls
                    continue;

                ++activeBalls;

                if ((ball.Position.Y + ball.Texture.Height) < screen.screenTopBound || ball.Position.Y > screen.screenBottomBound)
                {
                    //mark as inactive and remove from list of shapes
                    ball.IsActive = false;
                }

                //TODO: check for collide with left wall
                if (ball.Position.X < screen.screenLeftBound)
                {
                    ball.Velocity.X = -ball.Velocity.X;
                    screen.particles.CreateDefaultCollisionEffect(new Vector2(ball.Position.X + ball.Diameter / 2, ball.Position.Y + ball.Diameter / 2));
                }
                //TODO: check for collide with right wall
                else if (ball.Position.X + ball.Texture.Width > screen.screenRightBound)
                {
                    ball.Velocity.X = -ball.Velocity.X;
                    screen.particles.CreateDefaultCollisionEffect(new Vector2(ball.Position.X + ball.Diameter / 2, ball.Position.Y + ball.Diameter / 2));
                }
            }

            return activeBalls;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws all of the balls
        /// </summary>
        public void Draw()
        {
            foreach (Ball b in balls)
            {
                if (b.IsActive)
                {
                    spriteBatch.Draw(b.Texture, b.Position, null, Color.White, b.Rotation, new Vector2(b.Radius, b.Radius), 1, SpriteEffects.None, 0.0f);
                }
            }
        }

        #endregion

        #region Create Ball Functions

        /// <summary>
        /// Creates a new ball and adds to the list of balls
        /// </summary>
        /// <param name="ball"></param>
        /// <returns>
        /// Prefers to use 
        /// </returns>
        Ball CreateBall(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            Ball b = null;
            foreach (Ball old_ball in balls)
            {
                if (!old_ball.IsActive)
                {
                    b = old_ball;
                    break;
                }
            }
            if (b == null)
            {
                b = new Ball();
                balls.Add(b);
            }

            b.IsActive = true;
            b.Position = position;
            b.Velocity = velocity;
            b.MaxSpeed = 1.0f;
            b.Spin = 0f;
            b.Texture = texture;
            b.UpdateShape();

            return b;
        }

        /// <summary>
        /// Returns an instance of a default ball
        /// </summary>
        /// <returns>
        /// A ball, with the default texture.
        /// </returns>
        public Ball CreateDefaultBall()
        {
            return CreateBall(defaultBallTexture, new Vector2(), new Vector2());
        }

        /// <summary>
        /// Returns an instance of a default ball, initialized to some position and velocity
        /// </summary>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <returns>
        /// A ball, with the default texture.
        /// </returns>
        public Ball CreateDefaultBall(Vector2 position, Vector2 velocity)
        {
            return CreateBall(defaultBallTexture, position, velocity);
        }

        /// <summary>
        /// Returns an instance of a multiball
        /// </summary>
        /// <returns>
        /// A ball, with the multiball texture
        /// </returns>
        public Ball CreateMultiBall()
        {
            return CreateDefaultBall();
        }

        /// <summary>
        /// Returns an instance of a fastball
        /// </summary>
        /// <returns>
        /// A ball, with the fastball texture
        /// </returns>
        public Ball CreateFastBall()
        {
            Ball b = CreateDefaultBall();
            b.Texture = fastBallTexture;
            b.MaxSpeed = 2.0f;
            return b;
        }

        #endregion


    }
}
