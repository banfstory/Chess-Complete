namespace Chess
{
    // determine the current state of the piece such as whether the piece has moved or has the piece type changed to something else due to promotion
    public class PieceStateDetails
    {
        private ChessGame.pieceColor _pieceColor;
        private ChessGame.pieceName _pieceName;
        private bool _hasMoved = false;

        public PieceStateDetails(ChessGame.pieceColor pieceColor, ChessGame.pieceName pieceName, bool hasMoved = false)
        {
            _pieceColor = pieceColor;
            _pieceName = pieceName;
            _hasMoved = hasMoved;
        }

        public ChessGame.pieceColor PieceColor
        {
            get { return _pieceColor; }
            set { _pieceColor = value; }
        }

        public ChessGame.pieceName PieceName
        {
            get { return _pieceName; }
            set { _pieceName = value; }
        }

        public bool HasMoved
        {
            get { return _hasMoved; }
            set { _hasMoved = value; }
        }

        public PieceStateDetails Clone()
        {
            return new PieceStateDetails(_pieceColor, _pieceName, _hasMoved);
        }
    }
}
