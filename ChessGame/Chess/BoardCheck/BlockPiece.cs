using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.BoardCheck
{
    class BlockPiece
    {
        public bool BlockMultipleTargets(PictureBox[][] board, bool turn, Dictionary<int, HashSet<int>> reach, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            // loop through board to look for whether a piece can block the piece checking the king represented as reach (hashset)
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] == null) continue;
                    PieceStateDetails selectedPiece = pieceStateMapping[board[y][x]];
                    // determine if selected piece can block the piece checking the king
                    if (!PieceDetails.LegalSelectedPiece(turn, selectedPiece)) continue;
                    switch (selectedPiece.PieceName)
                    {
                        case ChessGame.pieceName.Pawn:
                            if (PawnBlock(board, y, x, turn, reach))
                                return true;
                            break;
                        case ChessGame.pieceName.Rook:
                            if (RookBlock(board, y, x, reach))
                                return true;
                            break;
                        case ChessGame.pieceName.Knight:
                            if (KnightBlock(y, x, reach))
                                return true;
                            break;
                        case ChessGame.pieceName.Bishop:
                            if (BishopBlock(board, y, x, reach))
                                return true;
                            break;
                        case ChessGame.pieceName.Queen:
                            if (QueenBlock(board, y, x, reach))
                                return true;
                            break;
                    }
                }
            }
            return false;
        }

        private bool PawnBlock(PictureBox[][] board, int y, int x, bool turn, Dictionary<int, HashSet<int>> targets)
        {
            if (turn && y - 1 >= 0) // white pawn block
            {
                if (targets.ContainsKey(y - 1)) // pawn move north, north-west, north-east one square
                {
                    if (board[y - 1][x] == null && targets[y - 1].Contains(x)) // can only move north one square if it is an empty square
                        return true;
                    // can only move north-east or north-west if there is an opposite piece there and the piece must also be the piece blocking the king
                    if ((x - 1 >= 0 && board[y - 1][x - 1] != null && targets[y - 1].Contains(x - 1)) || (x + 1 < 8 && board[y - 1][x + 1] != null && targets[y - 1].Contains(x + 1)))
                        return true;
                } // can only move two squares north if it has not moved before and square must be empty and must block the piece checking the king
                else if (y - 2 >= 0 && y == 6 && board[y - 1][x] == null && board[y - 2][x] == null && targets.ContainsKey(y - 2) && targets[y - 2].Contains(x))
                    return true;
            }
            if (!turn && y + 1 < 8) // black pawn block
            {
                if (targets.ContainsKey(y + 1)) // pawn move south, south-west, south-east one square
                {
                    if (board[y + 1][x] == null && targets[y + 1].Contains(x)) // can only move south one square if it is an empty square
                        return true;
                    // can only move south-east or south-west if there is an opposite piece there and the piece must also be the piece blocking the king
                    if ((x - 1 >= 0 && board[y + 1][x - 1] != null && targets[y + 1].Contains(x - 1)) || (x + 1 < 8 && board[y + 1][x + 1] != null && targets[y + 1].Contains(x + 1)))
                        return true;
                } // can only move two squares south if it has not moved before and square must be empty and must block the piece checking the king
                else if (y + 2 < 8 && y == 1 && board[y + 1][x] == null && board[y + 2][x] == null && targets.ContainsKey(y + 2) && targets[y + 2].Contains(x))
                    return true;
            }
            return false;
        }

        private bool RookBlock(PictureBox[][] board, int y, int x, Dictionary<int, HashSet<int>> targets)
        {
            bool[] pieceDirection = new bool[4]; // this array represents north, east, south, west and will reduce the processing time
            for (int i = 1; i <= 7; i++)
            {
                // rook cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.RookDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.RookDirection[j][0];
                    int X = x + i * PieceDetails.RookDirection[j][1];
                    if (targets.ContainsKey(Y) && targets[Y].Contains(X)) // determine if rook can block the piece checking the king 
                        return true;
                    else if (Y < 0 || Y > 7 || X < 0 || X > 7 || board[Y][X] != null) // rook cannot move anymore in this direction as it is either out of bound or there is another piece blocking it from its target
                        pieceDirection[j] = true;
                }
            }
            return false;
        }

        private bool KnightBlock(int y, int x, Dictionary<int, HashSet<int>> targets)
        {
            foreach (int[] dir in PieceDetails.KnightDirection) // loop through all knight moves
            {
                if (targets.ContainsKey(y + dir[0]) && targets[y + dir[0]].Contains(x + dir[1])) // determine if knight can block the piece checking the king
                    return true;
            }
            return false;
        }

        private bool BishopBlock(PictureBox[][] board, int y, int x, Dictionary<int, HashSet<int>> targets)
        {
            bool[] pieceDirection = new bool[4]; // this array represents northeast, southeast, southwest, northwest and will reduce the processing time
            for (int i = 1; i <= 7; i++)
            {
                // bishop cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.BishopDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.BishopDirection[j][0];
                    int X = x + i * PieceDetails.BishopDirection[j][1];
                    if (targets.ContainsKey(Y) && targets[Y].Contains(X)) // determine if bishop can block the piece checking the king 
                        return true;
                    else if (Y < 0 || Y > 7 || X < 0 || X > 7 || board[Y][X] != null) // bishop cannot move anymore in this direction as it is either out of bound or there is another piece blocking it from its target
                        pieceDirection[j] = true;
                }
            }
            return false;
        }

        private bool QueenBlock(PictureBox[][] board, int y, int x, Dictionary<int, HashSet<int>> targets)
        {
            bool[] pieceDirection = new bool[8]; // this array represents north, east, south, west, northeast, southeast, southwest, northwest and will reduce the processing time
            for (int i = 1; i <= 7; i++)
            {
                // queen cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.QueenDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.QueenDirection[j][0];
                    int X = x + i * PieceDetails.QueenDirection[j][1];
                    if (targets.ContainsKey(Y) && targets[Y].Contains(X)) // determine if queen can block the piece checking the king 
                        return true;
                    else if (Y < 0 || Y > 7 || X < 0 || X > 7 || board[Y][X] != null) // queen cannot move anymore in this direction as it is either out of bound or there is another piece blocking it from its target
                        pieceDirection[j] = true;
                }
            }
            return false;
        }
    }
}
