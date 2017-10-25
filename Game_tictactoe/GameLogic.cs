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

        // this is used with stirng format
        private const string BOARD_STRING_FORMAT= @"
              {0}  | {1} | {2}
             ____|___|____
              {3}  | {4} | {5}
             ____|___|____
              {6}  | {7} | {8}
                 |   |
";
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
        private string enumToStr(BoardSymbol symb) {
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
            return string.Format(BOARD_STRING_FORMAT, 
                enumToStr(board[0,0]),
                enumToStr(board[0,1]),
                enumToStr(board[0,2]),
                enumToStr(board[1,0]),
                enumToStr(board[1,1]),
                enumToStr(board[1,2]),
                enumToStr(board[2,0]),
                enumToStr(board[2,1]),
                enumToStr(board[2,2]));
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

        // assosiates keyboard buttons to moves in the game board
        private static Dictionary<ConsoleKey, Tuple<int,int>> KEY_POSITION_MAP;
        
        // used as default value when doing getwithdefault on the mapp
        private static Tuple<int,int> INVALID_POSITION;

        private static void initializeKeyPositionMap() {
            KEY_POSITION_MAP = new Dictionary<ConsoleKey, Tuple<int, int>>();
            INVALID_POSITION = new Tuple<int, int>(-1,-1);
            
            Tuple<int, int> cell00 = new Tuple<int, int>(0,0);
            Tuple<int, int> cell01 = new Tuple<int, int>(0,1);
            Tuple<int, int> cell02 = new Tuple<int, int>(0,2);
            Tuple<int, int> cell10 = new Tuple<int, int>(1,0);
            Tuple<int, int> cell11 = new Tuple<int, int>(1,1);
            Tuple<int, int> cell12 = new Tuple<int, int>(1,2);
            Tuple<int, int> cell20 = new Tuple<int, int>(2,0);
            Tuple<int, int> cell21 = new Tuple<int, int>(2,1);
            Tuple<int, int> cell22 = new Tuple<int, int>(2,2);

            
            KEY_POSITION_MAP.TryAdd(ConsoleKey.Q, cell00);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad7, cell00);
            
            KEY_POSITION_MAP.Add(ConsoleKey.W, cell01);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad8, cell01);

            KEY_POSITION_MAP.Add(ConsoleKey.E, cell02);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad9, cell02);

            KEY_POSITION_MAP.Add(ConsoleKey.A, cell10);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad4, cell10);

            KEY_POSITION_MAP.Add(ConsoleKey.S, cell11);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad5, cell11);
            
            KEY_POSITION_MAP.Add(ConsoleKey.D, cell12);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad6, cell12);

            KEY_POSITION_MAP.Add(ConsoleKey.Z, cell20);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad1, cell20);

            KEY_POSITION_MAP.Add(ConsoleKey.X, cell21);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad2, cell21);

            KEY_POSITION_MAP.Add(ConsoleKey.C, cell22);
            KEY_POSITION_MAP.Add(ConsoleKey.NumPad3, cell22);
        }

        public HumanPlayer(string name, BoardSymbol my_symbol) : base(name, my_symbol){
            // the association must be made only once
            if(KEY_POSITION_MAP == null) {
                initializeKeyPositionMap();
            }
        }

        override public void makeMove(Board board){
            while (true) {
                ConsoleKey pressed_key =  catchKeyPress();

                Tuple<int,int> move =  KEY_POSITION_MAP.GetValueOrDefault(pressed_key, INVALID_POSITION);

                if(move == INVALID_POSITION)
                    continue;
                
                int r = move.Item1;
                int c = move.Item2;

                if(board.getCell(r,c) != BoardSymbol.Empty) {
                    Console.WriteLine("Cell already used.");
                    continue;
                }

                board.makeMove(r,c, my_symbol);
                break;
            }
        }

        private System.ConsoleKey catchKeyPress() {
            // wait for the user to press a key
            while (!Console.KeyAvailable) {
                // do nothing
            }

            // read what key was pressed
            return Console.ReadKey().Key;
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

        // TODO turn them into local variables
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
        
        private static int max(int a, int b) {
            return a>b? a : b;
        }
        private static int min(int a, int b) {
            return a<b? a : b;
        }
        private BoardSymbol getOppositeSymbol(BoardSymbol symb){
            BoardSymbol opposite = BoardSymbol.Empty;   // invalid value

            if(symb == BoardSymbol.Circle) 
                opposite = BoardSymbol.Cross;
            else
                opposite = BoardSymbol.Circle;

            return opposite;
        }
    }
}
