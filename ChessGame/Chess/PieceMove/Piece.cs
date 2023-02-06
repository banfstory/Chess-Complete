using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    abstract class Piece
    {
        ChessGame Main;
        protected History history;
        protected bool turn;
        protected PictureBox source;
        protected PictureBox destination;
        protected int sourceY;
        protected int sourceX;
        protected int destinationY;
        protected int destinationX;
        protected int diffY; // difference between destination to source for Y which determine its direction and how far
        protected int diffX; // difference between destination to source for X which determine its direction and how far
        protected Dictionary<PictureBox, PieceStateDetails> pieceStateMapping;
        protected EnPassantDetails enPassantDetails = null;
        protected CastlingDetails castlingDetails = null;
        protected bool enPassantable = false;
        ChessGame.pieceName PromptedToAI;

        protected void InitalizePiece(PictureBox[][] board, bool Turn, int SourceY, int SourceX, int DestinationY, int DestinationX, History History, Dictionary<PictureBox, PieceStateDetails> PieceStateMapping, ChessGame.pieceName promptedToAI, ChessGame main)
        {
            Main = main;
            history = History;
            turn = Turn;
            source = board[SourceY][SourceX];
            destination = board[DestinationY][DestinationX];
            sourceY = SourceY;
            sourceX = SourceX;
            destinationY = DestinationY;
            destinationX = DestinationX;
            diffY = destinationY - sourceY;
            diffX = destinationX - sourceX;
            pieceStateMapping = PieceStateMapping;
            PromptedToAI = promptedToAI;
        }

        protected bool GameState(PictureBox[][] board) // determine the game state whether its check, checkmate, stalemate or normal
        {
            if (enPassantDetails != null)
                board[enPassantDetails.Y][enPassantDetails.X] = null;
            else if (castlingDetails != null) 
            {
                board[castlingDetails.SourceY][castlingDetails.SourceX] = null;
                board[castlingDetails.DestinationY][castlingDetails.DestinationX] = castlingDetails.Source;
            }
            board[sourceY][sourceX] = null;
            board[destinationY][destinationX] = source;
            
            int[] kingCoord = PieceDetails.FindKing(board, turn, pieceStateMapping);

            if (BoardCheck.Check.IsChecked(board, kingCoord[0], kingCoord[1], !turn, pieceStateMapping)) // determine if this is an illegal move by check
            {
                if (enPassantDetails != null)
                    board[enPassantDetails.Y][enPassantDetails.X] = enPassantDetails.Target;
                else if (castlingDetails != null)
                {
                    board[castlingDetails.SourceY][castlingDetails.SourceX] = castlingDetails.Source;
                    board[castlingDetails.DestinationY][castlingDetails.DestinationX] = castlingDetails.Destination;
                }
                board[sourceY][sourceX] = source;
                board[destinationY][destinationX] = destination;
                return false;
            }
            Promote promote = new Promote(source, pieceStateMapping, destinationY, turn, Main.opponent, Main.aiColor, PromptedToAI);
            ChessGame.gameState state = BoardCheck.CurrentStatus.TurnResult(board, turn, pieceStateMapping);

            if (state == ChessGame.gameState.Check)
            {
                setHistory(ChessGame.gameState.Check, promote);
            }
            else if (state == ChessGame.gameState.Checkmate)
            {
                setHistory(ChessGame.gameState.Checkmate, promote);
            }
            else if (state == ChessGame.gameState.Stalemate)
            {
                setHistory(ChessGame.gameState.Stalemate, promote);
            }
            else
            {
                setHistory(ChessGame.gameState.Normal, promote);
            }
            return true;      
        }

        private void setHistory(ChessGame.gameState state, Promote promote) // set history variables with values if turn was valid for the undo or redo feature
        {
            History currentHistory;
            if (!pieceStateMapping[source].HasMoved)
            {
                pieceStateMapping[source].HasMoved = true;
                currentHistory = new History(turn, sourceY, sourceX, destinationY, destinationX, source, destination, state, promote, enPassantable, enPassantDetails, castlingDetails, true);
            }
            else
                currentHistory = new History(turn, sourceY, sourceX, destinationY, destinationX, source, destination, state, promote, enPassantable, enPassantDetails, castlingDetails);
            if (castlingDetails != null) 
                pieceStateMapping[castlingDetails.Source].HasMoved = true;
            history.Next = currentHistory;
            currentHistory.Prev = history;
        }

        public abstract bool Move(PictureBox[][] board); // this method must be overriden by derived class for consistency
    }
}
