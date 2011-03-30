using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PongScreenManager;

namespace PongMobileXNA
{
    class MainMenuScreen : MenuScreen
    {
        public MainMenuScreen()
            : base("Main")
        {
            // Create our menu entries.
            MenuEntry singleGameMenuEntry = new MenuEntry("SinglePlayer");
            MenuEntry multiTouchGameMenuEntry = new MenuEntry("MultiTouch");
            //MenuEntry multiPhoneGameMenuEntry = new MenuEntry("MultiPhone");
            MenuEntry aboutMenuEntry = new MenuEntry("About");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            // Hook up menu event handlers.
            singleGameMenuEntry.Selected += SingleGameMenuEntrySelected;
            multiTouchGameMenuEntry.Selected += MultiTouchGameMenuEntrySelected;
            //multiPhoneGameMenuEntry.Selected += MultiPhoneGameMenuEntrySelected;
            aboutMenuEntry.Selected += AboutMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            // Add entries to the menu.
            MenuEntries.Add(singleGameMenuEntry);
            MenuEntries.Add(multiTouchGameMenuEntry);
            //MenuEntries.Add(multiPhoneGameMenuEntry);
            MenuEntries.Add(aboutMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        void SingleGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen());
        }
        void MultiTouchGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen());
        }
        void MultiPhoneGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new GameplayScreen());
        }
        void AboutMenuEntrySelected(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new AboutScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

    }
}
