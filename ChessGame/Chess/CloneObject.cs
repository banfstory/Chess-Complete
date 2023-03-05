using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess
{
    static class CloneObject
    {
        static public PictureBox[][] CloneBoard(PictureBox[][] board)
        {
            PictureBox[][] newBoard = new PictureBox[8][];
            for (int i = 0; i < newBoard.Length; i++)
            {
                newBoard[i] = new PictureBox[8];
                board[i].CopyTo(newBoard[i], 0);
            }
            return newBoard;
        }

        static public Dictionary<PictureBox, PieceStateDetails> ClonePieceStateMapping(Dictionary<PictureBox, PieceStateDetails> pieceStateMapping) 
        {
            Dictionary<PictureBox, PieceStateDetails> newPieceStateMapping = new Dictionary<PictureBox, PieceStateDetails>();
            foreach (KeyValuePair<PictureBox, PieceStateDetails> piece in pieceStateMapping)
            {
                newPieceStateMapping[piece.Key] = piece.Value.Clone();
            }
            return newPieceStateMapping;
        }
    }
}
