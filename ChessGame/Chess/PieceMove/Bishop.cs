using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class Bishop : Piece
    {
        public Bishop(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        public override bool Move(PictureBox[][] board) // move bishop
        {
            double diffYX = diffX != 0 ? ((double)diffY / (double)diffX) : 0; // diffYX determines if bishop is valid diagonal move where it must be -1 or 1
            if (diffYX == 1) // move bishop diagonally south-east or north-west
            {
                if (diffY > 0 && diffX > 0) // moving bishop south-east
                {
                    for (int y = sourceY + 1, x = sourceX + 1; y < destinationY && x < destinationX; y++, x++)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
                else // moving bishop north-west
                {
                    for (int y = sourceY - 1, x = sourceX - 1; y > destinationY && x > destinationX; y--, x--)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
            }
            else if (diffYX == -1) // move bishop diagonally north-east or south-west
            {
                if (diffY < 0 && diffX > 0) // moving bishop north-east
                {
                    for (int y = sourceY - 1, x = sourceX + 1; y > destinationY && x < destinationX; y--, x++)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
                else // moving bishop south-west
                {
                    for (int y = sourceY + 1, x = sourceX - 1; y < destinationY && x > destinationX; y++, x--)
                    {
                        if (board[y][x] != null)
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
                else // if piece is moving to an empty square
                {
                    return PieceDetails.movePiece(destinationY, destinationX, source);
                }
            }
            return false;
        }
    }
}
