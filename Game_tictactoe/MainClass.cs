using System;
using Utility;
using Game_views;

namespace Game_tictactoe
{
    class Launcher{
        static void Main(string[] args){
            ViewLauncher g = new ViewLauncher();
            //g.startGame();

            Board b = new Board();

            b.makeMove(0, 2, BoardSymbol.Circle);
            b.makeMove(1, 1, BoardSymbol.Circle);
            b.makeMove(2, 0, BoardSymbol.Circle);

            print(b);
            print(b.winningState());
            print(b.countEmptyCells());
        }

        // convenience funcitons 
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }
    }

}


