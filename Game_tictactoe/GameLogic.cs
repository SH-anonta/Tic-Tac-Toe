using System;
using System.Collections.Generic;
using System.Text;

namespace Game_tictactoe{
    class GameEngine{
        private Player player1;
        private Player player2;
        private Board board;
        
        public GameEngine(Player player1, Player player2){
            this.player1 = player1;
            this.player2 = player2;
            board = new Board();
        }

        // starts the game, asks playrs to make move at their turn
        // checks board status and declares outcome
        public void startGame(){
            const int MAX_TURNS = 9;
            Player current_turn_player = player1;
            Player next_turn_player = player2;

            Player winner= new DumbAI("Nobody", BoardSymbol.Circle);

            for (int i = 0; i < MAX_TURNS; i++) {
                Console.Clear();
                Console.WriteLine(current_turn_player.getPlayerName()+" turn" );
                Console.WriteLine(board);

                current_turn_player.makeMove(board);
                
                
                Player temp = current_turn_player;
                
                if (board.isWinningState()) {
                    winner = current_turn_player;
                    break;
                }
                
                // swap two references
                current_turn_player= next_turn_player;
                next_turn_player= temp;
            }
            
            const string outcome_msg_format = "{0} wins!";
            Console.WriteLine(board);
            Console.WriteLine(outcome_msg_format, winner.getPlayerName());
            
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


        // check if the board is in a winning state
        public bool isWinningState(){
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

    // Base class for entities that make moves on the game Board.
    // the makeMove method is meant to be overwritten so derived classes can have their own decision making stratagies
    class Player{
        protected BoardSymbol my_symbol;
        protected string player_name;

        public Player(string name, BoardSymbol my_symbol){
            this.player_name = name;
            this.my_symbol = my_symbol;
        }

        public string getPlayerName(){
            return player_name;
        }

        virtual public void makeMove(Board board){
            throw new NotImplementedException("This class is not meant to be used directly, use a descenent class instead");
        }

    }

    // asks the user through console and uses the human's decision to make a move
    class HumanPlayer : Player{
        // mapping of key buttons(1-9) to board cell possition
        private static Tuple<int, int>[] KEY_POSITION_MAP= { 
                    new Tuple<int, int>(2,0),
                    new Tuple<int, int>(2,1),
                    new Tuple<int, int>(2,2),
                    new Tuple<int, int>(1,0),
                    new Tuple<int, int>(1,1),
                    new Tuple<int, int>(1,2),
                    new Tuple<int, int>(0,0),
                    new Tuple<int, int>(0,1),
                    new Tuple<int, int>(0,2)
                };

        public HumanPlayer(string name, BoardSymbol my_symbol) : base(name, my_symbol){
            
        }

        override public void makeMove(Board board){
            while (true) {
                string input = Console.ReadLine();
                int n = int.Parse(input);

                int[] pos = numToCellPosition(n);
                int r = pos[0];
                int c = pos[1];

                if(board.checkCell(r,c) != BoardSymbol.Empty) {
                    continue;
                }

                board.makeMove(r,c, my_symbol);
                break;
            }
        }

        // returns a pair (r,c), that indicates the row and column number of n in the standard keyboard
        private int[] numToCellPosition(int n) {
            // index 0 and 1 store row and column number respectively
            int[] pos = {0,0};
            pos[0] = KEY_POSITION_MAP[n-1].Item1;
            pos[1] = KEY_POSITION_MAP[n-1].Item2;

            return pos;
        }

    }

    // randomly makes a move on an empty cell in the game board
    class DumbAI: Player {
        public DumbAI(string name, BoardSymbol my_symbol) : base(name, my_symbol){

        } 

        override public void makeMove(Board board){
        }
    }

    // smart enough to never let it's opponent win. The game will either end with his victory or a tie
    class SmartAI: Player {
        public SmartAI(string name, BoardSymbol my_symbol) : base(name, my_symbol){

        }

        override public void makeMove(Board board){

        }
    }
}
