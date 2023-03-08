using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess
{
    // This will ensure AI gets smarter when the late game begins
    static class IncreaseAIComplexity
    {
        static Dictionary<ChessGame.pieceName, int> pointsMapping = new Dictionary<ChessGame.pieceName, int>()
        {
            { ChessGame.pieceName.King, 1 },
            { ChessGame.pieceName.Knight, 1 },
            { ChessGame.pieceName.Bishop, 2 },
            { ChessGame.pieceName.Rook, 2 },
            { ChessGame.pieceName.Queen, 4 }
        };

        // This will increase the AI level depending on how what pieces are left on the board (the total should be 30 if all pieces are on the board)
        public static int CalculatePoints(PictureBox[][] board, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping)
        {
            int points = 0;
            for (int i = 0; i < board.Length; i++)
            {
                for (int j = 0; j < board[i].Length; j++)
                {
                    PictureBox piece = board[i][j];
                    if (piece == null) continue;
                    PieceStateDetails pieceState = pieceStateMapping[piece];
                    if (pointsMapping.ContainsKey(pieceState.PieceName))
                    {
                        points += pointsMapping[pieceState.PieceName];
                    }
                }
            }
            return AIComplexityLevel(points);
        }

        private static int AIComplexityLevel(int points) 
        {
            if (points > 10)
            {
                return 0;
            }
            return 1;
        }
    }
}
