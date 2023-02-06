using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.PieceMove
{
    class Pawn : Piece
    {
        public Pawn(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, History history, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo, ChessGame main)
        {
            InitalizePiece(board, turn, sourceY, sourceX, destinationY, destinationX, history, pieceStateMapping, promptedTo, main);
        }

        public override bool Move(PictureBox[][] board) // move pawn
        {
            if (AbleToEnPassant(board)) // pawn can En Passant
            {
                if (GameState(board)) 
                {
                    enPassantDetails.Target.Visible = false;
                    return PieceDetails.movePiece(destinationY, destinationX, source);
                }
            }
            if ((turn && diffY == -1) || (!turn && diffY == 1)) // pawn can only move forward and not backwards
            {
                if (destination != null && (diffX == 1 || diffX == -1)) // if moving diagonally pawn must not land on an empty square
                {
                    if (GameState(board)) // determine if piece can be moved without their king being checked  
                    {
                        destination.Visible = false;
                        return PieceDetails.movePiece(destinationY, destinationX, source);
                    }
                }
                else if (destination == null && sourceX == destinationX) // if moving forward but only one square forward
                {
                    if (GameState(board)) // determine if piece can be moved without their king being checked  
                    {
                        return PieceDetails.movePiece(destinationY, destinationX, source);
                    }
                }
            }
            else if (destination == null && sourceX == destinationX && ((turn && diffY == -2 && board[sourceY - 1][sourceX] == null && sourceY == 6) || (!turn && diffY == 2 && board[sourceY + 1][sourceX] == null && sourceY == 1)))
            {
                // moving pawn forward two blocks must land on an empty square and must not be moved before
                enPassantable = true;
                if (GameState(board)) // determine if piece can be moved without their king being checked  
                {
                    return PieceDetails.movePiece(destinationY, destinationX, source);
                }
            }
            return false;
        }

        private bool AbleToEnPassant(PictureBox[][] board) 
        {
            if (!history.EnPassantable) return false;
            PieceStateDetails targetSource = pieceStateMapping[history.Source];
            PieceStateDetails pieceSource = pieceStateMapping[source];
            // En Passant will not work if the piece that the source piece is trying to eat is on their side
            if (targetSource.PieceName == ChessGame.pieceName.Pawn && ((turn && targetSource.PieceColor == ChessGame.pieceColor.Black && pieceSource.PieceColor == ChessGame.pieceColor.White) || (!turn && targetSource.PieceColor == ChessGame.pieceColor.White && pieceSource.PieceColor == ChessGame.pieceColor.Black)))
            {
                int targetY = history.DestinationY; int targetX = history.DestinationX;
                int adjacentLeftX = targetX - 1; int adjacentRightX = targetX + 1;
                if (targetY < 0 || targetY > 7 || sourceY < 0 || sourceY > 7) return false;
                // check if source piece is next to the target piece either from the left or right side of the x axis
                if (!(targetY == sourceY && ((adjacentLeftX >= 0 && sourceX == adjacentLeftX) || (adjacentRightX < 8 && sourceX == adjacentRightX)))) return false;
                int enPassantY = pieceSource.PieceColor == ChessGame.pieceColor.White ? sourceY - 1 : pieceSource.PieceColor == ChessGame.pieceColor.Black ? sourceY + 1 : -1;
                if (enPassantY < 0 || enPassantY > 7 || targetX < 0 || targetX > 7) return false;
                // the destination position must be on the same x axis as the target piece and it has to be an empty square to be able to En Passant
                if (enPassantY == destinationY && targetX == destinationX && destination == null) 
                {
                    enPassantDetails = new EnPassantDetails(targetY, targetX, board[targetY][targetX]);
                    return true;
                }
            }
            return false;
        }
    }
}
