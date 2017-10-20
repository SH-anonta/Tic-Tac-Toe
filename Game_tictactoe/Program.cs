using System;
using Utility;

namespace Game_tictactoe
{
    class Launcher{
        static void Main(string[] args){
            Game g = new Game();
            //g.startGame();

            Board b = new Board();
            b.makeMove(0, 0, Board.CROSS);
            b.makeMove(0, 1, Board.CIRCLE);

            char[,] bo = b.getBoard();

            bo[1, 1] = 'X';
            print(b);
        }

        // convenience funcitons 
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }
    }

    class Game {
        private GameState current_state;

        public static GameState splash_screen;
        public static GameState main_menue;
        public static GameState confirm_exit;

        public Game() {
            splash_screen = new SplashScreen(this);
            main_menue = new MainMenue();
            confirm_exit = new ConfirmExit(this);
        }

        public void startGame() {
            current_state = splash_screen;
            GameState next;

            while (true) {
                next = current_state.onStart();     // run the current state and get the next state to run
                current_state = next;               // assign the next state to be run in next iteration
            }
        }

        //public void setState(GameState new_state){
        //    current_state = new_state;
        //    current_state.onStart();
        //}

    }

    abstract class GameState {
        // this function gets called when the program enters a game state
        // it returns the next state the game will go into
        public abstract GameState onStart();
    }

    // This is the first state thegame enters. 
    // It shows the splash screen for WAIT_TIME seconds then Main menue is shown
    class SplashScreen : GameState
    {
        private string SPLASH_SCREEN;
        private int WAIT_TIME;      // in milliseconds
        private Game game;

        public SplashScreen(Game game) {
            this.game = game;
            WAIT_TIME = 2000; // 3 seconds

            SPLASH_SCREEN = @"
  _______   _____    _____            _______               _____            _______    ____    ______ 
 |__   __| |_   _|  / ____|          |__   __|     /\      / ____|          |__   __|  / __ \  |  ____|
    | |      | |   | |       ______     | |       /  \    | |       ______     | |    | |  | | | |__   
    | |      | |   | |      |______|    | |      / /\ \   | |      |______|    | |    | |  | | |  __|  
    | |     _| |_  | |____              | |     / ____ \  | |____              | |    | |__| | | |____ 
    |_|    |_____|  \_____|             |_|    /_/    \_\  \_____|             |_|     \____/  |______|

";
        }

        override public GameState onStart() {
            Console.WriteLine(SPLASH_SCREEN);
            Console.WriteLine("\n");

            System.Threading.Thread.Sleep(WAIT_TIME);
            Console.Clear();

            return Game.main_menue;
        }
    }

    class MainMenue : GameState
    {
        private ConsoleMenue menue;

        public MainMenue()
        {
            string[] options = { "Play PVC", "Play PVP", "Credits", "Exit" };
            menue = new ConsoleMenue("Main Menue", options);
        }

        override public GameState onStart()
        {
            int selected = menue.getUserSelection();

            if (selected == 3)
                return Game.confirm_exit;

            // if someshow some invilid was returned by menue.getUserSelection()
            return Game.main_menue;
        }
    }

    class ConfirmExit : GameState
    {
        Game game;
        private ConsoleMenue menue;

        public ConfirmExit(Game game) {
            string[] options = { "Yes", "No" };

            menue = new ConsoleMenue("Exit Game?", options);
        }

        override public GameState onStart()
        {
            int selected = menue.getUserSelection();

            // user selected yes
            if (selected == 0) {
                Environment.Exit(0);
            }

            return Game.main_menue;
        }
    }


    class PVP : GameState
    {
        override public GameState onStart() {
            return Game.main_menue;
        }
    }

    class PlayGame{
        
    }

    abstract class Player{
        abstract public Tuple<int, int> makeMove(Board board);
    }

    class HumanPlayer: Player
    {
        public override Tuple<int, int> makeMove(Board board)
        {
            var move= new Tuple<int, int>(0,0);

            return move;
        }

    }
}


