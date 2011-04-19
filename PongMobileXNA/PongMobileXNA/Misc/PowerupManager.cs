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
        public Texture2D blueBubbleTexture;
        public Texture2D greenBubbleTexture;

        Random random;
        SpriteBatch spriteBatch;
        List<PowerupBubble> bubbles;
        List<Powerup> activePowerups;
        PongGameScreen screen;

        #endregion

        #region Initialization

        public PowerupManager(ContentManager content, PongGameScreen screen, SpriteBatch spriteBatch)
        {
            multiPupTexture = content.Load<Texture2D>("Images/Powerups/Multiball");
            fastPupTexture = content.Load<Texture2D>("Images/Powerups/Fastball");
            blueBubbleTexture = content.Load<Texture2D>("Images/Powerups/BlueBubble");
            greenBubbleTexture = content.Load<Texture2D>("Images/Powerups/GreenBubble");

            random = new Random();
            bubbles = new List<PowerupBubble>();
            activePowerups = new List<Powerup>();

            this.screen = screen;
            this.spriteBatch = spriteBatch;
        }

        #endregion

        #region Update

        /// <summary>
        /// Update all active bubbles and powerups
        /// </summary>
        /// <param name="elapsed">The amount of time elapsed since last Update.</param>
        public void Update(float elapsed)
        {
            foreach (Powerup p in activePowerups)
            {
                p.LifeTime -= elapsed;
                if (p.LifeTime <= 0.0f)
                {
                    p.Deactivate(); //Hopefully this won't screw things up with multiple powerups running
                    break;  //It did, so for now we just fix one at a time
                }
            }

            int numActive = 0;
            foreach (PowerupBubble b in bubbles)
            {
                if (b.IsActive)
                {
                    b.Update(elapsed, random);
                    if (!b.IsActive)
                    {
                        if (b.IsTracked)
                        {
                            CollisionManager.RemoveObject("Bubble", b);
                            b.IsTracked = false;
                        }
                        continue;
                    }
                    if (!b.IsTracked)
                    {
                        CollisionManager.AddObject("Bubble", b);
                        b.IsTracked = true;
                    }
                    ++numActive;

                    if (b.Position.X < screen.screenLeftBound)
                    {
                        b.Position.X = screen.screenLeftBound;
                        b.Velocity.X *= -1;
                    }
                    else if (b.Position.Y + b.Diameter > screen.screenRightBound)
                    {
                        b.Position.X = screen.screenRightBound - b.Diameter;
                        b.Velocity.X *= -1;
                    }
                    //TODO: handle hitting top and bottom

                }
            }

            if (0 == numActive)
            {
                CreatePowerupFastball();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the particles.
        /// </summary>
        public void Draw()
        {
            foreach (PowerupBubble b in bubbles)
            {
                if (!b.IsActive)
                    continue;
                //Draw bubble
                spriteBatch.Draw(b.Texture, b.Position, Color.White);
                //Draw powerup icon
                spriteBatch.Draw(b.P_Texture, b.P_Position, null, Color.White, b.P_Rotation, 
                    new Vector2(b.P_Texture.Width / 2, b.P_Texture.Height / 2), 1, SpriteEffects.None, 0.0f);
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

            b.Position = CreateBubblePosition();
            return b;
        }

        public Vector2 CreateBubblePosition()
        {
            Int32 x = random.Next(screen.screenRightBound - screen.screenLeftBound);
            Int32 y = random.Next((screen.screenBottomBound - screen.screenTopBound) / 2);

            return new Vector2(x, y);
        }

        #endregion

        #region Classes

        /// <summary>
        /// An abstract class for defining powerups.
        /// </summary>
        public class Powerup
        {
            public Powerup()
            {
                LifeTime = 25.0f;
            }

            public float LifeTime;

            public void Activate()
            {
                OnActivate(this, EventArgs.Empty);
            }
            public void Deactivate()
            {
                OnDeactivate(this, EventArgs.Empty);
            }

            public event EventHandler<EventArgs> OnActivate;
            public event EventHandler<EventArgs> OnDeactivate;
        }
        
        #endregion

        #region Multiball

        public void CreatePowerupMultiball()
        {
            PowerupBubble b = CreateBubble();
            b.Texture = blueBubbleTexture;

            Powerup p = new Powerup();
            b.P_Position = new Vector2(250, 410);
            b.P_Velocity = new Vector2(0, 0);
            b.P_Texture = multiPupTexture;
            b.P_Rotation = 0f;
            b.P_Spin = 1.0f;
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
            Ball b = screen.ballManager.CreateMultiBall();
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(200, 200);

            b = screen.ballManager.CreateMultiBall();
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(-200, -200);

            activePowerups.Add((Powerup)sender);
        }


        /// <summary>
        /// Called when multiballPowerup ends, does nothing for now
        /// </summary>
        void DectivatePowerupMultiball(object sender, EventArgs e)
        {
            activePowerups.Remove((Powerup)sender);
        }

        #endregion

        #region Fastball

        public void CreatePowerupFastball()
        {
            PowerupBubble b = CreateBubble();
            b.Texture = greenBubbleTexture;

            Powerup p = new Powerup();
            b.P_Position = new Vector2(250, 410);
            b.P_Velocity = new Vector2(0, 0);
            b.P_Texture = fastPupTexture;
            b.P_Rotation = 0f;
            b.P_Spin = 1.0f;
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
            foreach (Ball b in screen.ballManager.balls)
            {
                if (b.MaxSpeed == 1.0f)
                    b.MaxSpeed = 2.0f;
            }

            screen.topPaddle.MaxSpeed = 2.0f;
            screen.bottomPaddle.MaxSpeed = 2.0f;

            activePowerups.Add((Powerup)sender);
        }

        /// <summary>
        /// Convert all fastBalls back to defaultBalls
        /// </summary>
        void DectivatePowerupFastball(object sender, EventArgs e)
        {
            foreach (Ball b in screen.ballManager.balls)
            {
                if (b.MaxSpeed == 2.0f)
                    b.MaxSpeed = 1.0f;
            }

            screen.topPaddle.MaxSpeed = 1.0f;
            screen.bottomPaddle.MaxSpeed = 1.0f;

            activePowerups.Remove((Powerup)sender);
        }

        #endregion
    }
}