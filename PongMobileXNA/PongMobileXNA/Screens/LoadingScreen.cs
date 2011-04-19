using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;
using Microsoft.Xna.Framework;

namespace PONG
{
    class LoadingScreen : GameScreen
    {
        private Thread backgroundThread;

        public LoadingScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);
        }

        void BackgroundLoadContent()
        {
            //Images
            ScreenManager.Game.Content.Load<object>("Images/about");
            ScreenManager.Game.Content.Load<object>("Images/background");
            ScreenManager.Game.Content.Load<object>("Images/defaultTopPaddle");
            ScreenManager.Game.Content.Load<object>("Images/defaultBottomPaddle");
            ScreenManager.Game.Content.Load<object>("Images/defaultBall35px");
            //ScreenManager.Game.Content.Load<object>("Images/fastBall");
            ScreenManager.Game.Content.Load<object>("Images/title");
            ScreenManager.Game.Content.Load<object>("Images/defaultCollisionEffect");

            ScreenManager.Game.Content.Load<object>("Images/Powerups/Bubble");
            ScreenManager.Game.Content.Load<object>("Images/Powerups/BlueBubble");
            ScreenManager.Game.Content.Load<object>("Images/Powerups/GreenBubble");
            ScreenManager.Game.Content.Load<object>("Images/Powerups/RedBubble");
            ScreenManager.Game.Content.Load<object>("Images/Powerups/Fastball");
            ScreenManager.Game.Content.Load<object>("Images/Powerups/Multiball");

            //Fonts
            ScreenManager.Game.Content.Load<object>("Fonts/gamefont");
            ScreenManager.Game.Content.Load<object>("Fonts/menufont");
            ScreenManager.Game.Content.Load<object>("Fonts/scorefont");
            ScreenManager.Game.Content.Load<object>("Fonts/titlefont");

            //Sounds
            //ScreenManager.Game.Content.Load<object>("Sounds/hitWall");

        }

        public override void LoadContent()
        {
            if (backgroundThread == null)
            {
                backgroundThread = new Thread(BackgroundLoadContent);
                backgroundThread.Start();
            }
            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (backgroundThread != null && backgroundThread.Join(10))
            {
                backgroundThread = null;
                this.ExitScreen();
                ScreenManager.AddScreen(new MainMenuScreen());
                ScreenManager.Game.ResetElapsedTime();
            }
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }
    }
}
