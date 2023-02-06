using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class Rook : Piece
    {
        public Rook(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        public override bool Move(PictureBox[][] board) // move rook
        {
            if (diffY == 0) // move rook east or west
            {
                if (diffX < 0) // moving rook west
                {
                    for (int i = sourceX - 1; i > destinationX; i--)
                    {
                        if (board[sourceY][i] != null)
                            return false;
                    }
                }
                else // moving rook east
                {
                    for (int i = sourceX + 1; i < destinationX; i++)
                    {
                        if (board[sourceY][i] != null)
                            return false;
                    }
                }
            }
            else if (diffX == 0) // move rook north or south
            {
                if (diffY < 0) // moving rook north
                {
                    for (int i = sourceY - 1; i > destinationY; i--)
                    {
                        if (board[i][sourceX] != null)
                            return false;
                    }
                }
                else // moving rook south
                {
                    for (int i = sourceY + 1; i < destinationY; i++)
                    {
                        if (board[i][sourceX] != null)
                            return false;
                    }
                }
            }
            else
                return false;
            if (GameState(board)) // determine if piece can be moved without their king being checked  
            {
                if (destination != null) // if piece is eatting another piece
                {
                    destination.Visible = false;
                    return PieceDetails.movePiece(destinationY, destinationX, source);
                }
                else  // if piece is moving to an empty square
                {
                    return PieceDetails.movePiece(destinationY, destinationX, source);
                }
            }
            return false;
        }
    }
}
