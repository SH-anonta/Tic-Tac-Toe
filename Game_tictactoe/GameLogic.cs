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

            // default winner is no one
            Player winner= new DumbAI("Nobody", BoardSymbol.Circle);

            for (int i = 0; i < MAX_TURNS; i++) {
                Console.Clear();
                Console.WriteLine(current_turn_player.getPlayerName()+"'s turn" );
                Console.WriteLine("------------------");
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
            Console.Clear();
            Console.WriteLine(outcome_msg_format, winner.getPlayerName());
            Console.WriteLine("------------------");
            Console.WriteLine(board);
            // wait for user to press enter
            Console.Read(); 
            Console.Read(); 
            Console.Read(); 
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

        public BoardSymbol getCell(int r, int c){
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

        public void clearCell(int r, int c) {
            board[r,c] = BoardSymbol.Empty;
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

        public bool isTieState() {
            bool tie= false;

            if(! isWinningState() && countEmptyCells() == 0)
                tie = true;

            return tie;
        }

        public int countEmptyCells() {
            int count = 0;
            for(int i= 0; i<ROW; i++) {
                for(int j= 0; j<COL; j++) {
                    if(board[i,j] == BoardSymbol.Empty)
                        count++;
                }
            }

            return count;
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
                int key = getUserInput();
                if(key == -1) continue;
                
                int[] pos = numToCellPosition(key);
                int r = pos[0];
                int c = pos[1];

                if(board.getCell(r,c) != BoardSymbol.Empty) {
                    Console.WriteLine("Cell already used.");
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

        private System.ConsoleKey catchKeyPress() {
            // wait for the user to press a key
            while (!Console.KeyAvailable) {
                // do nothing
            }

            // read what key was pressed
            return Console.ReadKey().Key;
        }

        // Get input from user on which cell to place cross or circle
        private int getUserInput() {
            int key = 1;
            System.ConsoleKey press = catchKeyPress();

            if (press == System.ConsoleKey.NumPad1)
                key= 1;
            else if (press == System.ConsoleKey.NumPad2)
                key= 2;
            else if (press == System.ConsoleKey.NumPad3)
                key= 3;
            else if (press == System.ConsoleKey.NumPad4)
                key= 4;
            else if (press == System.ConsoleKey.NumPad5)
                key= 5;
            else if (press == System.ConsoleKey.NumPad6)
                key= 6;
            else if (press == System.ConsoleKey.NumPad7)
                key= 7;
            else if (press == System.ConsoleKey.NumPad8)
                key= 8;
            else if (press == System.ConsoleKey.NumPad9)
                key= 9;
            else
                // an invalid key was pressed
                key= -1;
            return key;
        }

    }

    // randomly makes a move on an empty cell in the game board
    class DumbAI: Player {
        public DumbAI(string name, BoardSymbol my_symbol) : base(name, my_symbol){

        } 

        override public void makeMove(Board board){
            while (true) {
                int[] move = generateRandomMove();
                int r= move[0];
                int c= move[1];

                if(board.getCell(r, c) == BoardSymbol.Empty) {
                    board.makeMove(r,c, my_symbol);
                    break;
                }
            }
        }

        private int[] generateRandomMove() {
            int[] move = {0,0};
            
            Random rand = new Random();
            move[0]= rand.Next(0,3);
            move[1]= rand.Next(0,3);

            return move;
        }
    }

    // smart enough to never let it's opponent win. The game will either end with his victory or a tie
    class SmartAI: Player {
        private const int WIN_STATE_SCORE= 150;
        private const int LOSE_STATE_SCORE= -100;
        private const int TIE_STATE_SCORE= 0;

        private int best_move_r;
        private int best_move_c;

        public SmartAI(string name, BoardSymbol my_symbol) : base(name, my_symbol){
            best_move_r= 0;
            best_move_r= 0;
        }

        override public void makeMove(Board board){
            int val = findBestMove(board, true, my_symbol,0);
            //print("Best", val);
            //Console.ReadLine();
            board.makeMove(best_move_r, best_move_c, my_symbol);
        }

        private static int max(int a, int b) {
            return a>b? a : b;
        }
        private static int min(int a, int b) {
            return a<b? a : b;
        }

        private static int MoveBonus(int r, int c) {
            int bons= 0;
            const int bon= 2;
            if(r == 0 && c == 0)
                bons = bon;
            else if(r == 0 && c == 2)
                bons = bon;
            else if(r == 2 && c == 0)
                bons = bon;
            else if(r == 2 && c == 2)
                bons = bon;
            return bons;
        }

        private int findBestMove(Board board, bool maximize, BoardSymbol symbol, int depth) {

            if(board.isWinningState()){
                // a win state here means whoever made the previous move wins
                // the depth is used to alter the winning and loosing score
                // the faster a state can be reached the higher it's score
                int score = maximize ?  LOSE_STATE_SCORE + depth: WIN_STATE_SCORE - depth;
                return score;
            }
            else if (board.countEmptyCells() == 0) {
                // if this is a tie state
                return TIE_STATE_SCORE - depth;
            }

            BoardSymbol next_symbol= getOppositeSymbol(symbol);
            int minmax= maximize ? -10000: 10000;  // best score for maximizer or minimizer
            int minmax_r= 0;   //next move row position for best score
            int minmax_c= 0;   //next move column position for best score 
            
            for (int r= 0; r<3; r++) {
                for(int c= 0; c<3; c++) {
                    // if this cell is already taken, skip
                    if(board.getCell(r,c) != BoardSymbol.Empty)
                        continue;
                    
                    // make move
                    board.makeMove(r,c,symbol);
                        
                    // find the best decision for the opposing player
                    // if this is maximizer, make call to minimize and vice versa
                    int score = findBestMove(board, !maximize, next_symbol, depth+1);
                    
                    // undo move
                    board.clearCell(r,c);
                    
                    //print("Out",score,minmax, r, c, maximize);

                    // get the best value for maximizer or minimizer
                    if (maximize) {
                        if(score > minmax) {
                            minmax = score;
                            minmax_r = r;
                            minmax_c = c;

                            
                            //print(minmax, r, c, maximize);
                        }
                    }
                    else if(score < minmax){
                        minmax = score;
                        minmax_r = r;
                        minmax_c = c;
                    }
                }
            }

            // theis is where the next move cell position is stored
            if(maximize) {
                best_move_r= minmax_r;
                best_move_c= minmax_c;
            }

            return minmax;
        }
        
        private void inspect(Board b) {
        }

        // todo remove
        private static void print(params object[] tokens) { Console.WriteLine(string.Join(" ", tokens)); }

        private BoardSymbol getOppositeSymbol(BoardSymbol symb){
            BoardSymbol opposite = BoardSymbol.Empty;   // invalid value

            if(symb == BoardSymbol.Circle) 
                opposite = BoardSymbol.Cross;
            else
                opposite = BoardSymbol.Circle;

            return opposite;
        }

        private int[] winInOneMove(Board board, BoardSymbol sym) {
            return new int[2];
        }
    }
}
