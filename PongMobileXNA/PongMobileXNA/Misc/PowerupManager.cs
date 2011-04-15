using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using PongClasses;

namespace PONG
{
    /// <summary>
    /// A class for managing powerups.
    /// It keeps track of the list of powerups and handles time-out events, etc.
    /// </summary>
    public class PowerupManager
    {
        #region Fields

        //Textures
        public Texture2D multiPupTexture;
        public Texture2D fastPupTexture;
        public Texture2D PowerupBubbleTexture;

        Random random;
        SpriteBatch spriteBatch;
        List<PowerupBubble> bubbles;
        PongGameScreen screen;
        BallManager ballManager;

        #endregion

        #region Initialization

        public PowerupManager(ContentManager content, PongGameScreen screen, SpriteBatch spriteBatch)
        {
            multiPupTexture = content.Load<Texture2D>("Images/Powerups/Multiball");
            fastPupTexture = content.Load<Texture2D>("Images/Powerups/Fastball");
            PowerupBubbleTexture = content.Load<Texture2D>("Images/Powerups/Bubble");

            random = new Random();
            bubbles = new List<PowerupBubble>();

            this.screen = screen;
            this.spriteBatch = spriteBatch;
        }

        #endregion

        #region Update

        /// <summary>
        /// Update all active particles.
        /// </summary>
        /// <param name="elapsed">The amount of time elapsed since last Update.</param>
        public void Update(float elapsed)
        {
            foreach (PowerupBubble bubble in bubbles)
            {
                if (bubble.IsActive)
                {
                    bubble.LifeTime -= elapsed;
                    if (bubble.LifeTime <= 0.0f)
                    {
                        bubble.IsActive = false;
                        continue;
                    }
                    //Create some random fluctuation in velocity

                    double angle = 2 * Math.PI * random.NextDouble();
                    bubble.Velocity += new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

                    bubble.Position += bubble.Velocity * elapsed;
                    if (bubble.Position.X < screen.screenLeftBound)
                    {
                        bubble.Position.X = screen.screenLeftBound;
                        bubble.Velocity.X *= -1;
                    }
                    else if (bubble.Position.Y + bubble.Width > screen.screenRightBound)
                    {
                        bubble.Position.X = screen.screenRightBound - bubble.Width;
                        bubble.Velocity.X *= -1;
                    }
                    //TODO: handle hitting top and bottom

                    bubble.powerup.Rotation += bubble.powerup.Spin * elapsed;
                    bubble.powerup.Position = bubble.Position + new Vector2(40, 40);
                }
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the particles.
        /// </summary>
        public void Draw()
        {
            foreach (PowerupBubble bubble in bubbles)
            {
                if (!bubble.IsActive)
                    continue;
                spriteBatch.Draw(bubble.Texture, bubble.Position, Color.White);
                Powerup p = bubble.powerup;
                spriteBatch.Draw(p.Texture, p.Position, null, Color.White, p.Rotation, new Vector2(p.Texture.Width / 2, p.Texture.Height / 2), 1, SpriteEffects.None, 0.0f);
            }
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// Creates a PowerupBubble, preferring to reuse a dead one in the bubbles list 
        /// before creating a new one.
        /// </summary>
        /// <returns>
        /// The new bubble
        /// </returns>
        PowerupBubble CreateBubble()
        {
            PowerupBubble b = null;

            foreach (PowerupBubble bubble in bubbles)
            {
                if (!bubble.IsActive)
                {
                    b = bubble;
                    break;
                }
            }

            if (b == null)
            {
                b = new PowerupBubble();
                bubbles.Add(b);
            }

            return b;
        }

        #endregion

        #region Classes

        /// <summary>
        /// A class to represent powerups on the screen that haven't been activated yet.
        /// </summary>
        /// <remarks>
        /// They look like bubbles, so we're calling them PowerupBubbles
        /// </remarks>
        class PowerupBubble
        {
            public Powerup powerup;
            public bool IsActive;
            public float LifeTime;
            public Vector2 Position;
            public Vector2 Velocity;
            public Texture2D Texture;
            public Int32 Width
            {
                get { return Texture.Width; }
            }
            public Int32 Height
            {
                get { return Texture.Height; }
            }
        }

        /// <summary>
        /// An abstract class for defining powerups.
        /// </summary>
        public class Powerup
        {
            public float LifeTime;
            public event EventHandler<EventArgs> OnActivate;
            public event EventHandler<EventArgs> OnDeactivate;

            //Attibutes while powerup is within a bubble
            public Vector2 Position;
            public Vector2 Velocity;
            public Texture2D Texture;
            public float Rotation;
            public float Spin;
        }
        
        #endregion

        #region Multiball

        public void CreatePowerupMultiball()
        {
            PowerupBubble b = new PowerupBubble();
            b.IsActive = true;
            b.LifeTime = 100;
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(0, 0);
            b.Texture = PowerupBubbleTexture;

            Powerup p = new Powerup();
            p.LifeTime = 10;
            p.Position = new Vector2(250, 410);
            p.Velocity = new Vector2(0, 0);
            p.Texture = multiPupTexture;
            p.Rotation = 0f;
            p.Spin = 1.0f;
            p.OnActivate += ActivatePowerupMultiball;
            p.OnDeactivate += DectivatePowerupMultiball;
            b.powerup = p;
            
            bubbles.Add(b);
        }


        /// <summary>
        /// Create two new multiBalls
        /// </summary>
        void ActivatePowerupMultiball(object sender, EventArgs e)
        {
            Ball b = ballManager.CreateMultiBall();
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(200, 200);

            b = ballManager.CreateMultiBall();
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(-200, -200);
        }


        /// <summary>
        /// Called when multiballPowerup ends, does nothing for now
        /// </summary>
        void DectivatePowerupMultiball(object sender, EventArgs e)
        {
        }

        #endregion

        #region Fastball

        public void CreatePowerupFastball()
        {
            PowerupBubble b = new PowerupBubble();
            b.IsActive = true;
            b.LifeTime = 100;
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(0, 0);
            b.Texture = PowerupBubbleTexture;

            Powerup p = new Powerup();
            p.LifeTime = 10;
            p.Position = new Vector2(250, 410);
            p.Velocity = new Vector2(0, 0);
            p.Texture = fastPupTexture;
            p.Rotation = 0f;
            p.Spin = 1.0f;
            p.OnActivate += ActivatePowerupFastball;
            p.OnDeactivate += DectivatePowerupFastball;
            b.powerup = p;

            bubbles.Add(b);
        }

        /// <summary>
        /// Convert all defaultBalls to fastBalls
        /// </summary>
        void ActivatePowerupFastball(object sender, EventArgs e)
        {
            foreach (Ball b in ballManager.balls)
            {
                Ball new_ball = ballManager.CreateFastBall();
                new_ball.Position = b.Position;
                new_ball.Velocity = b.Velocity * 2;
                new_ball.spin = b.spin;
                new_ball.IsActive = true;

                b.IsActive = false;
            }
        }

        /// <summary>
        /// Convert all fastBalls back to defaultBalls
        /// </summary>
        void DectivatePowerupFastball(object sender, EventArgs e)
        {
            foreach (Ball b in ballManager.balls)
            {
                Ball new_ball = ballManager.CreateDefaultBall();
                new_ball.Position = b.Position;
                new_ball.Velocity = b.Velocity / 2;
                new_ball.spin = b.spin;
                new_ball.IsActive = true;

                b.IsActive = false;
            }
        }

        #endregion
    }
}