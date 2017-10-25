using System;
using Utility;
using Game_views;

namespace Game_tictactoe
{
    class Launcher{
        static void Main(string[] args){
            ViewLauncher g = new ViewLauncher();
            g.launchView();
            //testBoradPrint();

        }

        // some driver funcitons
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }
        
        private static void testBoard() {
            Board b = new Board();

            b.makeMove(0, 0, BoardSymbol.Circle);
            b.makeMove(0, 1, BoardSymbol.Cross);
            b.makeMove(0, 2, BoardSymbol.Circle);
            b.makeMove(1, 0, BoardSymbol.Cross);
            b.makeMove(2, 0, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);
            
            print(b);
            print(b.isWinningState());
            print(b.isTieState());

        }

        private static void testGameEngine() {
            
            Player p = new HumanPlayer("Player1", BoardSymbol.Cross);
            Player q = new HumanPlayer("Player2", BoardSymbol.Circle);
            
            GameEngine ge = new GameEngine(p,q);
            ge.startGame();

        }

        private static void testSmartAI() {
            Board b = new Board();

            b.makeMove(0, 2, BoardSymbol.Circle);
            b.makeMove(1, 1, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);
            
            SmartAI s = new SmartAI("smartasss", BoardSymbol.Cross);
            s.makeMove(b);
        }

        private static void testBoradPrint() {
            string format= @"
 {0}  | {1} | {2}
____|___|____
 {3}  | {4} | {5}
____|___|____
 {6}  | {7} | {8}
    |   |
";

            char x = 'X';
            char o = 'O';

            Console.WriteLine(format, x,x,x,o,x,o,o,x,o);
        }
    }

}


