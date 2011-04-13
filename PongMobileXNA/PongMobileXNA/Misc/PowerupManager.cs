using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace PONG
{
    /// <summary>
    /// An abstract class for defining powerups.
    /// </summary>
    public abstract class Powerup
    {
        public float LifeTime;
    }

    /// <summary>
    /// A class to represent powerups on the screen that haven't been activated yet.
    /// </summary>
    /// <remarks>
    /// They look like bubbles, so we're calling them PowerupBubbles
    /// </remarks>
    public class PowerupBubble
    {
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
    /// A class for managing powerups.
    /// It keeps track of the list of powerups and handles time-out events, etc.
    /// </summary>
    public class PowerupManager
    {

        #region Fields

        Random random;
        SpriteBatch spriteBatch;
        List<PowerupBubble> bubbles;
        PongGameScreen screen;

        #endregion

        #region Initialization

        public PowerupManager(PongGameScreen screen, SpriteBatch spriteBatch)
        {
            random = new Random();

            bubbles = new List<PowerupBubble>();

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
                    }
                    else
                    {
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

                    }
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
    }
}