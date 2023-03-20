using System.Windows.Forms;

namespace Chess.AIMove
{
    // This will be used to reduce the processing time which ensures that it does not need to create a new board array for each node when processing recursively but instead it reverts the board array to it's original state
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

        // this will set the piece(s) to the board array where the node path starts
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

        // this will revert the board to it's original state once the node path has completely processing
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
