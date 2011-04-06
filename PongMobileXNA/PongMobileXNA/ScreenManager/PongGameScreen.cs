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

using PongClasses;

namespace PongScreens
{
    /// <summary>
    /// Represents a Paddle
    /// </summary>
    public class Paddle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Texture2D Texture;
    }

    /// <summary>
    /// Abstract class for all Pong game screens
    /// Other classes derive from this one, overriding the necessary functions
    /// </summary>
    public abstract class PongGameScreen : GameScreen
    {
        //Textures (images)
        Texture2D backgroundTexture;
        Texture2D defaultBallTexture;

        //Sound Effects
        SoundEffect hitWallSound;

        //Input Members
        AccelerometerReadingEventArgs accelState;
        TouchCollection touchState;
        Accelerometer Accelerometer;

        //Data Members
        List<Ball> balls;
        Paddle topPaddle;
        Paddle bottomPaddle;

        public PongGameScreen()
        {
            //TransitionOnTime = TimeSpan.FromSeconds(0.0);
            //TransitionOffTime = TimeSpan.FromSeconds(0.0);
            Accelerometer = new Accelerometer();
            if (Accelerometer.State == SensorState.Ready)
            {
                Accelerometer.ReadingChanged += (s, e) =>
                {
                    accelState = e;
                };
                Accelerometer.Start();
            }
            balls = new List<Ball>();

            bottomPaddle = new Paddle();
            bottomPaddle.Velocity = new Vector2();
            bottomPaddle.Position = new Vector2(200, 750);

            topPaddle = new Paddle();
            topPaddle.Velocity = new Vector2();
            topPaddle.Position = new Vector2(200, 0);
        }

        public override void LoadContent()
        {
            //Load Textures
            defaultBallTexture = ScreenManager.Game.Content.Load<Texture2D>("Images/defaultBall");
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("Images/background");
            bottomPaddle.Texture = ScreenManager.Game.Content.Load<Texture2D>("Images/bottomPaddle");
            topPaddle.Texture = ScreenManager.Game.Content.Load<Texture2D>("Images/topPaddle");

            //Load Sound Effects
            hitWallSound = ScreenManager.Game.Content.Load<SoundEffect>("Sounds/hitWall");

            //particles = new ParticleSystem(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            base.LoadContent();

            Start();
        }

        public override void UnloadContent()
        {
            //particles = null;
            base.UnloadContent();
        }

        /// <summary>
        /// Runs one frame of update for the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime,
            bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the bottomPaddle
            bottomPaddle.Position += bottomPaddle.Velocity * 128.0f * elapsed;
            if (bottomPaddle.Position.X <= 0.0f)
                bottomPaddle.Position = new Vector2(0.0f, bottomPaddle.Position.Y);
            if (bottomPaddle.Position.X + bottomPaddle.Texture.Width >= 800)
                bottomPaddle.Position = new Vector2(800 - bottomPaddle.Texture.Width, bottomPaddle.Position.Y);

            UpdateBalls(elapsed);
            HandleAI();
            CheckHits();
            //particles.Update(elapsed);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// Draw the game world, effects, and HUD
        /// </summary>
        /// <param name="gameTime">The elapsed time since last Draw</param>
        public override void Draw(GameTime gameTime)
        {
            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            ScreenManager.SpriteBatch.Begin();
            DrawBackground();
            DrawBalls();
            DrawBottomPaddle();
            DrawTopPaddle();
            //particles.Draw();
            //DrawHud();
            ScreenManager.SpriteBatch.End();
        }

        /// <summary>
        /// Draws the background
        /// </summary>
        void DrawBackground()
        {
            ScreenManager.SpriteBatch.Draw(backgroundTexture, new Vector2(0, 0),
                 new Color(255, 255, 255, TransitionAlpha));
        }
        /// <summary>
        /// Draws the bottom paddle
        /// </summary>
        void DrawBottomPaddle()
        {
            ScreenManager.SpriteBatch.Draw(bottomPaddle.Texture, bottomPaddle.Position, Color.White);
        }

        /// <summary>
        /// Draws the top paddle
        /// </summary>
        void DrawTopPaddle()
        {
            ScreenManager.SpriteBatch.Draw(topPaddle.Texture, topPaddle.Position, Color.White);
        }

        /// <summary>
        /// Draws all of the balls
        /// </summary>
        void DrawBalls()
        {
            for (int i = 0; i < balls.Count; ++i)
            {
                ScreenManager.SpriteBatch.Draw(balls[i].Texture, balls[i].Position, Color.White);
            }
        }

        /// <summary>
        /// Draw the hud, which consists of the score elements and the GAME OVER tag.
        /// </summary>
        void DrawHud()
        {
            //TODO: draw score?
        }

        private void finishCurrentGame()
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        /// <summary>
        /// Returns an instance of a ball
        /// </summary>
        /// <returns>A ball ready to place into the world.</returns>
        DefaultBall CreateBall()
        {
            DefaultBall b = new DefaultBall();
            b.Texture = defaultBallTexture;
            balls.Add(b);
            return b;
        }

        /// <summary>
        /// Moves all of the balls
        /// </summary>
        /// <param name="elapsed"></param>
        void UpdateBalls(float elapsed)
        {
            for (int i = 0; i < balls.Count; ++i)
            {
                if (balls[i].IsActive == false) //Ignore inactive balls
                    continue;

                balls[i].Position += balls[i].Velocity * elapsed;
                if (balls[i].Position.Y < bottomPaddle.Texture.Height)
                {
                    balls[i].Velocity.Y = -balls[i].Velocity.Y;
                }
                else if (balls[i].Position.Y > 800 - topPaddle.Texture.Height - balls[i].Texture.Height)
                {
                    balls[i].Velocity.Y = -balls[i].Velocity.Y;
                }
                if (balls[i].Position.X < 0)
                {
                    balls[i].Velocity.X = -balls[i].Velocity.X;
                }
                else if (balls[i].Position.X > 480 - balls[i].Texture.Width)
                {
                    balls[i].Velocity.X = -balls[i].Velocity.X;
                }
            }
        }
        /// <summary>
        /// Performs all ball collision detection.  Also handles game logic
        /// when a hit occurs, such as killing something, adding score, ending the game, etc.
        /// </summary>
        void CheckHits()
        {
            //TODO: all this
        }

        /// <summary>
        /// Starts a new game session, setting all game states to initial values.
        /// </summary>
        void Start()
        {
            DefaultBall b = CreateBall();
            b.Position = new Vector2(240, 400);
            b.Velocity = new Vector2(200, 200);
            b.IsActive = true;
            b.spin = 0;
            //Update game statistics
        }

        #region Input
        /// <summary>
        /// Input helper method provided by GameScreen.  Packages up the various input
        /// values for ease of use
        /// </summary>
        /// <param name="input">The state of the gamepads</param>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            touchState = TouchPanel.GetState();
            bool buttonTouched = false;
            //interpret touch screen presses
            foreach (TouchLocation location in touchState)
            {
                switch (location.State)
                {
                    case TouchLocationState.Pressed:
                        buttonTouched = true;
                        break;
                    case TouchLocationState.Moved:
                        break;
                    case TouchLocationState.Released:
                        break;
                }
            }
            float movement = 0.0f;
            if (accelState != null)
            {
                if (Math.Abs(accelState.X) > 0.10f)
                {
                    if (accelState.X > 0.0f)
                        movement = 1.0f;
                    else
                        movement = -1.0f;
                }
            }
            bottomPaddle.Velocity.X = movement;

            //This section handles tank movement.  We only allow one "movement" action
            //to occur at once so that touchpad devices don't get double hits.
            KeyboardState keyState = Keyboard.GetState();
            if (input.CurrentGamePadStates[0].DPad.Left == ButtonState.Pressed || keyState.IsKeyDown(Keys.Left))
            {
                bottomPaddle.Velocity.X = -1.0f;
            }
            else if (input.CurrentGamePadStates[0].DPad.Right == ButtonState.Pressed || keyState.IsKeyDown(Keys.Right))
            {
                bottomPaddle.Velocity.X = 1.0f;
            }
            else
            {
                bottomPaddle.Velocity.X = MathHelper.Min(input.CurrentGamePadStates[0].ThumbSticks.Left.X * 2.0f, 1.0f);
            }
        }
        #endregion

        #region AI
        /// <summary>
        /// Handles movement of the top paddle in single player
        /// </summary>
        /// <param name="input">The state of the gamepads</param>       
        public void HandleAI()
        {
            int MAX_SPEED = 8;
            //I want the paddle to pursue the ball once it's gone past
            //the screen's halfway point, before that the paddle will
            //just move to the center
            float ballPursuit = balls[0].Position.X;
            if (balls[0].Position.Y < 500) //arbitrary, what's half the screen?
            {
                if (topPaddle.Position.X > balls[0].Position.X) //is the ball on the right?
                {
                    //TODO : Fix bounds of field
                    /*if (topPaddle.Position.X + MAX_SPEED + topPaddle.Texture.Width > 480)
                        topPaddle.Position.X = 480 - topPaddle.Texture.Width;*/
                    if (topPaddle.Position.X - balls[0].Position.X < MAX_SPEED)
                        topPaddle.Position.X -= topPaddle.Position.X - balls[0].Position.X;
                    else
                        topPaddle.Position.X -= MAX_SPEED;
                }
                else if (topPaddle.Position.X < balls[0].Position.X)//the ball must be on the left
                {
                    //TODO : Fix bounds of field
                    /*if (topPaddle.Position.X - MAX_SPEED < 0)
                        topPaddle.Position.X = 0;*/
                    if (balls[0].Position.X - topPaddle.Position.X < MAX_SPEED)
                        topPaddle.Position.X += balls[0].Position.X - topPaddle.Position.X;
                    else
                        topPaddle.Position.X += MAX_SPEED;
                }
            }
            else
            {
                if (topPaddle.Position.X > 240 - topPaddle.Texture.Width/2)
                    topPaddle.Position.X -= 5;
                if (topPaddle.Position.X < 240 - topPaddle.Texture.Width/2)
                    topPaddle.Position.X += 5;
            }
        }
        #endregion

    }
}
