using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class Knight : Piece
    {
        public Knight(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        public override bool Move(PictureBox[][] board) // move knight
        {
            foreach (int[] dir in PieceDetails.KnightDirection) // loop through all directions knight can make
            {
                if (diffY == dir[0] && diffX == dir[1])
                {
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
            return false;
        }
    }
}
