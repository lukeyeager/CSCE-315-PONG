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

namespace PONG
{

    /// <summary>
    /// Abstract class for all Pong game screens
    /// Other classes derive from this one, overriding the necessary functions
    /// </summary>
    public abstract class PongGameScreen : GameScreen
    {
        #region Fields
        //Textures (images)
        public Texture2D backgroundTexture;
        public Texture2D defaultBallTexture;
        public Texture2D defaultPaddleTexture;

        //Sound Effects
        public SoundEffect hitWallSound;

        // Input related variables
        public AccelerometerReadingEventArgs accelState;
        public Accelerometer Accelerometer;
        public TouchLocation bottomPaddleTouch;
        public Int32 bottomPaddleTouchId;
        public List<TouchLocation> lastTouchInput;
        public List<Keys> lastKeyInput;
        public float lastAccelInput;

        //Data Members
        public List<Ball> balls;
        public Paddle topPaddle;
        public Paddle bottomPaddle;

        //Screen size
        public Int32 screenLeftBound;
        public Int32 screenRightBound;
        public Int32 screenTopBound;
        public Int32 screenBottomBound;

        //Settings
        public const float maxPaddleSpeed = 8f;
        public const float paddleFriction = 0.8f;
        public const float paddleBounceFriction = 0.0f;

        #endregion

        #region Initialization

        public PongGameScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
            screenLeftBound = 0;
            screenRightBound = 480;
            screenTopBound = 0;
            screenBottomBound = 800;

            Accelerometer = new Accelerometer();
            if (Accelerometer.State == SensorState.Ready)
            {
                Accelerometer.ReadingChanged += (s, e) =>
                {
                    accelState = e;
                };
                Accelerometer.Start();
            }
            lastAccelInput = 0f;
            bottomPaddleTouch = new TouchLocation();
            bottomPaddleTouchId = -1;
            lastTouchInput = new List<TouchLocation>();
            lastKeyInput = new List<Keys>();

            balls = new List<Ball>();
            bottomPaddle = new DefaultPaddle(new Vector2(200, 750), new Vector2());
            topPaddle = new DefaultPaddle(new Vector2(200, 0), new Vector2());
        }

        public override void LoadContent()
        {
            //Load Textures
            defaultPaddleTexture = ScreenManager.Game.Content.Load<Texture2D>("Images/defaultPaddle");
            defaultBallTexture = ScreenManager.Game.Content.Load<Texture2D>("Images/defaultBall");
            backgroundTexture = ScreenManager.Game.Content.Load<Texture2D>("Images/background");

            //Load Sound Effects
            hitWallSound = ScreenManager.Game.Content.Load<SoundEffect>("Sounds/hitWall");

            //particles = new ParticleSystem(ScreenManager.Game.Content, ScreenManager.SpriteBatch);

            base.LoadContent();

            Start();
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
            b.UpdateShape();
            b.spin = 0;
            bottomPaddle.Texture = defaultPaddleTexture;
            bottomPaddle.UpdateShape();
            topPaddle.Texture = defaultPaddleTexture;
            topPaddle.UpdateShape();
            CollisionManager.AddObject("Paddle", bottomPaddle);
            CollisionManager.AddObject("Paddle", topPaddle);
            CollisionManager.AddObject("Ball", b);
            RegisterCallbackFunctions();
            //Update game statistics
        }

        #endregion

        #region Finalization

        public override void UnloadContent()
        {
            //particles = null;
            base.UnloadContent();
        }

