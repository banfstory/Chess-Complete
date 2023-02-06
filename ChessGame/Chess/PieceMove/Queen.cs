using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class Queen : Piece
    {
        public Queen(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        public override bool Move(PictureBox[][] board) // move queen
        {
            double diffYX = diffX != 0 ? ((double)diffY / (double)diffX) : 0; // determine if queen is moved diagonally, vertically or horizontally
            if (diffY == 0) // move queen east or west
            {
                if (diffX < 0) // moving queen west
                {
                    for (int i = sourceX - 1; i > destinationX; i--)
                    {
                        if (board[sourceY][i] != null)
                            return false;
                    }
                }
                else // moving queen east
                {
                    for (int i = sourceX + 1; i < destinationX; i++)
                    {
                        if (board[sourceY][i] != null)
                            return false;
                    }
                }
            }
            else if (diffX == 0) // move queen north or south
            {
                if (diffY < 0) // moving queen north
                {
                    for (int i = sourceY - 1; i > destinationY; i--)
                    {
                        if (board[i][sourceX] != null)
                            return false;
                    }
                }
                else // moving queen south
                {
                    for (int i = sourceY + 1; i < destinationY; i++)
                    {
                        if (board[i][sourceX] != null)
                            return false;
                    }
                }
            }
            else if (diffYX == 1) // move queen diagonally south-east or north-west
            {
                if (diffY > 0 && diffX > 0) // moving queen south-east
                {
                    for (int y = sourceY + 1, x = sourceX + 1; y < destinationY && x < destinationX; y++, x++)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
                else // moving queen north-west
                {
                    for (int y = sourceY - 1, x = sourceX - 1; y > destinationY && x > destinationX; y--, x--)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
            }
            else if (diffYX == -1) // move queen diagonally north-east or south-west
            {
                if (diffY < 0 && diffX > 0) // moving queen north-east
                {
                    for (int y = sourceY - 1, x = sourceX + 1; y > destinationY && x < destinationX; y--, x++)
                    {
                        if (board[y][x] != null)
                            return false;
                    }
                }
                else // moving queen south-west
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
