namespace Chess.AIMove
{
    class AIPruning
    {
        private int _value;
        private bool _prune;
        private ChessGame.gameState _gameState;

        public int Value { get { return _value; } }
        public bool Prune { get { return _prune; } }
        public ChessGame.gameState GameState { get { return _gameState; } }

        public AIPruning(int value = 0, bool prune = false, ChessGame.gameState gameState = ChessGame.gameState.Normal)
        {
            _value = value;
            _prune = prune;
            _gameState = gameState;
        }
    }
}
