using System;
using Utility;
using Game_views;

namespace Game_tictactoe
{
    class Launcher{
        static void Main(string[] args){
            ViewLauncher g = new ViewLauncher();
            //g.launchView();
            testGameEngine();
        }

        // convenience funcitons 
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }
        
        private static void testBoard() {
            Board b = new Board();

            b.makeMove(0, 2, BoardSymbol.Circle);
            b.makeMove(1, 1, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);
            
            print(b);
            print(b.isWinningState());
        }

        private static void testGameEngine() {
            
            Player p = new HumanPlayer("Player1", BoardSymbol.Cross);
            Player q = new HumanPlayer("Player2", BoardSymbol.Circle);
            
            GameEngine ge = new GameEngine(p,q);
            ge.startGame();

        }
    }

}


