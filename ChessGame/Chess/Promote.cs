using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;

namespace Chess
{
    public class Promote
    {
        private PieceStateDetails selectedPiece;
        private bool promoted = false;
        private PromotedPieceDetails promotedFrom;
        private PromotedPieceDetails promotedTo;
        private PictureBox piece;
        private Dictionary<PictureBox, PieceStateDetails> pieceStateMapping;
        private int destinationY;
        private bool turn;
        private ComponentResourceManager resources = new ComponentResourceManager(typeof(ChessGame));
        private ChessGame.pieceName _piecePromoted = ChessGame.pieceName.Queen;
        ChessGame.Opponent opponent;
        ChessGame.AIColor aiColor;
        ChessGame.pieceName promptedToAI;

        public ChessGame.pieceName PiecePromoted { set { _piecePromoted = value; } }

        public Promote(PictureBox Piece, Dictionary<PictureBox, PieceStateDetails> PieceStateMapping, int DestinationY, bool Turn, ChessGame.Opponent Opponent, ChessGame.AIColor AIColor, ChessGame.pieceName PromptedToAI) 
        {
            piece = Piece;
            pieceStateMapping = PieceStateMapping;
            destinationY = DestinationY;
            turn = Turn;
            opponent = Opponent;
            aiColor = AIColor;
            promptedToAI = PromptedToAI;
            PromotePawn();
        }

        private void PromotePawn()
        {
            selectedPiece = pieceStateMapping[piece];
            if (selectedPiece.PieceName != ChessGame.pieceName.Pawn) return;
            if(AITurn() && promptedToAI != ChessGame.pieceName.None) 
            {
                _piecePromoted = promptedToAI;
                PromoteToPiece();

            }
            else if ((turn && selectedPiece.PieceColor == ChessGame.pieceColor.White && destinationY == 0) || (!turn && selectedPiece.PieceColor == ChessGame.pieceColor.Black && destinationY == 7))
            {
                setPromotePieceDetails();
            }
        }

        private void ShowPromotionPopup()
        {
            PromotePieceChoice promotePieceChoice = new PromotePieceChoice(this);
            promotePieceChoice.ShowDialog();
        }

        private void setPromotePieceDetails() 
        {
            ShowPromotionPopup();
            PromoteToPiece();
        }

        private void PromotionSucess() 
        {
            selectedPiece.PieceName = promotedTo.PieceName;
            piece.Image = promotedTo.Image;
            promoted = true;
        }

        private void PromoteToPiece() 
        {
            promotedFrom = new PromotedPieceDetails(selectedPiece.PieceName, piece.Image);
            switch (_piecePromoted) 
            {
                case ChessGame.pieceName.Queen:
                    promotedTo = new PromotedPieceDetails(ChessGame.pieceName.Queen, TranslatePieceImage("q"));
                    break;
                case ChessGame.pieceName.Rook:
                    promotedTo = new PromotedPieceDetails(ChessGame.pieceName.Rook, TranslatePieceImage("r1"));
                    break;
                case ChessGame.pieceName.Bishop:
                    promotedTo = new PromotedPieceDetails(ChessGame.pieceName.Bishop, TranslatePieceImage("b1"));
                    break;
                case ChessGame.pieceName.Knight:
                    promotedTo = new PromotedPieceDetails(ChessGame.pieceName.Knight, TranslatePieceImage("kn1"));
                    break;
            }
            if (promotedTo == null) return;
            PromotionSucess();
        }

        private Image TranslatePieceImage(string imageNameSuffix) 
        {
            string pieceImageName = imageNameSuffix + ".Image";
            if (selectedPiece.PieceColor == ChessGame.pieceColor.White)
            {
                pieceImageName = "w" + pieceImageName;
            }
            else 
            {
                pieceImageName = "b" + pieceImageName;
            }
            return (Image)resources.GetObject(pieceImageName);
        }

        private bool AITurn() 
        {
            return (opponent == ChessGame.Opponent.AI && (turn && aiColor == ChessGame.AIColor.White) || (!turn && aiColor == ChessGame.AIColor.Black));
        }

        public void UndoPromotePiece() 
        {
            if (promoted) 
            {
                selectedPiece.PieceName = promotedFrom.PieceName;
                piece.Image = promotedFrom.Image;
            }
        }

        public void RedoPromotePiece() 
        {
            if (promoted)
            {
                selectedPiece.PieceName = promotedTo.PieceName;
                piece.Image = promotedTo.Image;
            }
        }

        class PromotedPieceDetails
        {
            private ChessGame.pieceName _pieceName;
            private Image _image;

            public PromotedPieceDetails(ChessGame.pieceName piece, Image image)
            {
                _pieceName = piece;
                _image = image;
            }

            public ChessGame.pieceName PieceName { get { return _pieceName; } }
            public Image Image { get { return _image; } }
        }
    }
}
