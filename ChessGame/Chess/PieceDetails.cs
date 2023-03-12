using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace Chess
{
    static class PieceDetails
    {
        static private int[][] queenDirection = new int[][] { new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 }, new int[] { -1, 1 }, new int[] { 1, 1 }, new int[] { 1, -1 }, new int[] { -1, -1 } };
        // bishopDirection represents all the moves that bishop can make
        static private int[][] bishopDirection = new int[][] { new int[] { -1, 1 }, new int[] { 1, 1 }, new int[] { 1, -1 }, new int[] { -1, -1 } };
        // rookDirection represents all the moves that rook can make
        static private int[][] rookDirection = new int[][] { new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 1, 0 }, new int[] { 0, -1 } };
        // knightDirection represents all the moves that knight can make
        static private int[][] knightDirection = new int[][] { new int[] { -2, 1 }, new int[] { -2, -1 }, new int[] { 2, 1 }, new int[] { 2, -1 }, new int[] { 1, 2 }, new int[] { 1, -2 }, new int[] { -1, 2 }, new int[] { -1, -2 } };
        // queenDirection represents all the moves that queen can make

        static public int[][] QueenDirection { get { return queenDirection; } }
        static public int[][] BishopDirection { get { return bishopDirection; } }
        static public int[][] RookDirection { get { return rookDirection; } }
        static public int[][] KnightDirection { get { return knightDirection; } }

        static public bool checkedAllDirections(bool[] directions) // determine if all valid piece directions has already been checked
        {
            foreach (bool dir in directions)
            {
                if (!dir)
                    return false;
            }
            return true;
        }

        static public bool movePiece(int destinationY, int destinationX, PictureBox source) // allows to move the picture box represented as a chess piece on the panel
        {
            source.Location = new Point(ToCoordinate(destinationX), ToCoordinate(destinationY));
            return true;
        }

        static public int FindCoordinate(int coord) 
        {
            for (int i = 0; i < 8; i++)
                if (coord >= 45 * i && coord < 45 * (i + 1))
                    return i;
            return -1;
        }

        static public int ToCoordinate(int axis) 
        {
            return axis * 45;
        }

        static public PieceStateDetails findSelectedPiece(PictureBox piece, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            return pieceStateMapping[piece];
        }
        
        // check if piece is able to move by taking another piece based on the turn
        static public bool LegalTurnMove(bool turn, PieceStateDetails selectedPiece)
        {
            return (turn && selectedPiece.PieceColor == ChessGame.pieceColor.Black) || (!turn && selectedPiece.PieceColor == ChessGame.pieceColor.White);
        }

        // check if piece is able to move based moving to a square (it can be moving to empty square or taking a piece which is based on the turn)
        static public bool LegalTurnMoveWithNull(bool turn, PieceStateDetails selectedPiece)
        {
            return selectedPiece == null || (turn && selectedPiece.PieceColor == ChessGame.pieceColor.Black) || (!turn && selectedPiece.PieceColor == ChessGame.pieceColor.White);
        }

        // check if piece that is selected is able to be moved based on the turn
        static public bool LegalSelectedPiece(bool turn, PieceStateDetails selectedPiece) 
        {
            return (turn && selectedPiece.PieceColor == ChessGame.pieceColor.White) || (!turn && selectedPiece.PieceColor == ChessGame.pieceColor.Black);
        }

        static public int[] FindKing(PictureBox[][] board, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) // find the location of the king to find if piece can be moved without being checked or if it may result in a check, checkmate or stalemate
        {
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] == null) continue;
                    PieceStateDetails piece = findSelectedPiece(board[y][x], pieceStateMapping);
                    if (piece.PieceName == ChessGame.pieceName.King && 
                        ((turn && piece.PieceColor == ChessGame.pieceColor.White) || (!turn && piece.PieceColor == ChessGame.pieceColor.Black)))
                        return new int[] { y, x };
                }
            }
            throw null;
        }
    }
}
