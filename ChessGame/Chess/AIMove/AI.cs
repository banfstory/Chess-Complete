using Chess.PieceMove;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess.AIMove
{
    class AI // Minimax with Alpha Beta Pruning Algorithm (the way pruning works is if for that node a queen piece is taken, then any other pieces taken than is worth less than a queen for that node is pruned out)
    {
        private int movesCount;
        private int bestPath;
        private int squareCount;
        private const int kingvalue = 10000;
        private int BoardState; // determine the current best board state which will be used to prune results from a path that may provide a worse result
        private int SourceY;
        private int SourceX;
        private int DestinationY;
        private int DestinationX;
        private AIResult FinalResult;
        // 'PromptedTo' should be 'None' if it is not a pawn prompted to another piece
        private ChessGame.pieceName PromptedTo = ChessGame.pieceName.None;

        public AI(int count, AIResult finalResult = null)
        {
            movesCount = count;
            FinalResult = finalResult;
        }

        // if it is currently at white's turn, it will try to look for the maximum boardstate value but if it is at black's turn, it will look for the minimum boardstate
        public int MiniMax(PictureBox[][] board, bool turn, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, EnPassantDetails enPassantDetails = null)
        {
            if (movesCount == movesLimit) // this will be the base case which is an end point where this end value will be compared with other end values
                return currentBoardState;
            BoardState = turn ? int.MinValue : int.MaxValue; // boardstate looks at the materials represented in the current board (white looks for the maximum board state and black looks for the minimum board state)
            squareCount = turn ? int.MinValue : int.MaxValue; // squarecount looks at the number of spaces that can be moved represented in the current board (white looks for the maximum squarecount and black looks for the minimum squarecount - only compares when movecount is 0)
            bestPath = currentBoardState;
            for (int y = 0; y < board.Length; y++)
            {
                for (int x = 0; x < board[y].Length; x++)
                {
                    if (board[y][x] == null) continue;
                    // look for the piece that corresponds with its own selected piece (black must select a black piece and white must select a white piece)
                    PieceStateDetails selectedPiece = pieceStateMapping[board[y][x]];
                    if (!PieceDetails.LegalSelectedPiece(turn, selectedPiece)) continue;
                    switch (selectedPiece.PieceName)
                    {
                        // if these return true than stop performing the best path calculation and break out of this path as king has being taken by anothe piece
                        case ChessGame.pieceName.Pawn:
                            AIPruning prunePawn = PawnAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping, enPassantDetails);
                            if (prunePawn.Prune)
                                return prunePawn.Value;
                            break;
                        case ChessGame.pieceName.Rook:
                            AIPruning pruneRook = RookAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping);
                            if (pruneRook.Prune)
                                return pruneRook.Value;
                            break;
                        case ChessGame.pieceName.Knight:
                            AIPruning pruneKnight = KnightAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping);
                            if (pruneKnight.Prune)
                                return pruneKnight.Value;
                            break;
                        case ChessGame.pieceName.Bishop:
                            AIPruning pruneBishop = BishopAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping);
                            if (pruneBishop.Prune)
                                return pruneBishop.Value;
                            break;
                        case ChessGame.pieceName.Queen:
                            AIPruning pruneQueen = QueenAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping);
                            if (pruneQueen.Prune)
                                return pruneQueen.Value;
                            break;
                        case ChessGame.pieceName.King:
                            AIPruning pruneKing = KingAI(board, y, x, turn, movesCount, movesLimit, currentBoardState, pieceStateMapping);
                            if (pruneKing.Prune)
                                return pruneKing.Value;
                            break;
                    }
                }
            }
            if (movesCount == 0 && FinalResult != null) // this will store the beginning point of the best path which will allow AI to make that specific move
            {
                FinalResult.SourceY = SourceY;
                FinalResult.SourceX = SourceX;
                FinalResult.DestinationY = DestinationY;
                FinalResult.DestinationX = DestinationX;
                FinalResult.PromptedTo = PromptedTo;
            }
            return BoardState;
        }

        private AIPruning PawnAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, EnPassantDetails enPassantDetails)
        {
            PictureBox piece = board[y][x];
            PieceStateMappingDetails pieceMappingDetails = setPieceMappingDetailsTurn(piece, pieceStateMapping);
            if (turn) // white pawn move
            {
                // white pawn enPassant
                if (enPassantDetails != null && enPassantDetails.Y == y && (enPassantDetails.X == x - 1 || enPassantDetails.X == x + 1))
                {
                    PieceStateDetails enPassantTarget = pieceStateMapping[enPassantDetails.Target];
                    if (enPassantTarget.PieceColor == ChessGame.pieceColor.Black && enPassantTarget.PieceName == ChessGame.pieceName.Pawn)
                    {
                        int Y = enPassantDetails.Y - 1;
                        int X = enPassantDetails.X;
                        if (Y >= 0 && board[Y][X] == null)
                        {
                            int value = PieceValueColor(pieceStateMapping[enPassantDetails.Target]);
                            AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X], AIMoveType.moveType.EnPassant, enPassantDetails, null);
                            moveDetails.SetPieceToBoard(board);
                            board[enPassantDetails.Y][enPassantDetails.X] = null;
                            ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                            int totalBoardStateCurrent = currentBoardState + value;
                            foreach (ChessGame.pieceName currPiece in allPieces)
                            {
                                PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        pieceMappingDetails.RevertPieceState();
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                    }
                                }
                            }
                            moveDetails.RevertBoardToOriginalState(board);
                        }
                    }
                }

                if (y - 2 >= 0 && y == 6 && board[y - 1][x] == null && board[y - 2][x] == null) // white pawn moves 2 squares north to empty square 
                {
                    int Y = y - 2;
                    int X = x;
                    AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                    moveDetails.SetPieceToBoard(board);
                    ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                    EnPassantDetails enPassantable = new EnPassantDetails(Y, X, piece);
                    foreach (ChessGame.pieceName currPiece in allPieces)
                    {
                        PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                        if (IsValidMove(board, turn, pieceStateMapping))
                        {
                            ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                            AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                            if (aiPruning.Prune)
                            {
                                pieceMappingDetails.RevertPieceState();
                                moveDetails.RevertBoardToOriginalState(board);
                                return aiPruning;
                            }
                            if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                            {
                                int totalBoardState = currentBoardState + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true, promptedTo, enPassantable);
                            }
                        }
                    }
                    moveDetails.RevertBoardToOriginalState(board);
                }

                if (y - 1 >= 0)
                {
                    // white pawn moving 1 square north to empty square
                    if (board[y - 1][x] == null)
                    {
                        int Y = y - 1;
                        int X = x;
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = currentBoardState + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    // white pawn eat black piece at north-west
                    if (x - 1 >= 0 && board[y - 1][x - 1] != null && PieceDetails.findSelectedPiece(board[y - 1][x - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                    {
                        int Y = y - 1;
                        int X = x - 1;
                        PictureBox target = board[Y][X];
                        int value = PieceValueColor(pieceStateMapping[target]);
                        if (value == kingvalue) return KingTaken(turn);
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        int totalBoardStateCurrent = currentBoardState + value;
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    // white pawn eat black piece at north-east
                    if (x + 1 < 8 && board[y - 1][x + 1] != null && PieceDetails.findSelectedPiece(board[y - 1][x + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.Black)
                    {
                        int Y = y - 1;
                        int X = x + 1;
                        PictureBox target = board[Y][X];
                        int value = PieceValueColor(pieceStateMapping[target]);
                        if (value == kingvalue) return KingTaken(turn);
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        int totalBoardStateCurrent = currentBoardState + value;
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                }
            }
            else // black pawn move
            {
                // black pawn enPassant
                if (enPassantDetails != null && enPassantDetails.Y == y && (enPassantDetails.X == x - 1 || enPassantDetails.X == x + 1))
                {
                    PieceStateDetails enPassantTarget = pieceStateMapping[enPassantDetails.Target];
                    if (enPassantTarget.PieceColor == ChessGame.pieceColor.White && enPassantTarget.PieceName == ChessGame.pieceName.Pawn)
                    {
                        int Y = enPassantDetails.Y + 1;
                        int X = enPassantDetails.X;                    
                        if (Y < 8 && board[Y][X] == null)
                        {
                            int value = PieceValueColor(pieceStateMapping[enPassantDetails.Target]);
                            AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X], AIMoveType.moveType.EnPassant, enPassantDetails, null);
                            moveDetails.SetPieceToBoard(board);
                            ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                            int totalBoardStateCurrent = currentBoardState + value;
                            foreach (ChessGame.pieceName currPiece in allPieces)
                            {
                                PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                    if (aiPruning.Prune)
                                    {
                                        pieceMappingDetails.RevertPieceState();
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                    }
                                }
                            }
                            moveDetails.RevertBoardToOriginalState(board);
                        }
                    }
                }
                if (y + 2 < 8 && y == 1 && board[y + 1][x] == null && board[y + 2][x] == null) // white pawn moves 2 squares south to empty square 
                {
                    int Y = y + 2;
                    int X = x;
                    AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                    moveDetails.SetPieceToBoard(board);
                    ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                    EnPassantDetails enPassantable = new EnPassantDetails(Y, X, piece);
                    foreach (ChessGame.pieceName currPiece in allPieces)
                    {
                        PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                        if (IsValidMove(board, turn, pieceStateMapping))
                        {
                            ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                            AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                            if (aiPruning.Prune)
                            {
                                pieceMappingDetails.RevertPieceState();
                                moveDetails.RevertBoardToOriginalState(board);
                                return aiPruning;
                            }
                            if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                            {
                                int totalBoardState = currentBoardState + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true, promptedTo, enPassantable);
                            }
                        }
                    }
                    moveDetails.RevertBoardToOriginalState(board);
                }

                if (y + 1 < 8)
                {
                    if (board[y + 1][x] == null) // black pawn moving 1 square south to empty square
                    {
                        int Y = y + 1;
                        int X = x;
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = currentBoardState + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    if (x - 1 >= 0 && board[y + 1][x - 1] != null && PieceDetails.findSelectedPiece(board[y + 1][x - 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White) // black pawn eat white piece at south-west
                    {
                        int Y = y + 1;
                        int X = x - 1;
                        PictureBox target = board[Y][X];
                        int value = PieceValueColor(pieceStateMapping[target]);
                        if (value == kingvalue) return KingTaken(turn);
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        int totalBoardStateCurrent = currentBoardState + value;
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    if (x + 1 < 8 && board[y + 1][x + 1] != null && PieceDetails.findSelectedPiece(board[y + 1][x + 1], pieceStateMapping).PieceColor == ChessGame.pieceColor.White) // black pawn eat white piece at south-east  
                    {
                        int Y = y + 1;
                        int X = x + 1;
                        PictureBox target = board[Y][X];
                        int value = PieceValueColor(pieceStateMapping[target]);
                        if (value == kingvalue) return KingTaken(turn);
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        ChessGame.pieceName[] allPieces = ReturnAllPromotedPieces(Y, turn, movesCount);
                        int totalBoardStateCurrent = currentBoardState + value;
                        foreach (ChessGame.pieceName currPiece in allPieces)
                        {
                            PieceStateDetails promotedPiece = pieceMappingDetails.PromotePiece(currPiece);
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                ChessGame.pieceName promptedTo = currPiece != ChessGame.pieceName.Pawn ? currPiece : ChessGame.pieceName.None;
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping, promptedTo);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + PromotedPieceValue(currPiece, promotedPiece) + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, false, promptedTo);
                                }
                            }
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                }
            }
            pieceMappingDetails.RevertPieceState();
            return new AIPruning();
        }

        private AIPruning RookAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            PictureBox piece = board[y][x];
            PieceStateMappingDetails pieceMappingDetails = setPieceMappingDetailsTurn(piece, pieceStateMapping);
            pieceMappingDetails.SetPieceState();
            bool[] pieceDirection = new bool[4]; // this array represents north, east, south, west and will reduce the processing time

            for (int i = 1; i < 8; i++) // represents the distance to be moved
            {
                // rook cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.RookDirection.Length; j++) // represents the array to define the direction of the move
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.RookDirection[j][0];
                    int X = x + i * PieceDetails.RookDirection[j][1];
                    if (Y >= 0 && Y < 8 && X >= 0 && X < 8)
                    {
                        PictureBox target = board[Y][X];
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        if (target == null) // rook moving to empty square
                        {
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = currentBoardState + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                                }
                            }
                        }
                        else // rook moving to square that is not empty
                        {
                            PieceStateDetails targetPiece = pieceStateMapping[target];
                            int value = PieceValueColor(targetPiece);
                            int totalBoardStateCurrent = currentBoardState + value;
                            if (turn && targetPiece.PieceColor == ChessGame.pieceColor.Black) // white rook eat black piece
                            {
                                if (value == kingvalue)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        pieceMappingDetails.RevertPieceState();
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else if (!turn && targetPiece.PieceColor == ChessGame.pieceColor.White) // black rook eat white piece
                            {
                                if (value == kingvalue)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        pieceMappingDetails.RevertPieceState();
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else
                                pieceDirection[j] = true; // if rook lands on the its same color piece
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    else
                        pieceDirection[j] = true; // if rook lands out of bounds
                }
            }
            pieceMappingDetails.RevertPieceState();
            return new AIPruning();
        }

        private AIPruning KnightAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            foreach (int[] dir in PieceDetails.KnightDirection)
            {
                int Y = y + dir[0];
                int X = x + dir[1];
                if (Y < 0 || Y > 7 || X < 0 || X > 7) continue;
                PictureBox target = board[Y][X];
                AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                moveDetails.SetPieceToBoard(board);
                if (target == null) // knight moving to empty square
                {
                    if (IsValidMove(board, turn, pieceStateMapping))
                    {
                        AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                        if (aiPruning.Prune)
                        {
                            moveDetails.RevertBoardToOriginalState(board);
                            return aiPruning;
                        }
                        if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                        {
                            int totalBoardState = currentBoardState + aiPruning.Value;
                            CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                        }
                    }
                }
                else
                {
                    PieceStateDetails targetPiece = pieceStateMapping[target];
                    int value = PieceValueColor(targetPiece);
                    int totalBoardStateCurrent = currentBoardState + value;                
                    if (turn && targetPiece.PieceColor == ChessGame.pieceColor.Black) // white knight eatting black piece
                    {
                        if (value == kingvalue)
                        {
                            moveDetails.RevertBoardToOriginalState(board);
                            return KingTaken(turn);
                        }
                        if (IsValidMove(board, turn, pieceStateMapping))
                        {
                            AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                            if (aiPruning.Prune)
                            {
                                moveDetails.RevertBoardToOriginalState(board);
                                return aiPruning;
                            }
                            if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                            {
                                int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                            }
                        }
                    }
                    else if (!turn && targetPiece.PieceColor == ChessGame.pieceColor.White) // black knight eatting white piece
                    {
                        if (value == kingvalue)
                        {
                            moveDetails.RevertBoardToOriginalState(board);
                            return KingTaken(turn);
                        }
                        if (IsValidMove(board, turn, pieceStateMapping))
                        {
                            AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                            if (aiPruning.Prune)
                            {
                                moveDetails.RevertBoardToOriginalState(board);
                                return aiPruning;
                            }
                            if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                            {
                                int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                            }
                        }
                    }
                }
                moveDetails.RevertBoardToOriginalState(board);
            }
            return new AIPruning();
        }

        private AIPruning BishopAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            bool[] pieceDirection = new bool[4]; // this array represents northeast, southeast, southwest, northwest and will reduce the processing time

            for (int i = 1; i < 8; i++) // represents the distance to be moved
            {
                // bishop cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.BishopDirection.Length; j++) // represents the array to define the direction of the move
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.BishopDirection[j][0];
                    int X = x + i * PieceDetails.BishopDirection[j][1];
                    if (Y >= 0 && Y < 8 && X >= 0 && X < 8)
                    {
                        PictureBox target = board[Y][X];
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        if (target == null) // bishop moving to empty square
                        {
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                if (aiPruning.Prune)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = currentBoardState + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                                }
                            }
                        }
                        else
                        {
                            PieceStateDetails targetPiece = pieceStateMapping[target];
                            int value = PieceValueColor(targetPiece);
                            int totalBoardStateCurrent = currentBoardState + value;
                            if (turn && targetPiece.PieceColor == ChessGame.pieceColor.Black) // white bishop eatting black piece
                            {
                                if (value == kingvalue)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else if (!turn && targetPiece.PieceColor == ChessGame.pieceColor.White) // black bishop eatting white piece
                            {
                                if (value == kingvalue)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else
                                pieceDirection[j] = true; // if bishop lands on the its same color piece
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    else
                        pieceDirection[j] = true; // if bishop lands out of bounds
                }
            }
            return new AIPruning();
        }

        private AIPruning QueenAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            bool[] pieceDirection = new bool[8]; // this array represents north, east, south, west, northeast, southeast, southwest, northwest and will reduce the processing time

            for (int i = 1; i < 8; i++) // represents the distance to be moved
            {
                // queen cannot move anymore as it is either out of bound or there is another piece blocking it from its target
                if (PieceDetails.checkedAllDirections(pieceDirection))
                    break;
                for (int j = 0; j < PieceDetails.QueenDirection.Length; j++) // represents the array to define the direction of the move
                {
                    if (pieceDirection[j]) continue;
                    int Y = y + i * PieceDetails.QueenDirection[j][0];
                    int X = x + i * PieceDetails.QueenDirection[j][1];
                    if (Y >= 0 && Y < 8 && X >= 0 && X < 8)
                    {
                        PictureBox target = board[Y][X];
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                        moveDetails.SetPieceToBoard(board);
                        if (target == null) // if queen moves to an empty square
                        {
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                if (aiPruning.Prune)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = currentBoardState + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                                }
                            }
                        }
                        else
                        {
                            PieceStateDetails targetPiece = pieceStateMapping[target];
                            int value = PieceValueColor(targetPiece);
                            int totalBoardStateCurrent = currentBoardState + value;
                            if (turn && targetPiece.PieceColor == ChessGame.pieceColor.Black) // if white queen eats a black piece
                            {
                                if (value == kingvalue)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else if (!turn && targetPiece.PieceColor == ChessGame.pieceColor.White) // if black queen eats a white piece
                            {
                                if (value == kingvalue)
                                {
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return KingTaken(turn);
                                }
                                pieceDirection[j] = true;
                                if (IsValidMove(board, turn, pieceStateMapping))
                                {
                                    AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                    if (aiPruning.Prune)
                                    {
                                        moveDetails.RevertBoardToOriginalState(board);
                                        return aiPruning;
                                    }
                                    if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                    {
                                        int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                        CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                    }
                                }
                            }
                            else
                                pieceDirection[j] = true; // if bishop lands on the its same color piece
                        }
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                    else
                        pieceDirection[j] = true; // if bishop lands out of bounds
                }
            }
            return new AIPruning();
        }

        private AIPruning KingAI(PictureBox[][] board, int y, int x, bool turn, int movesCount, int movesLimit, int currentBoardState, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            PictureBox piece = board[y][x];
            if (!pieceStateMapping[board[y][x]].HasMoved) // if king castles 
            {
                PieceStateMappingDetails pieceMappingDetailsCastle = setPieceMappingDetailsTurn(piece, pieceStateMapping);
                pieceMappingDetailsCastle.SetPieceState();
                PieceStateDetails selectedPiece = pieceStateMapping[piece];
                int castlePoints = 10;
                int firstMoveCastlingPoints = 5;
                int castleTurnPoints = (selectedPiece.PieceColor == ChessGame.pieceColor.White ? castlePoints : -castlePoints);
                // if this is the first move then increase the points to ensure that the ai will castle at the first opportunity
                castleTurnPoints += movesCount != 0 ? 0 : selectedPiece.PieceColor == ChessGame.pieceColor.White ? firstMoveCastlingPoints : -firstMoveCastlingPoints;
                if (x + 2 < 8) // castling to the right
                {
                    int X = x + 2;
                    CastlingDetails castleDetails = AbleToCastle(board, y, x, y, X, pieceStateMapping, turn);
                    if (castleDetails != null)
                    {
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, y, X, board[y][x], board[y][X], AIMoveType.moveType.Castle, null, castleDetails);
                        moveDetails.SetPieceToBoard(board);
                        PieceStateMappingDetails rookMappingDetails = setPieceMappingDetailsTurn(castleDetails.Source, pieceStateMapping);
                        rookMappingDetails.SetPieceState();
                        AIPruning aiPruning = DetermineAIGameState(y, x, y, X, board, turn, pieceStateMapping);
                        if (aiPruning.Prune)
                        {
                            pieceMappingDetailsCastle.RevertPieceState();
                            rookMappingDetails.RevertPieceState();
                            moveDetails.RevertBoardToOriginalState(board);
                            return aiPruning;
                        }
                        if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                        {
                            int totalBoardState = currentBoardState + aiPruning.Value + castleTurnPoints;
                            CalculateBestPath(board, y, x, y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                        }
                        rookMappingDetails.RevertPieceState();
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                }
                if (x - 2 > 0) // castling to the left
                {
                    int X = x - 2;
                    CastlingDetails castleDetails = AbleToCastle(board, y, x, y, X, pieceStateMapping, turn);
                    if (castleDetails != null)
                    {
                        AIMoveDetails moveDetails = new AIMoveDetails(y, x, y, X, board[y][x], board[y][X], AIMoveType.moveType.Castle, null, castleDetails);
                        moveDetails.SetPieceToBoard(board);
                        PieceStateMappingDetails rookMappingDetails = setPieceMappingDetailsTurn(castleDetails.Source, pieceStateMapping);
                        rookMappingDetails.SetPieceState();
                        AIPruning aiPruning = DetermineAIGameState(y, x, y, X, board, turn, pieceStateMapping);
                        if (aiPruning.Prune)
                        {
                            pieceMappingDetailsCastle.RevertPieceState();
                            rookMappingDetails.RevertPieceState();
                            moveDetails.RevertBoardToOriginalState(board);
                            return aiPruning;
                        }
                        if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                        {
                            int totalBoardState = currentBoardState + aiPruning.Value + castleTurnPoints;
                            CalculateBestPath(board, y, x, y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                        }
                        rookMappingDetails.RevertPieceState();
                        moveDetails.RevertBoardToOriginalState(board);
                    }
                }
                pieceMappingDetailsCastle.RevertPieceState();
            }
            PieceStateMappingDetails pieceMappingDetails = setPieceMappingDetailsTurn(piece, pieceStateMapping);
            pieceMappingDetails.SetPieceState();
            // loop through all directions king can move by one square 
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int Y = y + i;
                    int X = x + j;
                    if ((i == 0 && j == 0) || Y < 0 || Y > 7 || X < 0 || X > 7) continue;
                    PictureBox target = board[Y][X];
                    AIMoveDetails moveDetails = new AIMoveDetails(y, x, Y, X, board[y][x], board[Y][X]);
                    moveDetails.SetPieceToBoard(board);
                    if (target == null) // if king moves to empty square
                    {
                        if (IsValidMove(board, turn, pieceStateMapping))
                        {
                            AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                            if (aiPruning.Prune)
                            {
                                pieceMappingDetails.RevertPieceState();
                                moveDetails.RevertBoardToOriginalState(board);
                                return aiPruning;
                            }
                            if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                            {
                                int totalBoardState = currentBoardState + aiPruning.Value;
                                CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping, true);
                            }
                        }
                    }
                    else
                    {
                        PieceStateDetails targetPiece = pieceStateMapping[target];
                        int value = PieceValueColor(targetPiece);
                        int totalBoardStateCurrent = currentBoardState + value;
                        if (turn && targetPiece.PieceColor == ChessGame.pieceColor.Black) // white king eats black piece
                        {
                            if (value == kingvalue)
                            {
                                pieceMappingDetails.RevertPieceState();
                                moveDetails.RevertBoardToOriginalState(board);
                                return KingTaken(turn);
                            }
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                }
                            }
                        }
                        else if (!turn && targetPiece.PieceColor == ChessGame.pieceColor.White) // black king eats white piece
                        {
                            if (value == kingvalue)
                            {
                                pieceMappingDetails.RevertPieceState();
                                moveDetails.RevertBoardToOriginalState(board);
                                return KingTaken(turn);
                            }
                            if (IsValidMove(board, turn, pieceStateMapping))
                            {
                                AIPruning aiPruning = DetermineAIGameState(y, x, Y, X, board, turn, pieceStateMapping);
                                if (aiPruning.Prune)
                                {
                                    pieceMappingDetails.RevertPieceState();
                                    moveDetails.RevertBoardToOriginalState(board);
                                    return aiPruning;
                                }
                                if (aiPruning.GameState != ChessGame.gameState.Stalemate)
                                {
                                    int totalBoardState = totalBoardStateCurrent + aiPruning.Value;
                                    CalculateBestPath(board, y, x, Y, X, turn, totalBoardState, movesLimit, pieceStateMapping);
                                }
                            }
                        }
                    }
                    moveDetails.RevertBoardToOriginalState(board);
                }
            }
            pieceMappingDetails.RevertPieceState();
            return new AIPruning();
        }

        private void CalculateBestPath(PictureBox[][] board, int y, int x, int Y, int X, bool turn, int totalBoardState, int movesLimit, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, bool skipPruning = false, ChessGame.pieceName promptedTo = ChessGame.pieceName.None, EnPassantDetails enPassantable = null)
        {
            // skip the process of pruning path if required
            if (!skipPruning)
            {
                // prune path any path that is not necessary to calculate further
                if ((turn && bestPath > totalBoardState) || (!turn && bestPath < totalBoardState)) return;
                bestPath = totalBoardState;
            }
            AI newAI = new AI(movesCount + 1);
            int bestBoardState = newAI.MiniMax(board, !turn, movesLimit, totalBoardState, pieceStateMapping, enPassantable);
            SetResultBasedOnTurn(y, x, Y, X, turn, bestBoardState, board, pieceStateMapping, promptedTo);
        }

        private CastlingDetails AbleToCastle(PictureBox[][] board, int y, int x, int Y, int X, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, bool turn)
        {
            if (board[Y][X] != null) return null;
            PictureBox source = board[y][x];
            int diffX = X - x;
            if (diffX < 0)
            {
                for (int i = x - 1; i >= 0; i--)
                {
                    PictureBox currentPiece = board[Y][i];
                    if (currentPiece == null) continue;
                    PieceStateDetails pieceStateDetails = pieceStateMapping[currentPiece];
                    // this has to be a rook and the king has to be the same color as the rook and the rook has to be more than 2 squares away from the king and rook has not moved
                    if (pieceStateDetails.PieceName == ChessGame.pieceName.Rook && pieceStateDetails.PieceColor == pieceStateMapping[source].PieceColor && x - i > 2 && !pieceStateDetails.HasMoved)
                    {
                        if (CastlingCheck(board, y, new int[] { x, x - 1, x - 2 }, source, pieceStateMapping, turn))
                        {
                            return new CastlingDetails(y, i, y, x - 1, currentPiece, board[y][x - 1]);
                        }
                    }
                    return null;
                }
            }
            else
            {
                for (int i = x + 1; i < 8; i++)
                {
                    PictureBox currentPiece = board[Y][i];
                    if (currentPiece == null) continue;
                    PieceStateDetails pieceStateDetails = pieceStateMapping[currentPiece];
                    // this has to be a rook and the king has to be the same color as the rook and the rook has to be more than 2 squares away from the king and rook has not moved
                    if (pieceStateDetails.PieceName == ChessGame.pieceName.Rook && pieceStateDetails.PieceColor == pieceStateMapping[source].PieceColor && i - x > 2 && !pieceStateDetails.HasMoved)
                    {
                        if (CastlingCheck(board, y, new int[] { x, x + 1, x + 2 }, source, pieceStateMapping, turn))
                        {
                            return new CastlingDetails(y, i, y, x + 1, currentPiece, board[y][x + 1]);
                        }
                    }
                    return null;
                }
            }
            return null;
        }

        private bool CastlingCheck(PictureBox[][] board, int Y, int[] allPositionsX, PictureBox source, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, bool turn)
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

        private void SetResultBasedOnTurn(int y, int x, int Y, int X, bool turn, int bestBoardState, PictureBox[][] newBoard, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo = ChessGame.pieceName.None)
        {
            if (movesCount == 0)
            {
                CapturedSquares capture = new CapturedSquares(newBoard, pieceStateMapping);
                setResult(turn, bestBoardState, y, x, Y, X, capture.CaptureCount(), promptedTo);
            }
            else
                setResult(turn, bestBoardState, y, x, Y, X, 0, promptedTo);
        }

        // this will be subtracted or added to the current board state depending on who's turn it is (if it is black's turn it will subtract and if it is white's turn it will add)
        public int PieceValue(ChessGame.pieceName piece)
        {
            switch (piece)
            {
                case ChessGame.pieceName.Pawn:
                    return 10;
                case ChessGame.pieceName.Rook:
                    return 50;
                case ChessGame.pieceName.Knight:
                    return 30;
                case ChessGame.pieceName.Bishop:
                    return 35;
                case ChessGame.pieceName.Queen:
                    return 90;
                default:
                    return kingvalue;
            }
        }

        private int PieceValueColor(PieceStateDetails piece)
        {
            int value = PieceValue(piece.PieceName);
            return piece.PieceColor == ChessGame.pieceColor.White ? -value : value;
        }

        private int PromotedPieceValue(ChessGame.pieceName pieceName, PieceStateDetails piece)
        {
            int value = PieceValue(pieceName) - PieceValue(ChessGame.pieceName.Pawn);
            return piece.PieceColor == ChessGame.pieceColor.White ? value : -value;
        }

        // return a list of pieces that might be promoted pieces or just a pawn (which represents an unpromoted piece)
        private ChessGame.pieceName[] ReturnAllPromotedPieces(int Y, bool turn, int movesCount)
        {
            if ((turn && Y == 0) || (!turn && Y == 7))
            {
                // for the first move made by the AI it will check for stalemate and checkmate where choosing a rook or bishop over a queen might be desirable to not cause a stalemate
                if (movesCount == 0)
                {
                    return new ChessGame.pieceName[] { ChessGame.pieceName.Queen, ChessGame.pieceName.Rook, ChessGame.pieceName.Bishop, ChessGame.pieceName.Knight };
                }
                return new ChessGame.pieceName[] { ChessGame.pieceName.Queen, ChessGame.pieceName.Knight };
            }
            return new ChessGame.pieceName[] { ChessGame.pieceName.Pawn };
        }

        // set result for the current best boardstate
        private void setResult(bool turn, int bestBoardState, int sourceY, int sourceX, int destinationY, int destinationX, int squares, ChessGame.pieceName promptedTo)
        {
            if ((turn && bestBoardState >= BoardState) || (!turn && bestBoardState <= BoardState))
            {
                // movesCount comparison will not be made if movesCount is not 0 as there is no point checking equal boardstates if it is not an actual move being made by the AI
                if (movesCount != 0 && bestBoardState == BoardState)
                    return;
                // This will only apply if movesCount is 0 where it should determine if this is the best move based off the number of squares captured
                if (bestBoardState == BoardState && ((turn && squares <= squareCount) || (!turn && squares >= squareCount)))
                    return;
                BoardState = bestBoardState;
                squareCount = squares;
                if (movesCount == 0) // only stores source and destination of move if movesCount is 0 as AI actually make that move in that specific move
                {
                    SourceY = sourceY;
                    SourceX = sourceX;
                    DestinationY = destinationY;
                    DestinationX = destinationX;
                    PromptedTo = promptedTo;
                }
            }
        }

        private AIPruning KingTaken(bool turn)
        {
            return new AIPruning(SetToMinOrMax(turn), true);
        }

        private int SetToMinOrMax(bool turn)
        {
            return turn ? int.MaxValue : int.MinValue;
        }

        // determine if move been made by the AI is illegal or not (this will ensure that moves made are not illegal which will help the AI optimize it's moves)
        private bool IsValidMove(PictureBox[][] board, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            // this illegal move check will be capped at movesCount 2 as it seems to triple the processing time after movesCount 3
            if (movesCount > 2) return true;
            int[] kingCoord = PieceDetails.FindKing(board, turn, pieceStateMapping);
            if (BoardCheck.Check.IsChecked(board, kingCoord[0], kingCoord[1], !turn, pieceStateMapping)) // determine if this is an illegal move by check
                return false;
            return true;
        }

        // used to test if two boards are the same (used for testing purposes)
        public bool CompareChessBoardTest(PictureBox[][] boardOne, PictureBox[][] boardTwo)
        {
            for (int y = 0; y < boardOne.Length; y++) 
            {
                for (int x = 0; x < boardOne[y].Length; x++) 
                {
                    if (boardOne[y][x] != boardTwo[y][x])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private AIPruning DetermineAIGameState(int sourceY, int sourceX, int destinationY, int destinationX, PictureBox[][] board, bool turn, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, ChessGame.pieceName promptedTo = ChessGame.pieceName.None)
        {
            if (movesCount < 3)
            {
                ChessGame.gameState currentStatus = BoardCheck.CurrentStatus.TurnResult(board, turn, pieceStateMapping);
                // if the first move made by the AI is checkmate then treat this as the winning move for the AI (WIN)
                if (currentStatus == ChessGame.gameState.Checkmate)
                {
                    if (movesCount == 0)
                    {
                        FinalResult.SourceY = sourceY;
                        FinalResult.SourceX = sourceX;
                        FinalResult.DestinationY = destinationY;
                        FinalResult.DestinationX = destinationX;
                        FinalResult.PromptedTo = promptedTo;
                    }
                    return new AIPruning(turn ? int.MaxValue : int.MinValue, true, currentStatus);
                }
                // if the first move made by the AI is stalemate then treat this as the losing move for the AI (LOSE)
                if (currentStatus == ChessGame.gameState.Stalemate)
                    return new AIPruning(turn ? -1000 : 1000, false, currentStatus);
                // if the first move made by the AI is check then treat this as the winning move for the AI (WIN)
                if (currentStatus == ChessGame.gameState.Check)
                    return new AIPruning(turn ? 1 : -1, false, currentStatus);
            }
            return new AIPruning(0, false);
        }

        private PieceStateMappingDetails setPieceMappingDetailsTurn(PictureBox piece, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) 
        {
            PieceStateDetails nextPieceStateDetails = pieceStateMapping[piece].Clone();
            nextPieceStateDetails.HasMoved = true;
            return new PieceStateMappingDetails(piece, pieceStateMapping, pieceStateMapping[piece].Clone(), nextPieceStateDetails);
        }
    }
}

