using System;
using System.Collections.Generic;
using System.Text;

namespace Game_tictactoe
{

    class GameEngine{
        private Player player1;
        private Player player2;

        GameEngine(Player player1, Player player2){
            this.player1 = player1;
            this.player2 = player2;
        }

        public string startGame(){
            const int MAX_TURNS = 9;
            Player current_turn = player1;
            Player next_turn = player1;


            for (int i = 0; i < MAX_TURNS; i++) {

            }

            return "";
        }

        private void swap(ref Object a, ref Object b){
            Object temp = a;
            a = b;
            b = temp;
        }
    }

    // Valid values of a Board cell
    enum BoardSymbol{
        Cross,
        Circle,
        Empty
    }

    class Board{
        // board dimensions
        private const int ROW = 3;
        private const int COL = 3;

        // matrix to represent the board
        private BoardSymbol[,] board;

        public Board(){
            board = new BoardSymbol[ROW, COL];
            resetBoard();
        }

        // make the board empty
        private void resetBoard(){
            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    board[r, c] = BoardSymbol.Empty;
                }
            }
        }

        // helper class, converts string BoardSymbole enum value to it's string representative
        private string getStringRepresentation(BoardSymbol symb) {
            string symbol = "";

            if (symb == BoardSymbol.Cross)
                symbol= "X";
            else if (symb == BoardSymbol.Circle)
                symbol= "O";
            else if (symb == BoardSymbol.Empty)
                symbol= "-";

            return symbol;
        }


        // returns the staring (matrix) representation of the board
        override public string ToString(){
            StringBuilder temp = new StringBuilder();

            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    temp.Append(getStringRepresentation(board[r, c]));
                    temp.Append(" ");
                }
                temp.Append("\n");
            }

            return temp.ToString();
        }

        public BoardSymbol checkCell(int r, int c){
            //TODO: return a deep copy
            return board[r, c];
        }

        public BoardSymbol[,] getBoard(){
            // returning a deep copy 
            return board;
        }

        // set the value of a cell. value must always be O or X
        public void makeMove(int r, int c, BoardSymbol value){
            if (board[r, c] != BoardSymbol.Empty)
                throw new ArgumentException("Another value in this cell aldready exists");

            board[r, c] = value;
        }

        public int countEmptyCells(){
            int empty_count = 0;

            for (int r = 0; r < ROW; r++) {
                for (int c = 0; c < COL; c++) {
                    if (board[r, c] == BoardSymbol.Empty)
                        empty_count++;
                }
            }

            return empty_count;
        }

        // check if the board is in a winning state
        public bool winningState(){
            bool win = false;

            // check diagonal, and none of then are empty
            if (board[0, 0] != BoardSymbol.Empty && board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                win = true;
            // check diagonal, and none of then are empty
            else if (board[0, 2] != BoardSymbol.Empty && board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                win = true;

            // check rows for streak of same value
            for (int r = 0; r < ROW; r++) {
                if (board[r, 0] != BoardSymbol.Empty && board[r, 0] == board[r, 1] && board[r, 1] == board[r, 2])
                    win = true;
            }

            // check columns for streak of same value
            for (int c = 0; c < ROW; c++) {
                if (board[0, c] != BoardSymbol.Empty && board[0, c] == board[1, c] && board[1, c] == board[2, c])
                    win = true;
            }

            return win;
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
        virtual public Tuple<int, int> makeMove(Board board){
            throw new NotImplementedException();
        }

    }

    class HumanPlayer : Player{
        public HumanPlayer(string name, char symbol) : base(name, symbol){

        }

        public Tuple<int, int> makeMove(Board board){
            var move = new Tuple<int, int>(0, 0);

            return move;
        }

    }
}
