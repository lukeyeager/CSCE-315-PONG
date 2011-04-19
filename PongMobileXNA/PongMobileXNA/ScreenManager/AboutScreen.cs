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

namespace PONG
{
    class AboutScreen : GameScreen
    {

        #region Fields

        //Textures
        public Texture2D about;

        //Screen size
        public Int32 screenLeftBound;
        public Int32 screenRightBound;
        public Int32 screenTopBound;
        public Int32 screenBottomBound;

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public AboutScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
            screenLeftBound = 0;
            screenRightBound = 480;
            screenTopBound = 0;
            screenBottomBound = 800;
        }

        public override void LoadContent()
        {
            //Load Textures
            about = ScreenManager.Game.Content.Load<Texture2D>("Images/about");
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draw the game world, effects, and HUD
        /// </summary>
        /// <param name="gameTime">The elapsed time since last Draw</param>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(about, new Vector2(0, 0),
                 new Color(255, 255, 255, TransitionAlpha));
            ScreenManager.SpriteBatch.End();
            AudioManager.PlaySound("Intro");
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
        }

        #endregion

    }
}
