using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace Game_views
{

    class ViewLauncher
    {
        private GameView current_view;

        public ViewLauncher()
        {
        }

        public void launchView()
        {
            current_view = GameView.splash_screen_view;
            GameView next;

            while (true) {
                next = current_view.onStart();     // run the current state and get the next state to run
                current_view = next;               // assign the next state to be run in next iteration
            }
        }

    }

    class GameView
    {
        private static GameView splash_screen;
        private static GameView main_menue;
        private static GameView confirm_exit;

        public static GameView splash_screen_view
        {
            get {
                return splash_screen ?? (splash_screen = new SplashScreen());
            }
        }

        public static GameView main_menue_view
        {
            get {
                return main_menue ?? (main_menue = new MainMenue());
            }
        }

        public static GameView confirm_exit_view
        {
            get {
                return confirm_exit ?? (confirm_exit = new ConfirmExit());
            }
        }

        // this function gets called when the program enters a game state/view
        // it returns the next state the game will go into
        virtual public GameView onStart()
        {
            throw new NotImplementedException();
        }
    }

    // This is the first state thegame enters. 
    // It shows the splash screen for WAIT_TIME seconds then Main menue is shown
    class SplashScreen : GameView
    {
        private string SPLASH_SCREEN;
        private int WAIT_TIME;      // in milliseconds

        public SplashScreen()
        {
            WAIT_TIME = 2000; // 2 seconds

            SPLASH_SCREEN = @"
  _______   _____    _____            _______               _____            _______    ____    ______ 
 |__   __| |_   _|  / ____|          |__   __|     /\      / ____|          |__   __|  / __ \  |  ____|
    | |      | |   | |       ______     | |       /  \    | |       ______     | |    | |  | | | |__   
    | |      | |   | |      |______|    | |      / /\ \   | |      |______|    | |    | |  | | |  __|  
    | |     _| |_  | |____              | |     / ____ \  | |____              | |    | |__| | | |____ 
    |_|    |_____|  \_____|             |_|    /_/    \_\  \_____|             |_|     \____/  |______|

";
        }

        override public GameView onStart()
        {
            Console.WriteLine(SPLASH_SCREEN);
            Console.WriteLine("\n");

            System.Threading.Thread.Sleep(WAIT_TIME);
            Console.Clear();

            return GameView.main_menue_view;
        }
    }

    class MainMenue : GameView
    {
        private ConsoleMenue menue;

        public MainMenue()
        {
            string[] options = { "Play PVC", "Play PVP", "Credits", "Exit" };
            menue = new ConsoleMenue("Main Menue", options);
        }

        override public GameView onStart()
        {
            int selected = menue.getUserSelection();

            if (selected == 3)
                return GameView.confirm_exit_view;

            // if someshow some invilid was returned by menue.getUserSelection()
            return GameView.main_menue_view;
        }
    }

    class ConfirmExit : GameView
    {
        private ConsoleMenue menue;

        public ConfirmExit()
        {
            string[] options = { "Yes", "No" };

            menue = new ConsoleMenue("Exit Game?", options);
        }

        override public GameView onStart()
        {
            int selected = menue.getUserSelection();

            // user selected yes
            if (selected == 0) {
                Environment.Exit(0);
            }

            // user selected no
            return GameView.main_menue_view;
        }
    }


    class PlayGame : GameView
    {
        override public GameView onStart()
        {
            return GameView.main_menue_view;
        }
    }

}
