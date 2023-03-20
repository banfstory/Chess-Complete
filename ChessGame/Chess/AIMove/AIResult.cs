namespace Chess.AIMove
{
    // this will be used to determine the best final move that is made by the AI
    class AIResult
    {
        public int SourceY;
        public int SourceX;
        public int DestinationY;
        public int DestinationX;
        public ChessGame.pieceName PromptedTo = ChessGame.pieceName.None;
    }
}
