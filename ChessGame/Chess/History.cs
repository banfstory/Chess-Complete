using System.Windows.Forms;

namespace Chess
{
    public class History // a doubly linked list that allows player to undo or redo move based these variables
    {
        public bool Turn; 
        public int SourceY;
        public int SourceX;
        public int DestinationY;
        public int DestinationX;
        public bool FirstMoveMade;
        public bool EnPassantable;
        public EnPassantDetails EnPassantDetails = null;
        public CastlingDetails CastlingDetails = null;
        public PictureBox Source;
        public PictureBox Destination;
        public ChessGame.gameState State;
        public History Prev;
        public History Next;
        public Promote Promote;

        // this will be the root where no variables will passed to it
        public History() { }

        public History(bool turn, int sourceY, int sourceX, int destinationY, int destinationX, PictureBox source, PictureBox destination, ChessGame.gameState state, Promote promote, bool enPassantable, EnPassantDetails enPassantDetails, CastlingDetails castlingDetails, bool firstMoveMade = false)
        { 
            Turn = turn;
            SourceY = sourceY;
            SourceX = sourceX;
            DestinationY = destinationY;
            DestinationX = destinationX;
            Source = source;
            Destination = destination;
            State = state;
            Promote = promote;
            EnPassantable = enPassantable;
            EnPassantDetails = enPassantDetails;
            CastlingDetails = castlingDetails;
            FirstMoveMade = firstMoveMade;
        }
    }
}
