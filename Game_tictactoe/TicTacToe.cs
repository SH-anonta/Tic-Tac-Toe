using System;
using System.Collections.Generic;
using System.Text;

namespace Game_tictactoes
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

        public int countEmptyCells(){
            int empty_count = 0;

            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    if (board[r, c] == EMPTY)
                        empty_count++;
                }
            }

            return empty_count;
        }

        // check if the board is in a winning state
        public bool winningState(){
            bool win = false;

            // check diagonal, and none of then are empty
            if (board[0,0] != EMPTY && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                win = true;
            // check diagonal, and none of then are empty
            else if (board[0, 2] != EMPTY && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                win = true;
            
            // check rows for streak of same value
            for(int r= 0; r<ROW; r++) {
                if (board[r, 0] != EMPTY && board[r, 0] == board[r, 1] && board[r, 1] == board[r, 2])
                    win = true;
            }

            // check columns for streak of same value
            for (int c = 0; c < ROW; c++) {
                if (board[0, c] != EMPTY && board[0,c] == board[1, c] && board[1, c] == board[2, c])
                    win = true;
            }

            return win;
        }
    }
}
