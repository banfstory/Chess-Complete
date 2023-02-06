using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class King : Piece
    {
        public King(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        override public bool Move(PictureBox[][] board) // move king - can only move one block in each direction
        {
            double diffYX = diffX != 0 ? (diffY / diffX) : 0;
            if (AbleToCastle(board)) // castling the king and rook
            {
                if (castlingDetails != null)
                    PieceDetails.movePiece(castlingDetails.DestinationY, castlingDetails.DestinationX, castlingDetails.Source);
                return moveKing(board);
            }
            else if (diffY == 0 && (diffX == -1 || diffX == 1)) // moving east or west
            {
                return moveKing(board);
            }
            else if (diffX == 0 && (diffY == -1 || diffY == 1)) // moving north or south
            {
                return moveKing(board);
            }
            else if (diffYX == 1 && ((diffY == 1 && diffX == 1) || (diffY == -1 && diffX == -1))) // move south-east or north-west
            {
                return moveKing(board);
            }
            else if (diffYX == -1 && ((diffY == -1 && diffX == 1) || (diffY == 1 && diffX == -1))) // move north-east or south-west
            {
                return moveKing(board);
            }
            return false;
        }

        private bool moveKing(PictureBox[][] board) 
        {
            if (GameState(board))  // determine if piece can be moved without their king being checked         
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

        private bool AbleToCastle(PictureBox[][] board) 
        {
            if (!pieceStateMapping[source].HasMoved && diffY == 0 && (diffX == -2 || diffX == 2)) 
            {
                if (board[destinationY][destinationX] != null) return false;
                int fromX = sourceX;
                if (diffX < 0)
                {
                    for (int i = fromX - 1; i >= 0; i--) 
                    {
                        PictureBox currentPiece = board[sourceY][i];
                        if (currentPiece == null) continue;
                        PieceStateDetails pieceStateDetails = pieceStateMapping[currentPiece];
                        // this has to be a rook and the king has to be the same color as the rook and the rook has to be more than 2 squares away from the king and rook has not moved
                        if (pieceStateDetails.PieceName == ChessGame.pieceName.Rook && pieceStateDetails.PieceColor == pieceStateMapping[source].PieceColor && sourceX - i > 2 && !pieceStateDetails.HasMoved)
                        {
                            if (CastlingCheck(board, sourceY, new int[] { sourceX, sourceX - 1, sourceX - 2 }))
                            {
                                castlingDetails = new CastlingDetails(sourceY, i, sourceY, sourceX - 1, currentPiece, board[sourceY][sourceX - 1]);
                                return true;
                            }
                        }
                        return false;
                    }
                }
                else                
                {
                    for (int i = fromX + 1; i < 8; i++) 
                    {
                        PictureBox currentPiece = board[sourceY][i];                     
                        if (currentPiece == null) continue;
                        PieceStateDetails pieceStateDetails = pieceStateMapping[currentPiece];
                        // this has to be a rook and the king has to be the same color as the rook and the rook has to be more than 2 squares away from the king and rook has not moved
                        if (pieceStateDetails.PieceName == ChessGame.pieceName.Rook && pieceStateDetails.PieceColor == pieceStateMapping[source].PieceColor && i - sourceX > 2 && !pieceStateDetails.HasMoved)  
                        {
                            if (CastlingCheck(board, sourceY, new int[] { sourceX, sourceX + 1, sourceX + 2 }))   
                            {
                                castlingDetails = new CastlingDetails(sourceY, i, sourceY, sourceX + 1, currentPiece, board[sourceY][sourceX + 1]);
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        private bool CastlingCheck(PictureBox[][] board, int Y, int[] allPositionsX)
        {
            PictureBox[][] newBoard = new PictureBox[8][];
            for (int i = 0; i < newBoard.Length; i++)
            {
                newBoard[i] = new PictureBox[8];
                board[i].CopyTo(newBoard[i], 0);
            }
            int prevX = -1;
            for (int i = 0; i < allPositionsX.Length; i++) 
            {
                int X = allPositionsX[i];
                if (prevX == -1)
                    prevX = X;
                else
                {
                    newBoard[Y][prevX] = null;
                    newBoard[Y][X] = source;
                    prevX = X;
                }
                if (BoardCheck.Check.IsChecked(board, Y, X, !turn, pieceStateMapping)) return false;
            }
            return true;
        }
    }
}
