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
            ScreenManager.Game.Content.Load<object>("Images/background");
            ScreenManager.Game.Content.Load<object>("Images/defaultPaddle");
            ScreenManager.Game.Content.Load<object>("Images/defaultBall");
            ScreenManager.Game.Content.Load<object>("Images/title");
            //Fonts
            ScreenManager.Game.Content.Load<object>("Fonts/gamefont");
            ScreenManager.Game.Content.Load<object>("Fonts/menufont");
            ScreenManager.Game.Content.Load<object>("Fonts/scorefont");
            ScreenManager.Game.Content.Load<object>("Fonts/titlefont");
            //Sounds
            ScreenManager.Game.Content.Load<object>("Sounds/hitWall");

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
