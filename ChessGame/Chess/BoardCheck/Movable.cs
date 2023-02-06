using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.BoardCheck
{
    class Movable
    {
        public bool PawnMove(PictureBox[][] board, int y, int x, bool turn, int targetY, int targetX, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // pawn move
        {
            if (turn)
            {
                // if white pawn can move one square forward (does not need to check if it can two squares forward as we just need to find if it can move without being checked)
                if (y - 1 >= 0)
                {
                    if (board[y - 1][x] == null) // white pawn move forward one square and must be null
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y - 1, x, !turn, targetY, targetX, pieceStateMapping)) // determine if white pawn can move without being checked
                            return true;
                    }
                    if (x - 1 >= 0 && board[y - 1][x - 1] != null && pieceStateMapping[board[y - 1][x - 1]].PieceColor == ChessGame.pieceColor.Black) // white pawn eat another piece at north-west 
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y - 1, x - 1, !turn, targetY, targetX, pieceStateMapping))
                            return true;
                    }
                    if (x + 1 < 8 && board[y - 1][x + 1] != null && pieceStateMapping[board[y - 1][x + 1]].PieceColor == ChessGame.pieceColor.Black) // white pawn eat another piece at north-east
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y - 1, x + 1, !turn, targetY, targetX, pieceStateMapping))
                            return true;
                    }
                }
            }
            else if (!turn)
            {
                // if black pawn can move one square forward (does not need to check if it can two squares forward as we just need to find if it can move without being checked)
                if (y + 1 < 8)
                {
                    if (board[y + 1][x] == null) // black pawn move forward one square and must be null
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y + 1, x, !turn, targetY, targetX, pieceStateMapping)) // determine if white pawn can move without being checked
                            return true;
                    }
                    else if (x - 1 >= 0 && board[y + 1][x - 1] != null && pieceStateMapping[board[y + 1][x - 1]].PieceColor == ChessGame.pieceColor.White) // black pawn eat another piece at south-west
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y + 1, x - 1, !turn, targetY, targetX, pieceStateMapping))
                            return true;
                    }
                    else if (x + 1 < 8 && board[y + 1][x + 1] != null && pieceStateMapping[board[y + 1][x + 1]].PieceColor == ChessGame.pieceColor.White) // black pawn eat another piece at south-west
                    {
                        if (Check.IsAbleToMovePiece(board, y, x, y + 1, x + 1, !turn, targetY, targetX, pieceStateMapping))
                            return true;
                    }
                }
            }
            return false;
        }

        public bool RookMove(PictureBox[][] board, int y, int x, bool turn, int targetY, int targetX, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // rook move
        {
            for (int i = 0; i < PieceDetails.RookDirection.Length; i++) // looks at each directions rook can travel: north, east, south, west
            {
                // only one square for each direction needs to be checked as it only needs to determine rook can be moved
                int Y = y + 1 * PieceDetails.RookDirection[i][0];
                int X = x + 1 * PieceDetails.RookDirection[i][1];
                if (Y < 0 || Y > 7 || X < 0 || X > 7) continue;
                PieceStateDetails selectedPiece = board[Y][X] != null ? pieceStateMapping[board[Y][X]] : null;
                if (PieceDetails.LegalTurnMoveWithNull(turn, selectedPiece))
                {
                    if (Check.IsAbleToMovePiece(board, y, x, Y, X, !turn, targetY, targetX, pieceStateMapping)) // determine if rook can move without being checked     
                        return true;
                }
            }
            return false;
        }

        public bool KnightMove(PictureBox[][] board, int y, int x, bool turn, int targetY, int targetX, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // knight move
        {
            foreach (int[] dir in PieceDetails.KnightDirection)
            {
                int Y = y + dir[0];
                int X = x + dir[1];
                if (Y < 0 || X < 0 || Y > 7 || X > 7) continue; // knight cannot move out of bounds
                PieceStateDetails selectedPiece = board[Y][X] != null ? pieceStateMapping[board[Y][X]] : null;
                if (PieceDetails.LegalTurnMoveWithNull(turn, selectedPiece))
                {
                    if (Check.IsAbleToMovePiece(board, y, x, Y, X, !turn, targetY, targetX, pieceStateMapping)) // determine if knight can move without being checked   
                        return true;
                }
            }
            return false;
        }

        public bool BishopMove(PictureBox[][] board, int y, int x, bool turn, int targetY, int targetX, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // bishop move
        {
            for (int i = 0; i < PieceDetails.BishopDirection.Length; i++) // looks at each directions bishop can travel: northeast, southeast, southwest, northwest
            {
                // only one square for each direction needs to be checked as it only needs to determine bishop can be moved
                int Y = y + 1 * PieceDetails.BishopDirection[i][0];
                int X = x + 1 * PieceDetails.BishopDirection[i][1];
                if (Y < 0 || Y > 7 || X < 0 || X > 7) continue;
                PieceStateDetails selectedPiece = board[Y][X] != null ? pieceStateMapping[board[Y][X]] : null;
                if (PieceDetails.LegalTurnMoveWithNull(turn, selectedPiece))
                {
                    if (Check.IsAbleToMovePiece(board, y, x, Y, X, !turn, targetY, targetX, pieceStateMapping)) // determine if bishop can move without being checked     
                        return true;
                }
            }
            return false;
        }

        public bool QueenMove(PictureBox[][] board, int y, int x, bool turn, int targetY, int targetX, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // queen move
        {

            for (int i = 0; i < PieceDetails.QueenDirection.Length; i++) // looks at each directions queen can travel: north, east, south, west, northeast, southeast, southwest, northwest
            {
                // only one square for each direction needs to be checked as it only needs to determine queen can be moved
                int Y = y + 1 * PieceDetails.QueenDirection[i][0];
                int X = x + 1 * PieceDetails.QueenDirection[i][1];
                if (Y < 0 || Y > 7 || X < 0 || X > 7) continue;
                PieceStateDetails selectedPiece = board[Y][X] != null ? pieceStateMapping[board[Y][X]] : null;
                if (PieceDetails.LegalTurnMoveWithNull(turn, selectedPiece))
                {
                    if (Check.IsAbleToMovePiece(board, y, x, Y, X, !turn, targetY, targetX, pieceStateMapping)) // determine if queen can move without being checked     
                        return true;
                }
            }
            return false;
        }
    }
}

