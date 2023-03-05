using System.Collections.Generic;
using System.Windows.Forms;

namespace Chess
{
    class PieceStateMappingDetails
    {
        PictureBox Piece;
        Dictionary<PictureBox, PieceStateDetails> PieceStateMapping;
        PieceStateDetails PieceStateBefore;
        PieceStateDetails PieceStateAfter;

        public PieceStateMappingDetails(PictureBox piece, Dictionary<PictureBox, PieceStateDetails> pieceStateMapping, PieceStateDetails pieceStateBefore, PieceStateDetails pieceStateAfter)
        {
            Piece = piece;
            PieceStateMapping = pieceStateMapping;
            PieceStateBefore = pieceStateBefore;
            PieceStateAfter = pieceStateAfter;
        }

        public void SetPieceState()
        {
            PieceStateMapping[Piece] = PieceStateAfter.Clone();
        }

        public void RevertPieceState()
        {
            PieceStateMapping[Piece] = PieceStateBefore.Clone();
        }

        public PieceStateDetails PromotePiece(ChessGame.pieceName pieceName) 
        {
            PieceStateAfter = PieceStateAfter.Clone();
            PieceStateAfter.PieceName = pieceName;
            SetPieceState();
            return PieceStateAfter;
        }
    }
}
