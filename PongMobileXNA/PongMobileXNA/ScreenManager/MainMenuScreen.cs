using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PongScreens.GameScreens;

namespace PongScreens
{
    class MainMenuScreen : MenuScreen
    {
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

        void CampaignGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new CampaignGameScreen());
        }
        void EndlessGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new EndlessGameScreen());
        }
        void MultiTouchGameMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MultitouchGameScreen());
        }
        void AboutMenuEntrySelected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new AboutScreen());
        }

        protected override void OnCancel()
        {
            ScreenManager.Game.Exit();
        }

    }
}
