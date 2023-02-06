using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Windows.Forms;
using PieceBehavior = Chess.PieceMove;

namespace Chess
{
    public partial class ChessGame : Form
    {
        private History History = new History();
        private PictureBox[][] board = new PictureBox[8][]; // overview of all pieces on the board currently
        private PictureBox selectedpiece; // piece that is currently selected to move
        private List<PictureBox> PossiblePieceToTake = new List<PictureBox>(); // highlight all possible pieces that can be taken by a selected piece
        private LastMoveMade lastMoveMade;
        DisplayPossibleMoves displayPossibleMoves;
        private gameState currentState = gameState.Normal;
        public enum pieceName { Rook, Knight, Bishop, Queen, King, Pawn, None };
        public enum pieceColor { White, Black };
        public enum AIColor { White, Black, None }
        public enum gameState { Normal, Stalemate, Checkmate, Check };
        private bool turn = true; // determine whether it is white's or black's turn : true for white and false for black
        private bool alreadySelected = false; // determine if a piece is already selected
        public enum Opponent { AI, Player, None };
        public Opponent opponent = Opponent.None;
        public AIColor aiColor = AIColor.None; // determine the AI color
        public int AIComplexity = 2;
        private ComponentResourceManager resources = new ComponentResourceManager(typeof(ChessGame));
        public Dictionary<pieceColor, Dictionary<pieceName, List<PictureBox>>> allChessPieces = new Dictionary<pieceColor, Dictionary<pieceName, List<PictureBox>>>() 
        {
            { pieceColor.White, new Dictionary<pieceName, List<PictureBox>>() },
            { pieceColor.Black, new Dictionary<pieceName, List<PictureBox>>() }
        };
        private Dictionary<PictureBox, PieceStateDetails> pieceStateMapping = new Dictionary<PictureBox, PieceStateDetails>(); // maps each chess piece to the type of piece

        public ChessGame()
        {
            InitializeComponent();
            InitializeChessPiecesDictionary();
            ResetGame();
            InitializeHighlightBoard();
            /* For testing different board positions */
            //TestBoard testBoard = new TestBoard(board, this);
            //testBoard.selectBoardChoice("TestAIOptimization", ChessGame.Opponent.AI, true, ChessGame.AIColor.Black, 3);
        }

        private void InitializeHighlightBoard() 
        {
            lastMoveMade = new LastMoveMade(panel.CreateGraphics(), Color.FromArgb(150, 178, 34, 34), pieceStateMapping);
            displayPossibleMoves = new DisplayPossibleMoves(panel.CreateGraphics(), Color.FromArgb(125, 48, 118, 240), pieceStateMapping);
        }

        private void InitializeChessPiecesDictionary() 
        {
            allChessPieces[pieceColor.White] = new Dictionary<pieceName, List<PictureBox>>()
            {
                { pieceName.Pawn, new List<PictureBox>(new PictureBox[] { wp1, wp2, wp3, wp4, wp5, wp6, wp7, wp8 }) },
                { pieceName.Rook, new List<PictureBox>(new PictureBox[] { wr1, wr2 }) },
                { pieceName.Knight, new List<PictureBox>(new PictureBox[] { wkn1, wkn2 }) },
                { pieceName.Bishop, new List<PictureBox>(new PictureBox[] { wb1, wb2 }) },
                { pieceName.Queen, new List<PictureBox>(new PictureBox[] { wq }) },
                { pieceName.King, new List<PictureBox>(new PictureBox[] { wk }) }
            };

            allChessPieces[pieceColor.Black] = new Dictionary<pieceName, List<PictureBox>>()
            {
                { pieceName.Pawn, new List<PictureBox>(new PictureBox[] { bp1, bp2, bp3, bp4, bp5, bp6, bp7, bp8 }) },
                { pieceName.Rook, new List<PictureBox>(new PictureBox[] { br1, br2 }) },
                { pieceName.Knight, new List<PictureBox>(new PictureBox[] { bkn1, bkn2 }) },
                { pieceName.Bishop, new List<PictureBox>(new PictureBox[] { bb1, bb2 }) },
                { pieceName.Queen, new List<PictureBox>(new PictureBox[] { bq }) },
                { pieceName.King, new List<PictureBox>(new PictureBox[] { bk }) }
            };
        }

