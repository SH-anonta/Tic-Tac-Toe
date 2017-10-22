using System;
using Utility;
using Game_views;

namespace Game_tictactoe
{
    class Launcher{
        static void Main(string[] args){
            ViewLauncher g = new ViewLauncher();
             g.startGame();

            Board b = new Board();

            b.makeMove(0, 2, Board.CIRCLE);
            b.makeMove(1, 1, Board.CIRCLE);
            b.makeMove(2, 0, Board.CIRCLE);

            print(b);
            print(b.winningState());
            print(b.countEmptyCells());
        }

        // convenience funcitons 
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }
    }

    class GameEngine{
        private Player player1;
        private Player player2;

        GameEngine(Player player1, Player player2){
            this.player1 = player1;
            this.player2 = player2;
        }

        public string startGame(){
            const int MAX_TURNS= 9;
            Player current_turn= player1;
            Player next_turn = player1;


            for (int i= 0; i< MAX_TURNS; i++) {

            }

            return "";
        }

        private void swap(ref Object a, ref Object b)
        {
            Object temp = a;
            a = b;
            b = temp;
        }
    }

    class Player{
        private char symbol;
        private string player_name;

        public Player(string name, char symbol){
            this.player_name = name;
            this.symbol = symbol;
        }

        public string getPlayerName(){
            return player_name;
        }
        virtual public Tuple<int, int> makeMove(Board board) {
            throw new NotImplementedException();
        }

    }

    class HumanPlayer: Player
    {
        public HumanPlayer(string name, char symbol): base(name, symbol)
        {
            
        }

        public Tuple<int, int> makeMove(Board board)
        {
            var move= new Tuple<int, int>(0,0);

            return move;
        }

    }
}


