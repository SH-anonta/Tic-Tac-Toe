using System;
using System.Collections.Generic;
using System.Text;
using Utility;
using Game_tictactoe;

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
                Console.Clear();
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
        private static GameView help;

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

        public static GameView help_view {
            get {
                return help ?? (help = new HelpView());
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
            WAIT_TIME = 1000; // 2 seconds

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
            string[] options = { "Play PVC", "Play PVP", "Help", "Exit" };
            menue = new ConsoleMenue("Main Menue", options);
        }

        override public GameView onStart()
        {
            int selected = menue.getUserSelection();
            GameView next_view = null;

            if (selected == 0) {
                // todo access through property
                next_view = new PVC_Difficulty();
            }
            else if(selected == 1) {
                Player p1 = new HumanPlayer("Player1", BoardSymbol.Cross);
                Player p2 = new HumanPlayer("Player2", BoardSymbol.Circle);
                next_view = new PlayGame(new GameEngine(p1,p2));
            }
            else if (selected == 2){
                next_view = GameView.help_view;
            }
            else if (selected == 3){
                next_view = GameView.confirm_exit_view;
            }

            // if someshow some invilid was returned by menue.getUserSelection()
            return next_view;
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

    class PVC_Difficulty: GameView{
        override public GameView onStart(){
            string[] options = { "Easy", "Hard", "Back"};

            ConsoleMenue menue = new ConsoleMenue("Pick Dificulty", options);
            int selected = menue.getUserSelection();

            Player p1 = new HumanPlayer("Player1", BoardSymbol.Cross);
            Player p2 = null;

            if (selected == 0) {
                p2 = new DumbAI("Player2", BoardSymbol.Circle);
            }
            else if (selected == 1) {
                p2 = new SmartAI("Player2", BoardSymbol.Circle);
            }
            else if (selected == 2) {
                return GameView.main_menue_view;
            }
            
            GameEngine game = new GameEngine(p1,p2);

            return new PlayGame(game);
        }
    }


    class PlayGame : GameView{
        GameEngine game;
        public PlayGame(GameEngine eng) {
            game = eng;
        }
        override public GameView onStart(){
            game.startGame();
            return GameView.main_menue_view;
        }
    }

    class HelpView : GameView {
        const string SCREEN_CONTENT= @"
Help screen (press Esc to go back)
--------------------------------------------
Game controls:
Use the numpad to enter X or O into a cell

    7 8 9   
    4 5 6
    1 2 3

Pressing 7 will enter a X or O into cell 0,0
Pressing 8 will enter a X or O into cell 0,1
and so forth

Incase you dont have a numpad, use the following heys to enter X or O into a cell

    Q W E
    A S D
    Z X C

Pressing Q will enter a X or O into cell 0,0
Pressing W will enter a X or O into cell 0,1
and so forth
(Case doesn't matter)
";
        public HelpView() {

        }

        override public GameView onStart(){
            Console.WriteLine(SCREEN_CONTENT);
            while (true) {
                ConsoleKey pressed_key= catchKeyPress();
                if(pressed_key == ConsoleKey.Escape)
                    break;
            }
            return GameView.main_menue_view;
        }


        private static System.ConsoleKey catchKeyPress() {
            // wait for the user to press a key
            while (!Console.KeyAvailable) {
                // do nothing
            }

            // get the pressed key and take necessary steps
            return Console.ReadKey().Key;
        }
    }
}