        // this will set the map each piece to the correct type of piece (which can changed if promoted)
        private void setPieceStateMapping() 
        {
            pieceStateMapping = new Dictionary<PictureBox, PieceStateDetails>();
            foreach (KeyValuePair<pieceColor, Dictionary<pieceName, List<PictureBox>>> pieceColor in allChessPieces) 
            {
                foreach (KeyValuePair<pieceName, List<PictureBox>> pieceName in pieceColor.Value) 
                {
                    foreach (PictureBox pieceObj in pieceName.Value) 
                    {
                        pieceStateMapping.Add(pieceObj, new PieceStateDetails(pieceColor.Key, pieceName.Key));
                        pieceObj.Image = (Image)resources.GetObject($"{pieceObj.Name}.Image");
                    }
                }
            }
        }

        private void resetBoard() 
        {
            pieceName[] dualPieces = new pieceName[] { pieceName.Rook, pieceName.Knight, pieceName.Bishop };
            resetPieceFormation(pieceColor.White, dualPieces, new int[] { 7, 6 });
            resetPieceFormation(pieceColor.Black, dualPieces, new int[] { 0, 1 });
        }

        // set all pieces into the correct position when board is resetted
        private void resetPieceFormation(pieceColor pieceColor, pieceName[] dualPieces, int[] boardXCoord) 
        {
            Dictionary<pieceName, List<PictureBox>> pieces = allChessPieces[pieceColor];
            for (int i = 0; i < pieces[pieceName.Pawn].Count; i++)
            {
                board[boardXCoord[1]][i] = pieces[pieceName.Pawn][i];
            }

            for (int i = 0; i < dualPieces.Count(); i++) 
            {
                for (int j = 0; j < 2; j++)
                {
                    if (j == 0)
                    {
                        board[boardXCoord[0]][0 + i] = pieces[dualPieces[i]][j];
                    }
                    else if (j == 1)
                    {
                        board[boardXCoord[0]][7 - i] = pieces[dualPieces[i]][j];
                    }
                }
            }

            board[boardXCoord[0]][3] = pieces[pieceName.Queen][0];
            board[boardXCoord[0]][4] = pieces[pieceName.King][0];
        }

        private void clickWhite(object sender, EventArgs e)
        {
            if (currentState != gameState.Checkmate && currentState != gameState.Stalemate && opponent != Opponent.None) 
            {
                if (alreadySelected && !turn) // when black piece eats white piece
                {
                    if (MovePiece((PictureBox)sender))
                    {
                        LastModeMade();
                        selectedpiece.BackColor = Color.Transparent;
                        foreach (PictureBox p in PossiblePieceToTake)
                            p.BackColor = Color.Transparent;
                        PossiblePieceToTake = new List<PictureBox>();
                        completeTurn();
                        AIMove();
                    }
                    else 
                    {
                        InvalidMove();
                    } 
                }
                else if (turn && selectedpiece != (PictureBox)sender) // selecting white piece
                {
                    resetHighlightedPieces();
                    LastModeMade();
                    selectedpiece = (PictureBox)sender;
                    selectedpiece.BackColor = Color.FromArgb(220, 13, 86, 212);
                    alreadySelected = true;
                    pieceName sourcePieceType = PieceDetails.findSelectedPiece(selectedpiece, pieceStateMapping).PieceName;
                    int Y = PieceDetails.FindCoordinate(selectedpiece.Location.Y);
                    int X = PieceDetails.FindCoordinate(selectedpiece.Location.X);
                    displayPossibleMoves.DisplayMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, History);
                }
                else if (alreadySelected && selectedpiece == (PictureBox)sender)
                {
                    InvalidMove();
                }
            }
        }

