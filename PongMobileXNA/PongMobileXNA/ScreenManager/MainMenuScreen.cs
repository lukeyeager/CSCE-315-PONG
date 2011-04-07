using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PONG
{
    class MainMenuScreen : MenuScreen
    {
        #region Initialize

        public MainMenuScreen()
            : base("Main")
        {
            // Create our menu entries.
            MenuEntry campaignGameMenuEntry = new MenuEntry("Campaign");
            MenuEntry endlessGameMenuEntry = new MenuEntry("Endless");
            MenuEntry multiTouchGameMenuEntry = new MenuEntry("MultiTouch");
            MenuEntry aboutMenuEntry = new MenuEntry("About");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            campaignGameMenuEntry.Selected += CampaignGameMenuEntrySelected;
            endlessGameMenuEntry.Selected += EndlessGameMenuEntrySelected;
            multiTouchGameMenuEntry.Selected += MultiTouchGameMenuEntrySelected;
            aboutMenuEntry.Selected += AboutMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(campaignGameMenuEntry);
            MenuEntries.Add(endlessGameMenuEntry);
            MenuEntries.Add(multiTouchGameMenuEntry);
            MenuEntries.Add(aboutMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        #region Callbacks

        void CampaignGameMenuEntrySelected(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();

            ScreenManager.AddScreen(new CampaignGameScreen());
        }
        void EndlessGameMenuEntrySelected(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();

            ScreenManager.AddScreen(new EndlessGameScreen());
        }
        void MultiTouchGameMenuEntrySelected(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();

            ScreenManager.AddScreen(new MultitouchGameScreen());
        }
        void AboutMenuEntrySelected(object sender, EventArgs e)
        {
            foreach (GameScreen screen in ScreenManager.GetScreens())
                screen.ExitScreen();

            ScreenManager.AddScreen(new AboutScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

        #endregion

    }
}