        private void finishCurrentGame()
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();
            ScreenManager.AddScreen(new BackgroundScreen());
            ScreenManager.AddScreen(new MainMenuScreen());
        }

        #endregion

        #region Update

        /// <summary>
        /// Runs one frame of update for the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime,
            bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            UpdateBottomPaddle(elapsed);
            UpdateTopPaddle(elapsed);
            //UpdateBottomPaddle(elapsed);
            UpdateBalls(elapsed);
            CheckHits();
            //particles.Update(elapsed);
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
        
        /// <summary>
        /// Moves all of the balls
        /// </summary>
        /// <param name="elapsed"></param>
        void UpdateBalls(float elapsed)
        {
            foreach (Ball ball in balls)
            {
                if (ball.IsActive == false) //Ignore inactive balls
                    continue;

                ball.Update(elapsed);
                ////TODO: check for collide with topPaddle
                //if (ball.Position.Y < 40)
                //{
                //    ball.Velocity.Y = -ball.Velocity.Y;
                //}
                ////TODO: check for collide with bottomPaddle
                //else if (ball.Position.Y > 730) 
                //{
                //    ball.Velocity.Y = -ball.Velocity.Y;
                //}
                if (ball.Position.Y < 30)
                {
                }

                if (ball.Position.Y < screenTopBound || ball.Position.Y > screenBottomBound)
                    ball.IsActive = false;

                //TODO: check for collide with left wall
                if (ball.Position.X < screenLeftBound)
                {
                    ball.Velocity.X = -ball.Velocity.X;
                }
                //TODO: check for collide with right wall
                else if (ball.Position.X + ball.Texture.Width > screenRightBound)
                {
                    ball.Velocity.X = -ball.Velocity.X;
                }
            }
        }

        /// <summary>
        /// Update the bottom paddle
        /// </summary>
        /// <param name="elapsed"></param>
        void UpdateBottomPaddle(float elapsed)
        {
            //Update from touch input
            if (bottomPaddleTouchId != -1)
            {
                var newX = bottomPaddleTouch.Position.X + 5 - bottomPaddle.Width / 2;
                bottomPaddle.Velocity.X = (newX - bottomPaddle.Position.X) / ( 128.0f * elapsed);
            }
            //Update from key input
            else if (lastKeyInput.Count > 0) 
            {
                if (lastKeyInput.Contains(Keys.Left) && !lastKeyInput.Contains(Keys.Right))
                {
                    bottomPaddle.Velocity.X += -1.0f;
                }
                else if (lastKeyInput.Contains(Keys.Right) && !lastKeyInput.Contains(Keys.Left))
                {
                    bottomPaddle.Velocity.X += 1.0f;
                }
            }

            if (bottomPaddle.Velocity.X > maxPaddleSpeed)
                bottomPaddle.Velocity.X = maxPaddleSpeed;
            else if (bottomPaddle.Velocity.X < -maxPaddleSpeed)
                bottomPaddle.Velocity.X = -maxPaddleSpeed;

            // Move the bottomPaddle
            bottomPaddle.Position += bottomPaddle.Velocity * 128.0f * elapsed;
            bottomPaddle.Velocity *= paddleFriction;

            if (bottomPaddle.Position.X <= screenLeftBound - bottomPaddle.Width/2)
            {
                bottomPaddle.Position = new Vector2(screenLeftBound - bottomPaddle.Width / 2, bottomPaddle.Position.Y);
                bottomPaddle.Velocity.X *= -paddleBounceFriction;
            }
            if (bottomPaddle.Position.X + bottomPaddle.Width >= screenRightBound + bottomPaddle.Width/2)
            {
                bottomPaddle.Position = new Vector2(screenRightBound - bottomPaddle.Width + bottomPaddle.Width / 2, bottomPaddle.Position.Y);
                bottomPaddle.Velocity *= -paddleBounceFriction;
            }
            bottomPaddle.UpdateShape();
        }

        /// <summary>
        /// Update the top paddle
        /// Each derived class should define this for themselves
        /// </summary>
        /// <param name="elapsed"></param>
        public virtual void UpdateTopPaddle(float elapsed)
        {
            topPaddle.Position += topPaddle.Velocity * 128.0f * elapsed;

            if (topPaddle.Position.X <= screenLeftBound - topPaddle.Width/2)
            {
                topPaddle.Position = new Vector2(screenLeftBound - topPaddle.Width/2, topPaddle.Position.Y);
                topPaddle.Velocity = new Vector2(0, 0);
            }
            if (topPaddle.Position.X + topPaddle.Width >= screenRightBound + topPaddle.Width/2)
            {
                topPaddle.Position = new Vector2(screenRightBound - topPaddle.Width + topPaddle.Width/2, topPaddle.Position.Y);
                topPaddle.Velocity = new Vector2(0, 0);
            }
            topPaddle.UpdateShape();
        }

        #endregion

        #region Draw


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

        #endregion

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


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                foreach (GameScreen screen in ScreenManager.GetScreens())
                    screen.ExitScreen();

                CollisionManager.ClearAll();
                ScreenManager.AddScreen(new BackgroundScreen());
                ScreenManager.AddScreen(new MainMenuScreen());
            }

            //if (input.IsPauseGame(null))
            //{
            //    PauseCurrentGame();
            //}

            /// Read Touchscreen input
            /// For now, add all "Pressed" and "Moved" events to lastPressInput
            if (input.TouchState.Count > 0)
            {
                lastTouchInput = new List<TouchLocation>();
                foreach (var touch in input.TouchState)
                {
                    switch (touch.State)
                    {
                        case TouchLocationState.Pressed:
                            if (bottomPaddleTouchId == -1 &&
                                touch.Position.Y > bottomPaddle.Position.Y &&
                                touch.Position.X > bottomPaddle.Position.X - 20 &&
                                touch.Position.X < bottomPaddle.Position.X + bottomPaddle.Width + 20)
                            {
                                bottomPaddleTouch = touch;
                                bottomPaddleTouchId = touch.Id;
                            }
                            else
                            {
                                lastTouchInput.Add(touch);
                            }
                            break;
                        case TouchLocationState.Moved:
                            if (touch.Id == bottomPaddleTouchId)
                            {
                                bottomPaddleTouch = touch;
                            }
                            else
                            {
                                lastTouchInput.Add(touch);
                            }
                            break;
                        case TouchLocationState.Released:
                            if (touch.Id == bottomPaddleTouchId)
                            {
                                bottomPaddleTouchId = -1;
                            }
                            else
                            {
                                lastTouchInput.Add(touch);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            /// Read Accelerometer input
            /// (use it to update powerups)
            float movement = 0.0f;
            if (accelState != null)
            {
                if (Math.Abs(accelState.X) > 0.10f)
                {
                    if (accelState.X > 0.0f)
                        movement = 10.0f;
                    else
                        movement = -10.0f;
                }
            }
            // TODO: use this movement to update powerups

            /// Read keyboard input
            /// (use it for debugging)
            KeyboardState keyState = Keyboard.GetState();
            lastKeyInput = new List<Keys>();
            if (keyState.IsKeyDown(Keys.Left))
            {
                lastKeyInput.Add(Keys.Left);
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                lastKeyInput.Add(Keys.Right);
            }

        }
        #endregion

        #region Private functions

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
        /// Performs all ball collision detection.  Also handles game logic
        /// when a hit occurs, such as killing something, adding score, ending the game, etc.
        /// </summary>
        void CheckHits()
        {
            CollisionManager.HandleCollisions();
        }

        #endregion

        #region Collision detection callbacks

        public void BounceBallOffPaddle(PongObject param1, PongObject param2)
        {
            Ball b = (Ball)param1;
            Paddle p = (Paddle)param2;
            //TODO: incorporate spin and also whether or not we hit the rounded part
            b.Velocity.Y = -b.Velocity.Y;
        }

        private void RegisterCallbackFunctions()
        {
            CollisionManager.RegisterCallback("Ball", "Paddle", BounceBallOffPaddle);
        }

        #endregion
    }
}
