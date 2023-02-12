using System.Collections.Generic;

namespace Chess.AIMove
{
    static class SquarePoints
    {
        // this determine the points to increase the captured square by depending on the target
        static public int move = 3;

        static public Dictionary<ChessGame.pieceName, int> attack = new Dictionary<ChessGame.pieceName, int>()
        {
            { ChessGame.pieceName.King, 5 },
            { ChessGame.pieceName.Queen, 5 },
            { ChessGame.pieceName.Rook, 5 },
            { ChessGame.pieceName.Bishop, 5 },
            { ChessGame.pieceName.Knight, 4 },
            { ChessGame.pieceName.Pawn, 4 }      
        };

        static public Dictionary<ChessGame.pieceName, int> defense = new Dictionary<ChessGame.pieceName, int>()
        {
            { ChessGame.pieceName.King, 0 }, // you cannot trade a king therefore it should be worth nothing defending that square
            { ChessGame.pieceName.Queen, 5 },
            { ChessGame.pieceName.Rook, 5 },
            { ChessGame.pieceName.Bishop, 5 },
            { ChessGame.pieceName.Knight, 5 },
            { ChessGame.pieceName.Pawn, 4 }
        };
    }
}
