using System.Collections.Generic;

namespace Chess.AIMove
{
    static class SquarePoints
    {
        // this determine the points to increase the captured square by depending on the target
        static public int move = 1;

        static public Dictionary<ChessGame.pieceName, int> attack = new Dictionary<ChessGame.pieceName, int>()
        {
            { ChessGame.pieceName.King, 4 },
            { ChessGame.pieceName.Queen, 4 },
            { ChessGame.pieceName.Rook, 3 },
            { ChessGame.pieceName.Bishop, 3 },
            { ChessGame.pieceName.Knight, 3 },
            { ChessGame.pieceName.Pawn, 2 }      
        };

        static public Dictionary<ChessGame.pieceName, int> defense = new Dictionary<ChessGame.pieceName, int>()
        {
            { ChessGame.pieceName.King, 0 }, // you cannot trade a king therefore it should be worth nothing defending that square
            { ChessGame.pieceName.Queen, 4 },
            { ChessGame.pieceName.Rook, 3 },
            { ChessGame.pieceName.Bishop, 3 },
            { ChessGame.pieceName.Knight, 3 },
            { ChessGame.pieceName.Pawn, 2 }
        };
    }
}