        private void clickBlack(object sender, EventArgs e)
        {
            if (currentState != gameState.Checkmate && currentState != gameState.Stalemate && opponent != Opponent.None)
            {
                if (alreadySelected && turn) // when white piece eats black piece
                {
                    if (MovePiece((PictureBox)sender))
                    {
                        LastModeMade();
                        selectedpiece.BackColor = Color.Transparent;
                        foreach (PictureBox p in PossiblePieceToTake)
                            p.BackColor = Color.Transparent;
                        PossiblePieceToTake = new List<PictureBox>();
                        completeTurn();
                        AIMove();
                    }
                    else
                    {
                        InvalidMove();
                    }
                }
                else if (!turn && selectedpiece != (PictureBox)sender) // selecting black piece
                {
                    resetHighlightedPieces();
                    LastModeMade();
                    selectedpiece = (PictureBox)sender;
                    selectedpiece.BackColor = Color.FromArgb(220, 13, 86, 212);
                    alreadySelected = true;
                    pieceName sourcePieceType = PieceDetails.findSelectedPiece(selectedpiece, pieceStateMapping).PieceName;
                    int Y = PieceDetails.FindCoordinate(selectedpiece.Location.Y);
                    int X = PieceDetails.FindCoordinate(selectedpiece.Location.X);
                    displayPossibleMoves.DisplayMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, History);
                }
                else if (alreadySelected && selectedpiece == (PictureBox)sender) 
                {
                    InvalidMove();
                }
            }
        }

        private void board_Click(object sender, EventArgs e) // when selected piece move to empty square
        {
            if (currentState != gameState.Checkmate && currentState != gameState.Stalemate && opponent != Opponent.None)
            {
                if (alreadySelected)
                {
                    Point position = Cursor.Position;
                    position = panel.PointToClient(position);
                    int destinationY = PieceDetails.FindCoordinate(position.Y);
                    int destinationX = PieceDetails.FindCoordinate(position.X);
                    if (MovePiece(destinationY, destinationX))
                    {
                        resetHighlightedPieces();
                        completeTurn();
                        AIMove();
                    }
                    else 
                    {
                        InvalidMove();
                    }
                }
            }
        }

        private bool MovePiece(PictureBox destination) // moving a piece to eat another piece
        {
            pieceName sourcePieceType = PieceDetails.findSelectedPiece(selectedpiece, pieceStateMapping).PieceName;
            int destinationY = PieceDetails.FindCoordinate(destination.Location.Y);
            int destinationX = PieceDetails.FindCoordinate(destination.Location.X);
            return IsValidMove(sourcePieceType, destinationY, destinationX);
        }


        private bool MovePiece(int destinationY, int destinationX) // moving a piece to an empty square
        {
            pieceName sourcePieceType = PieceDetails.findSelectedPiece(selectedpiece, pieceStateMapping).PieceName;
            return IsValidMove(sourcePieceType, destinationY, destinationX);
        }

        private bool IsValidMove(pieceName sourcePieceType, int destinationY, int destinationX, ChessGame.pieceName promptedToAI = ChessGame.pieceName.None) // determine if the piece is able to move to its target square
        {
            int sourceY = PieceDetails.FindCoordinate(selectedpiece.Location.Y);
            int sourceX = PieceDetails.FindCoordinate(selectedpiece.Location.X);
            switch (sourcePieceType)
            {
                case pieceName.Pawn:
                    PieceBehavior.Pawn pawn = new PieceBehavior.Pawn(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return pawn.Move(board);
                case pieceName.Rook:
                    PieceBehavior.Rook rook = new PieceBehavior.Rook(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return rook.Move(board);
                case pieceName.Knight:
                    PieceBehavior.Knight knight = new PieceBehavior.Knight(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return knight.Move(board);
                case pieceName.Bishop:
                    PieceBehavior.Bishop bishop = new PieceBehavior.Bishop(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return bishop.Move(board);
                case pieceName.Queen:
                    PieceBehavior.Queen queen = new PieceBehavior.Queen(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return queen.Move(board);
                default:
                    PieceBehavior.King king = new PieceBehavior.King(board, sourceY, sourceX, destinationY, destinationX, History, turn, pieceStateMapping, promptedToAI, this);
                    return king.Move(board);
            }
        }

        private void completeTurn()
        {
            panel.Refresh();
            selectedpiece = null;
            alreadySelected = false;
            History = History.Next;
            currentState = History.State;
            displayGameState();
            LastModeMade();
            turn = !turn;
        }

        private void InvalidMove() 
        {
            resetHighlightedPieces();
            panel.Refresh();
            selectedpiece = null;
            alreadySelected = false;
            LastModeMade();
        }

        private void displayGameState() // display state of game over completing a move such as Check,Checkmate,Stalemate
        {
            if (currentState == gameState.Check)
            {
                if (turn)
                    Status.Text = "Black king checked";
                else
                    Status.Text = "White king checked";
            }
            else if (currentState == gameState.Checkmate)
            {
                if (turn)
                    Status.Text = "Checkmate! White wins!";
                else
                    Status.Text = "Checkmate! Black wins!";
            }
            else if (currentState == gameState.Stalemate)
            {
                if (turn)
                    Status.Text = "Stalemate by white";
                else
                    Status.Text = "Stalemate by black!";
            }
            else
                Status.Text = "";
        }

        private void resetHighlightedPieces() // reset all highlighted squares for selected piece and all other squares with a piece highlighted to transparent
        {
            panel.Refresh();
            if (selectedpiece != null)
                selectedpiece.BackColor = Color.Transparent;
            foreach (PictureBox p in PossiblePieceToTake)
                p.BackColor = Color.Transparent;          
            PossiblePieceToTake = new List<PictureBox>();
        }

        private void LastModeMade() 
        {
            lastMoveMade.DisplayLastMove(board, History);
        }

        public void ResetGame() // reset chess game
        {
            setPieceStateMapping();
            resetHighlightedPieces();
            RemoveHighlightLastMove();
            History = new History();
            currentState = gameState.Normal;
            turn = true;
            selectedpiece = null;
            alreadySelected = false;
            PossiblePieceToTake = new List<PictureBox>();
            opponent = Opponent.None;
            aiColor = AIColor.None;
            AIComplexity = 2;
            board = new PictureBox[8][];
            for (int i = 0; i < board.Length; i++)
                board[i] = new PictureBox[8];
            resetBoard();
            for (int y = 0; y < board.Length; y++)
            {
                if ((y >= 0 && y <= 1) || y >= 6)
                {
                    for (int x = 0; x < board.Length; x++)
                    {
                        board[y][x].Visible = true;
                        PieceDetails.movePiece(y, x, board[y][x]);
                    }
                }
            }
        }

        private void RemoveHighlightLastMove() 
        {
            if (History.Prev != null)
                History.Source.BackColor = Color.Transparent;
        }

        private void undo() 
        {
            History prev = History.Prev;
            currentState = prev != null ? prev.State : gameState.Normal; 
            turn = History.Turn;
            board[History.SourceY][History.SourceX] = History.Source;
            PieceDetails.movePiece(History.SourceY, History.SourceX, History.Source);
            board[History.DestinationY][History.DestinationX] = History.Destination;
            if (History.EnPassantDetails != null)
            {
                int Y = History.EnPassantDetails.Y;
                int X = History.EnPassantDetails.X;
                PictureBox Target = History.EnPassantDetails.Target;
                board[Y][X] = Target;
                PieceDetails.movePiece(Y, X, Target);
                Target.Visible = true;
            }
            else if (History.CastlingDetails != null) 
            {
                CastlingDetails castlingDetails = History.CastlingDetails;
                board[castlingDetails.SourceY][castlingDetails.SourceX] = castlingDetails.Source;
                board[castlingDetails.DestinationY][castlingDetails.DestinationX] = castlingDetails.Destination;
                PieceDetails.movePiece(castlingDetails.SourceY, castlingDetails.SourceX, castlingDetails.Source);
                pieceStateMapping[castlingDetails.Source].HasMoved = false;
            }
            if (History.FirstMoveMade) 
                pieceStateMapping[History.Source].HasMoved = false;
            if (History.Destination != null) // if destination was null than the picturebox object does not need to be modified
            {
                PieceDetails.movePiece(History.DestinationY, History.DestinationX, History.Destination);
                History.Destination.Visible = true;
            }
            History.Promote.UndoPromotePiece();
            History = History.Prev; // go to previous History object
            LastModeMade();
        }

        private void redo()
        {
            History = History.Next;
            currentState = History.State;
            turn = !History.Turn;
            board[History.DestinationY][History.DestinationX] = History.Source;
            PieceDetails.movePiece(History.DestinationY, History.DestinationX, History.Source);
            board[History.SourceY][History.SourceX] = null;
            if (History.EnPassantDetails != null)
            {
                board[History.EnPassantDetails.Y][History.EnPassantDetails.X] = null; 
                History.EnPassantDetails.Target.Visible = false;
            }
            else if (History.CastlingDetails != null)
            {
                CastlingDetails castlingDetails = History.CastlingDetails;
                board[castlingDetails.SourceY][castlingDetails.SourceX] = castlingDetails.Destination;
                board[castlingDetails.DestinationY][castlingDetails.DestinationX] = castlingDetails.Source;
                PieceDetails.movePiece(castlingDetails.SourceY, castlingDetails.SourceX, castlingDetails.Source);
                pieceStateMapping[castlingDetails.Source].HasMoved = true;
            }
            if (History.FirstMoveMade)
                pieceStateMapping[History.Source].HasMoved = true;
            History.Promote.RedoPromotePiece();
            if (History.Destination != null)
                History.Destination.Visible = false;
            LastModeMade();
        }

        private void UndoRedoReset() 
        {
            resetHighlightedPieces();
            LastModeMade();
            selectedpiece = null;
            alreadySelected = false;
            displayGameState();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e) // undo feature
        {
            if (History.Prev != null) // The previous move is based on the current History object
            {
                panel.Refresh();
                gameState prevGameState = currentState;
                undo();
                if (opponent == Opponent.AI)
                {
                    if (History.Prev != null && ((turn && aiColor == AIColor.White) || (!turn && aiColor == AIColor.Black) || prevGameState != gameState.Checkmate && prevGameState != gameState.Stalemate))
                        undo();
                    if (turn && aiColor == AIColor.White)
                        AIMove();
                }
                UndoRedoReset();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e) // redo feature
        {
            if (History.Next != null) // The next History object is the next of the current History object
            {
                panel.Refresh();
                redo();
                if (opponent == Opponent.AI)
                {
                    if (History.Next != null)
                        redo();
                    else
                        turn = !turn;
                }
                UndoRedoReset();
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e) // start a new game with player vs player or player vs ai 
        {
            NewGame newgame = new NewGame(this);
            DialogResult show = newgame.ShowDialog();
            if (opponent == Opponent.AI && aiColor == AIColor.White)
                AIMove();
        }

        private void rulesToolStripMenuItem_Click(object sender, EventArgs e) // game rules
        {
            Rules rules = new Rules();
            DialogResult show = rules.ShowDialog();
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e) // details of game
        {
            About about = new About();
            DialogResult show = about.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) // exit application
        {
            this.Close();
        }

        private void ChessBoard_KeyDown(object sender, KeyEventArgs e)
        {
            // shortcut keys
            if (e.Control && e.KeyCode.ToString() == "Z") // undo
            {
                undoToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode.ToString() == "R") // redo
            {
                redoToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode.ToString() == "N") // new game
            {
                newGameToolStripMenuItem_Click(sender, e); 
            }
            else if (e.KeyCode == Keys.Escape) // exit game
            {
                this.Close();
            }
        }

        public void AIMove() // determine how the AI will move and swap back to players turn after AI makes its move
        {
            if (opponent == Opponent.AI && (currentState == gameState.Normal || currentState == gameState.Check) && ((turn && aiColor == AIColor.White) || (!turn && aiColor == AIColor.Black)))
            {
                int currentBoardState = 0;
                AIMove.AIResult aiResult = new AIMove.AIResult();
                AIMove.AI ai = new AIMove.AI(0, aiResult);
                for (int y = 0; y < board.Length; y++)
                {
                    for (int x = 0; x < board[y].Length; x++)
                    {
                        if (board[y][x] == null) continue;
                        if (PieceDetails.findSelectedPiece(board[y][x], pieceStateMapping).PieceColor == pieceColor.White)
                            currentBoardState += ai.PieceValue(PieceDetails.findSelectedPiece(board[y][x], pieceStateMapping).PieceName);
                        else
                            currentBoardState -= ai.PieceValue(PieceDetails.findSelectedPiece(board[y][x], pieceStateMapping).PieceName);
                    }
                }
                EnPassantDetails enPassantDetails = History.EnPassantable ? new EnPassantDetails(History.DestinationY, History.DestinationX, History.Source) : null;
                ai.MiniMax(board, turn, AIComplexity, currentBoardState, pieceStateMapping, enPassantDetails);
                selectedpiece = board[aiResult.SourceY][aiResult.SourceX];
                if (!IsValidMove(PieceDetails.findSelectedPiece(board[aiResult.SourceY][aiResult.SourceX], pieceStateMapping).PieceName, aiResult.DestinationY, aiResult.DestinationX, aiResult.PromptedTo))
                {
                    MessageBox.Show("AI ERROR");
                    undo();
                    return;
                }
                completeTurn();
            }
        }

        public void changeTurn(bool Turn, string objType)
        {
            if (objType == "TestBoard")
                turn = Turn;
        }
    }
}