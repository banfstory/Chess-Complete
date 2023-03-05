using System.Windows.Forms;

namespace Chess.AIMove
{
    class AIMoveDetails
    {
        private AIMoveType.moveType MoveType;
        private int SourceY;
        private int SourceX;
        private int DestinationY;
        private int DestinationX;
        private PictureBox Source;
        private PictureBox Destination;
        private EnPassantDetails EnPassantDetails;
        private CastlingDetails CastlingDetails;
        private PieceStateMappingDetails pieceStateMappingDetails;

        public AIMoveDetails(int sourceY, int sourceX, int destinationY, int destinationX, PictureBox source, PictureBox destination, AIMoveType.moveType moveType = AIMoveType.moveType.Normal, EnPassantDetails enPassantDetails = null, CastlingDetails castleDetails = null, PieceStateMappingDetails pieceStateMappingDetails = null)
        {
            MoveType = moveType;
            SourceY = sourceY;
            SourceX = sourceX;
            DestinationY = destinationY;
            DestinationX = destinationX;
            Source = source;
            Destination = destination;
            EnPassantDetails = enPassantDetails;
            CastlingDetails = castleDetails;
            this.pieceStateMappingDetails = pieceStateMappingDetails;
        }

        public void SetPieceToBoard(PictureBox[][] board) 
        {
            board[SourceY][SourceX] = null;
            board[DestinationY][DestinationX] = Source;
            if (MoveType == AIMoveType.moveType.EnPassant && EnPassantDetails != null)
            {
                board[EnPassantDetails.Y][EnPassantDetails.X] = null;
            }
            else if (MoveType == AIMoveType.moveType.Castle && CastlingDetails != null)
            {
                board[CastlingDetails.SourceY][CastlingDetails.SourceX] = CastlingDetails.Destination;
                board[CastlingDetails.DestinationY][CastlingDetails.DestinationX] = CastlingDetails.Source;
            }
        }

        public void RevertBoardToOriginalState(PictureBox[][] board)
        {
            board[SourceY][SourceX] = Source;
            board[DestinationY][DestinationX] = Destination;
            if (MoveType == AIMoveType.moveType.EnPassant && EnPassantDetails != null)
            {
                board[EnPassantDetails.Y][EnPassantDetails.X] = EnPassantDetails.Target;
            }
            else if (MoveType == AIMoveType.moveType.Castle && CastlingDetails != null)
            {
                board[CastlingDetails.SourceY][CastlingDetails.SourceX] = CastlingDetails.Source;
                board[CastlingDetails.DestinationY][CastlingDetails.DestinationX] = CastlingDetails.Destination;
            }
        }
    }
}
