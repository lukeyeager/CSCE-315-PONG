using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PONG
{
    class AboutScreen : GameScreen
    {
        public Texture2D about;

        //Screen size
        public Int32 screenLeftBound;
        public Int32 screenRightBound;
        public Int32 screenTopBound;
        public Int32 screenBottomBound;

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

            DrawAbout();
        }

        void DrawAbout()
        {
            about = ScreenManager.Game.Content.Load<Texture2D>("Images/about");
            ScreenManager.SpriteBatch.Begin();
            ScreenManager.SpriteBatch.Draw(about, new Vector2(0, 0),
                 new Color(255, 255, 255, TransitionAlpha));
            ScreenManager.SpriteBatch.End();
        }

    }
}
