using Chess.BoardCheck;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    class DisplayPossibleMoves
    {
        int sizeOfBox = 45;
        Color backcolor;
        Graphics gObject;
        Brush brush;
        EnPassantDetails enPassantDetails = null;
        Dictionary<PictureBox, PieceStateDetails> pieceStateMapping;

        public DisplayPossibleMoves(Graphics GObject, Color color, Dictionary<PictureBox, PieceStateDetails> PieceStateMapping)
        {
            backcolor = color;
            gObject = GObject;
            brush = new SolidBrush(color);
            pieceStateMapping = PieceStateMapping;
        }

        public void DisplayMoves(int Y, int X, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
        {
            switch (sourcePieceType) 
            {
                case ChessGame.pieceName.Pawn:
                    PawnPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
                case ChessGame.pieceName.Rook:
                    RookPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
                case ChessGame.pieceName.Knight:
                    KnightPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
                case ChessGame.pieceName.Bishop:
                    BishopPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
                case ChessGame.pieceName.Queen:
                    QueenPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
                default:
                    KingPossibleMoves(Y, X, board, PossiblePieceToTake, turn, sourcePieceType, history);
                    return;
            }
        }

        private void RookPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
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
                    int Y = y + (i * PieceDetails.RookDirection[j][0]);
                    int X = x + (i * PieceDetails.RookDirection[j][1]);
                    bool isChecked = IsChecked(board, y, x, Y, X, turn);
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null) 
                    {
                        if(!isChecked)
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                    }
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        if (!isChecked) 
                        {
                            PossiblePieceToTake.Add(board[Y][X]);
                            board[Y][X].BackColor = backcolor;
                        }
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }
        }

        private void BishopPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
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
                    int Y = y + (i * PieceDetails.BishopDirection[j][0]);
                    int X = x + (i * PieceDetails.BishopDirection[j][1]);
                    bool isChecked = IsChecked(board, y, x, Y, X, turn);
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null) 
                    {
                        if (!isChecked)
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                    }
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        if (!isChecked)
                        {
                            PossiblePieceToTake.Add(board[Y][X]);
                            board[Y][X].BackColor = backcolor;
                        }
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }           
        }

        private void QueenPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
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
                    int Y = y + (i * PieceDetails.QueenDirection[j][0]);
                    int X = x + (i * PieceDetails.QueenDirection[j][1]);
                    bool isChecked = IsChecked(board, y, x, Y, X, turn);
                    if (Y < 0 || Y > 7 || X < 0 || X > 7)
                        pieceDirection[j] = true;
                    else if (board[Y][X] == null) 
                    {
                        if (!isChecked)
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                    } 
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        if (!isChecked)
                        {
                            PossiblePieceToTake.Add(board[Y][X]);
                            board[Y][X].BackColor = backcolor;
                        }
                        pieceDirection[j] = true;
                    }
                    else
                        pieceDirection[j] = true;
                }
            }
        }

        private bool AbleToCastle(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, bool turn)
        {
            PictureBox source = board[sourceY][sourceX];
            int diffY = destinationY - sourceY;
            int diffX = destinationX - sourceX;
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
                            if (CastlingCheck(board, sourceY, new int[] { sourceX, sourceX - 1, sourceX - 2 }, source, turn)) 
                            {
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
                            if (CastlingCheck(board, sourceY, new int[] { sourceX, sourceX + 1, sourceX + 2 }, source, turn)) 
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

        private bool CastlingCheck(PictureBox[][] board, int Y, int[] allPositionsX, PictureBox source, bool turn)
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
                if (Check.IsChecked(board, Y, X, !turn, pieceStateMapping)) return false;
            }
            return true;
        }

        private void KingPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
        {
            if (x + 2 < 8 && AbleToCastle(board, y, x, y, x + 2, turn))  
            {
                gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x + 2), PieceDetails.ToCoordinate(y), sizeOfBox, sizeOfBox);
            }
            if (x - 2 >= 0 && AbleToCastle(board, y, x, y, x - 2, turn)) 
            {
                gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x - 2), PieceDetails.ToCoordinate(y), sizeOfBox, sizeOfBox);
            }
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int Y = y + i;
                    int X = x + j;
                    if ((i == 0 && j == 0) || Y >= 8 || Y < 0 || X >= 8 || X < 0) continue;
                    bool isChecked = IsChecked(board, y, x, Y, X, turn);
                    if (board[Y][X] == null) 
                    {
                        if (!isChecked)
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                    }    
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        if (!isChecked)
                        {
                            PossiblePieceToTake.Add(board[Y][X]);
                            board[Y][X].BackColor = backcolor;
                        }
                    }
                }
            }
        }

        private bool AbleToEnPassant(PictureBox[][] board, int sourceY, int sourceX, bool turn, History history)
        {
            if (!history.EnPassantable) return false;
            PictureBox source = board[sourceY][sourceX];
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
                if ((targetX == sourceX - 1 || targetX == sourceX + 1) && board[enPassantY][targetX] == null)
                {
                    enPassantDetails = new EnPassantDetails(targetY, targetX, board[targetY][targetX]);
                    return true;
                }
            }
            return false;
        }

        private void PawnPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
        {
            if (turn)
            {
                if (AbleToEnPassant(board, y, x, turn, history)) 
                {
                    if (enPassantDetails == null) return;
                    // the pawn taken through En Passant will be either above or below the piece being taken depending on the turn
                    int enPassantY = turn ? enPassantDetails.Y - 1 : !turn ? enPassantDetails.Y + 1 : -1;
                    if (enPassantY == -1) return;
                    if (!IsChecked(board, y, x, enPassantY, enPassantDetails.X, turn)) 
                    {
                        gObject.FillRectangle(brush, PieceDetails.ToCoordinate(enPassantDetails.X), PieceDetails.ToCoordinate(enPassantY), sizeOfBox, sizeOfBox);
                        enPassantDetails = null;
                    }
                }
                if (y - 1 >= 0)
                {
                    if (board[y - 1][x] == null) 
                    {
                        if (!IsChecked(board, y, x, y - 1, x, turn))
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x), PieceDetails.ToCoordinate(y - 1), sizeOfBox, sizeOfBox);
                    }                      
                    if (x - 1 >= 0 && board[y - 1][x - 1] != null && PieceDetails.findSelectedPiece(board[y - 1][x - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                    {
                        if (!IsChecked(board, y, x, y - 1, x - 1, turn))
                        {
                            PossiblePieceToTake.Add(board[y - 1][x - 1]);
                            board[y - 1][x - 1].BackColor = backcolor;
                        }
                    }
                    if (x + 1 < 8 && board[y - 1][x + 1] != null && PieceDetails.findSelectedPiece(board[y - 1][x + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                    {
                        if (!IsChecked(board, y, x, y - 1, x + 1, turn))
                        {
                            PossiblePieceToTake.Add(board[y - 1][x + 1]);
                            board[y - 1][x + 1].BackColor = backcolor;
                        }
                    }
                }
                if (y == 6 && y - 2 >= 0 && board[y - 2][x] == null && board[y - 1][x] == null) 
                {
                    if (!IsChecked(board, y, x, y - 2, x, turn))
                        gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x), PieceDetails.ToCoordinate(y - 2), sizeOfBox, sizeOfBox);
                }                 
            }
            else
            {
                if (y + 1 < 8)
                {
                    if (board[y + 1][x] == null) 
                    {
                        if (!IsChecked(board, y, x, y + 1, x, turn))
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x), PieceDetails.ToCoordinate(y + 1), sizeOfBox, sizeOfBox);
                    }             
                    if (x - 1 >= 0 && board[y + 1][x - 1] != null && PieceDetails.findSelectedPiece(board[y + 1][x - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White)
                    {
                        if (!IsChecked(board, y, x, y + 1, x - 1, turn))
                        {
                            PossiblePieceToTake.Add(board[y + 1][x - 1]);
                            board[y + 1][x - 1].BackColor = backcolor;
                        }
                    }
                    if (x + 1 < 8 && board[y + 1][x + 1] != null && PieceDetails.findSelectedPiece(board[y + 1][x + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White)
                    {
                        if (!IsChecked(board, y, x, y + 1, x + 1, turn))
                        {
                            PossiblePieceToTake.Add(board[y + 1][x + 1]);
                            board[y + 1][x + 1].BackColor = backcolor;
                        }
                    }
                }
                if (y == 1 && y + 2 < 8 && board[y + 2][x] == null && board[y + 1][x] == null) 
                {
                    if (!IsChecked(board, y, x, y + 2, x, turn))
                        gObject.FillRectangle(brush, PieceDetails.ToCoordinate(x), PieceDetails.ToCoordinate(y + 2), sizeOfBox, sizeOfBox);
                }    
            }
        }

        private void KnightPossibleMoves(int y, int x, PictureBox[][] board, List<PictureBox> PossiblePieceToTake, bool turn, ChessGame.pieceName sourcePieceType, History history)
        {
            foreach (int[] dir in PieceDetails.KnightDirection)  
            {
                if ((dir[0] < 0 ? y + dir[0] >= 0 : y + dir[0] < 8) && (dir[1] < 0 ? x + dir[1] >= 0 : x + dir[1] < 8))
                {
                    int Y = y + dir[0];
                    int X = x + dir[1];
                    bool isChecked = IsChecked(board, y, x, Y, X, turn);
                    if (board[Y][X] == null) 
                    {
                        if (!isChecked)
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                    }                                      
                    else if (PieceDetails.LegalTurnMove(turn, pieceStateMapping[board[Y][X]]))
                    {
                        if (!isChecked) 
                        {
                            gObject.FillRectangle(brush, PieceDetails.ToCoordinate(X), PieceDetails.ToCoordinate(Y), sizeOfBox, sizeOfBox);
                            PossiblePieceToTake.Add(board[Y][X]);
                            board[Y][X].BackColor = backcolor;
                        }
                    }
                }
            }         
        }

        // determine if the move being made is legal
        private bool IsChecked(PictureBox[][] board, int sourceY, int sourceX, int destinationY, int destinationX, bool turn) 
        {
            if (destinationY < 0 || destinationY > 7 || destinationX < 0 || destinationX > 7) return false;
            PictureBox selectedPiece = board[sourceY][sourceX];
            PictureBox destinationPiece = board[destinationY][destinationX];
            // if source and destination are the same color piece then return
            if (destinationPiece != null && pieceStateMapping[destinationPiece].PieceColor == pieceStateMapping[selectedPiece].PieceColor) return false;
            PictureBox[][] newBoard = new PictureBox[8][];
            for (int i = 0; i < newBoard.Length; i++)
            {
                newBoard[i] = new PictureBox[8];
                board[i].CopyTo(newBoard[i], 0);
            }
            newBoard[sourceY][sourceX] = null;
            if (enPassantDetails != null) 
                newBoard[enPassantDetails.Y][enPassantDetails.X] = null;
            newBoard[destinationY][destinationX] = selectedPiece;
            int[] kingCoord = PieceDetails.FindKing(newBoard, turn, pieceStateMapping);
            return Check.IsChecked(newBoard, kingCoord[0], kingCoord[1], !turn, pieceStateMapping);
        }
    }
}
