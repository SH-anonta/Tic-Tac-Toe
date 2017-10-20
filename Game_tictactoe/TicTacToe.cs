using System;
using System.Collections.Generic;
using System.Text;

namespace Game_tictactoe
{
    class Board{
        // board dimensions
        private const int ROW = 3;
        private const int COL = 3;

        // representation of possible contents of a cell
        public const char CIRCLE = 'O';
        public const char CROSS = 'X';
        public const char EMPTY = '-';

        // matrix to represent the board
        private char[,] board;

        public Board(){
            board = new char[3, 3];
            resetBoard();
        }

        // make the board empty
        private void resetBoard(){
            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    board[r, c] = EMPTY;
                }
            }
        }

        // returns the staring (matrix) representation of the board
        override public string ToString(){
            StringBuilder temp = new StringBuilder();

            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    temp.Append(board[r,c]);
                    temp.Append(" ");
                }
                temp.Append("\n");
            }

            return temp.ToString();
        }

        public char checkCell(int r, int c)
        {
            //TODO: return a deep copy
            return board[r, c];
        }

        public char [,] getBoard(){
            // returning a deep copy 
            return board;
        }

        // set the value of a cell. value must always be O or X
        public void makeMove(int r, int c, char value){
            if (value != CROSS && value != CIRCLE)
                throw new ArgumentException("Value must be O or X");
            if (board[r, c] != EMPTY)
                throw new ArgumentException("Another value in this cell aldready exists");

            board[r, c] = value;
        }
    }
}
