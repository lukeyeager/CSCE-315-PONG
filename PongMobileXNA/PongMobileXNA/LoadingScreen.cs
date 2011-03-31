using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PongScreenManager;
using System.Threading;
using Microsoft.Xna.Framework;

namespace PongMobileXNA
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
            ScreenManager.Game.Content.Load<object>("background");
            ScreenManager.Game.Content.Load<object>("bottomPaddle");
            ScreenManager.Game.Content.Load<object>("ball");
            ScreenManager.Game.Content.Load<object>("title");
            ScreenManager.Game.Content.Load<object>("topPaddle");
            //Fonts
            ScreenManager.Game.Content.Load<object>("gamefont");
            ScreenManager.Game.Content.Load<object>("menufont");
            ScreenManager.Game.Content.Load<object>("scorefont");
            ScreenManager.Game.Content.Load<object>("titlefont");
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
