using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.AIMove
{
    class CapturedSquares
    {
        public int capturedBoard = 0;
        private PictureBox[][] board;
        private Dictionary<PictureBox, PieceStateDetails> pieceStateMapping;
        private const int move = 2;
        private const int defense = 3;
        private const int attack = 3;

        public CapturedSquares(PictureBox[][] Board, Dictionary<PictureBox, PieceStateDetails> PieceStateMapping) 
        {
            board = Board;
            pieceStateMapping = PieceStateMapping;
        }

        public int CaptureCount()
        {
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] == null)
                        continue;
                    PieceStateDetails selectedPiece = pieceStateMapping[board[y][x]];
                    bool turn = selectedPiece.PieceColor == ChessGame.pieceColor.White ? true : false;
                    switch (selectedPiece.PieceName)
                    {
                        case ChessGame.pieceName.Pawn:
                            PawnSquaresTaken(y, x, turn);
                            break;
                        case ChessGame.pieceName.Rook:
                            RookSquaresTaken(y, x, turn);
                            break;
                        case ChessGame.pieceName.Knight:
                            KnightSquaresTaken(y, x, turn);
                            break;
                        case ChessGame.pieceName.Bishop:
                            BishopSquaresTaken(y, x, turn);
                            break;
                        case ChessGame.pieceName.Queen:
                            QueenSquaresTaken(y, x, turn);
                            break;
                        default:
                            KingSquaresTaken(y, x, turn);
                            break;
                    }
                }
            }
            return capturedBoard;
        }

        private void capturedIncrDecr(int y, int x, bool turn)
        {
            // this will increase or decrease the captured value by an certain amount depending on whether a piece is being defended, attacked or moved
            if (turn)
            {
                if (board[y][x] == null)
                    capturedBoard = capturedBoard + move;
                else
                {
                    PieceStateDetails target = pieceStateMapping[board[y][x]];
                    if (target.PieceColor == ChessGame.pieceColor.White)
                        capturedBoard = capturedBoard + defense;
                    else 
                        capturedBoard = capturedBoard + attack;
                }
                
            }
            else
            {
                if (board[y][x] == null)
                    capturedBoard = capturedBoard - move;
                else
                {
                    PieceStateDetails target = pieceStateMapping[board[y][x]];
                    if (target.PieceColor == ChessGame.pieceColor.Black)
                        capturedBoard = capturedBoard - defense;
                    else
                        capturedBoard = capturedBoard - attack;
                }
            }
        }

        private void RookSquaresTaken(int y, int x, bool turn)
        {
            bool[] pieceDirection = new bool[4]; // this array represents north, east, south, west and will reduce the processing time
            for (int i = 1; i < 8; i++)
            {
                // rook cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.RookDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.RookDirection[j][0];
                    int X = x + i * PieceDetails.RookDirection[j][1];
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null)
                        capturedIncrDecr(Y, X, turn);
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        capturedIncrDecr(Y, X, turn);
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }
        }

        private void BishopSquaresTaken(int y, int x, bool turn)
        {
            bool[] pieceDirection = new bool[4]; // this array represents northeast, southeast, southwest, northwest and will reduce the processing time
            for (int i = 1; i < 8; i++)
            {
                // bishop cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.BishopDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.BishopDirection[j][0];
                    int X = x + i * PieceDetails.BishopDirection[j][1];
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null)
                        capturedIncrDecr(Y, X, turn);
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        capturedIncrDecr(Y, X, turn);
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }
        }

        private void QueenSquaresTaken(int y, int x, bool turn)
        {
            bool[] pieceDirection = new bool[8]; // this array represents north, east, south, west, northeast, southeast, southwest, northwest and will reduce the processing time
            for (int i = 1; i < 8; i++)
            {
                // bishop cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.QueenDirection.Length; j++)
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.QueenDirection[j][0];
                    int X = x + i * PieceDetails.QueenDirection[j][1];
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null)
                        capturedIncrDecr(Y, X, turn);
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        capturedIncrDecr(Y, X, turn);
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }

        }

        private void KingSquaresTaken(int y, int x, bool turn)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int Y = y + i;
                    int X = x + j;
                    if ((i == 0 && j == 0) || Y >= 8 || Y < 0 || X >= 8 || X < 0) continue;
                    if (board[Y][X] == null)
                        capturedIncrDecr(Y, X, turn);
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                        capturedIncrDecr(Y, X, turn);
                }
            }
        }

        private void PawnSquaresTaken(int Y, int X, bool turn)
        {
            if (turn)
            {
                if (Y - 1 >= 0)
                {
                    if (board[Y - 1][X] == null)
                        capturedIncrDecr(Y - 1, X, turn);
                    if (X - 1 >= 0 && board[Y - 1][X - 1] != null && PieceDetails.findSelectedPiece(board[Y - 1][X - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                        capturedIncrDecr(Y - 1, X - 1, turn);
                    if (X + 1 < 8 && board[Y - 1][X + 1] != null && PieceDetails.findSelectedPiece(board[Y - 1][X + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                        capturedIncrDecr(Y - 1, X + 1, turn);
                }
                if (Y == 6 && Y - 2 >= 0 && board[Y - 2][X] == null && board[Y - 1][X] == null)
                    capturedIncrDecr(Y - 2, X, turn);
            }
            else
            {
                if (Y + 1 < 8)
                {
                    if (board[Y + 1][X] == null)
                        capturedIncrDecr(Y + 1, X, turn);
                    if (X - 1 >= 0 && board[Y + 1][X - 1] != null && PieceDetails.findSelectedPiece(board[Y + 1][X - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White)
                        capturedIncrDecr(Y + 1, X - 1, turn);
                    if (X + 1 < 8 && board[Y + 1][X + 1] != null && PieceDetails.findSelectedPiece(board[Y + 1][X + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White)
                        capturedIncrDecr(Y + 1, X + 1, turn);
                }
                if (Y == 1 && Y + 2 < 8 && board[Y + 2][X] == null && board[Y + 1][X] == null)
                    capturedIncrDecr(Y + 2, X, turn);
            }
        }

        private void KnightSquaresTaken(int y, int x, bool turn)
        {
            foreach (int[] dir in PieceDetails.KnightDirection)
            {
                int Y = y + dir[0];
                int X = x + dir[1];
                if ((dir[0] < 0 ? Y >= 0 : Y < 8) && (dir[1] < 0 ? X >= 0 : X < 8))
                {
                    if (board[Y][X] == null)
                        capturedIncrDecr(Y, X, turn);
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                        capturedIncrDecr(Y, X, turn);
                }
            }
        }
    }
}
