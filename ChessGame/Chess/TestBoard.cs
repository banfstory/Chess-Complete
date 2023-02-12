using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static Chess.ChessGame;

namespace Chess
{
    class TestBoard
    {
        private ChessGame main;
        private PictureBox[][] testBoard;
        private Dictionary<string, PictureBox> pieceMap = new Dictionary<string, PictureBox>();

        public TestBoard(PictureBox[][] board, ChessGame Main)
        {
            InitializePieceMap(Main.allChessPieces);
            main = Main;
            testBoard = board;
        }

        private void InitializePieceMap(Dictionary<pieceColor, Dictionary<pieceName, List<PictureBox>>> allChessPieces)
        {
            foreach (KeyValuePair<pieceColor, Dictionary<pieceName, List<PictureBox>>> pieceColor in allChessPieces)
            {
                foreach (KeyValuePair<pieceName, List<PictureBox>> pieceName in pieceColor.Value)
                {
                    foreach (PictureBox pieceObj in pieceName.Value)
                    {
                        pieceMap.Add(pieceObj.Name, pieceObj);
                    }
                }
            }
        }

        private void ClearBoard()
        {
            for (int i = 0; i < testBoard.Length; i++)
            {
                for (int j = 0; j < testBoard[i].Length; j++)
                {
                    if (testBoard[i][j] == null)
                        continue;
                    testBoard[i][j].Visible = false;
                }
            }
            for (int i = 0; i < testBoard.Length; i++)
            {
                for (int j = 0; j < testBoard[i].Length; j++)
                {
                    testBoard[i][j] = null;
                }
            }
        }

        private void SetUpBoard()
        {
            for (int y = 0; y < testBoard.Length; y++)
            {
                for (int x = 0; x < testBoard[y].Length; x++)
                {
                    if (testBoard[y][x] != null)
                    {
                        testBoard[y][x].Visible = true;
                        PieceDetails.movePiece(y, x, testBoard[y][x]);
                    }
                }
            }
        }

        public void selectBoardChoice(string choice, ChessGame.Opponent opponent = ChessGame.Opponent.Player, bool turn = true, ChessGame.AIColor aiColor = ChessGame.AIColor.None, int AIComplexity = 2)
        {
            main.opponent = opponent;
            main.aiColor = aiColor;
            main.AIComplexity = AIComplexity;
            main.changeTurn(turn, this.GetType().Name);
            chosenBoard(choice);
            if (main.opponent == Opponent.AI && ((turn && main.aiColor == AIColor.White) || (!turn && main.aiColor == AIColor.Black)))
                main.AIMove();
        }

        private void chosenBoard(string choice)
        {
            ClearBoard();
            switch (choice)
            {
                case "TestStaleMate":
                    TestStaleMate();
                    break;
                case "TestAICheckmate":
                    TestAICheckmate();
                    break;
                case "TestAICheckmate2":
                    TestAICheckmate2();
                    break;
                case "TestAIPromote":
                    TestAIPromote();
                    break;
                case "TestPossibleMoves":
                    TestPossibleMoves();
                    break;
                case "TestEnPassant":
                    TestEnPassant();
                    break;
                case "TestCastling":
                    TestCastling();
                    break;
                case "TestAIOptimization":
                    TestAIOptimization();
                    break;
                case "TestAIEnPassant":
                    TestAIEnPassant();
                    break;
            }
            SetUpBoard();
        }

        // List of different board position setup
        private void TestStaleMate() // Move white queen to E3 to perform stalemate (Used for testing purposes)
        {
            testBoard[0][2] = pieceMap["wq"]; testBoard[0][5] = pieceMap["bb1"]; testBoard[0][6] = pieceMap["bkn1"]; testBoard[0][7] = pieceMap["br1"]; testBoard[1][4] = pieceMap["bp1"];
            testBoard[1][6] = pieceMap["bp2"]; testBoard[1][7] = pieceMap["bq"]; testBoard[2][5] = pieceMap["bp3"]; testBoard[2][6] = pieceMap["bk"]; testBoard[2][7] = pieceMap["br2"];
            testBoard[3][7] = pieceMap["bp4"]; testBoard[7][0] = pieceMap["wk"]; testBoard[6][0] = pieceMap["wp1"]; testBoard[6][1] = pieceMap["wp2"]; testBoard[7][2] = pieceMap["wr1"];
            testBoard[4][7] = pieceMap["wp3"];
        }

        private void TestAICheckmate() // Move white queen to E3 to perform stalemate (Used for testing purposes)
        {
            testBoard[0][0] = pieceMap["br1"]; testBoard[0][2] = pieceMap["bb1"]; testBoard[0][4] = pieceMap["bq"]; testBoard[0][6] = pieceMap["br2"]; testBoard[1][0] = pieceMap["bp1"];
            testBoard[1][1] = pieceMap["bp2"]; testBoard[1][2] = pieceMap["bp3"]; testBoard[1][3] = pieceMap["bp4"]; testBoard[1][7] = pieceMap["bp5"]; testBoard[2][4] = pieceMap["bp6"];
            testBoard[2][6] = pieceMap["bk"]; testBoard[7][5] = pieceMap["wq"]; testBoard[3][4] = pieceMap["wp1"]; testBoard[3][6] = pieceMap["wb1"]; testBoard[5][2] = pieceMap["wp2"];
            testBoard[5][7] = pieceMap["wp3"]; testBoard[6][0] = pieceMap["wp4"]; testBoard[6][6] = pieceMap["wp5"]; testBoard[7][0] = pieceMap["wr1"]; testBoard[7][4] = pieceMap["wk"];
            testBoard[7][7] = pieceMap["wr2"];
        }

        private void TestAICheckmate2() // Move white queen to E3 to perform stalemate (Used for testing purposes)
        {
            testBoard[1][6] = pieceMap["bk"]; testBoard[2][4] = pieceMap["wq"]; testBoard[1][5] = pieceMap["wp1"]; testBoard[4][7] = pieceMap["wb1"]; testBoard[6][6] = pieceMap["wk"];
        }

        private void TestAIPromote()
        {
            testBoard[1][3] = pieceMap["bk"]; testBoard[7][3] = pieceMap["wk"]; testBoard[1][0] = pieceMap["wp1"]; testBoard[6][0] = pieceMap["bp1"];
        }

        private void TestPossibleMoves()
        {
            testBoard[0][1] = pieceMap["bk"];
            testBoard[7][3] = pieceMap["wk"];
            int test_case = 10;
            switch (test_case)
            {
                case 0:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[6][3] = pieceMap["wr1"];
                    break;
                case 1:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[6][3] = pieceMap["wb1"];
                    break;
                case 2:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[6][3] = pieceMap["wkn1"];
                    break;
                case 3:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[6][3] = pieceMap["wq"];
                    break;
                case 4:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[5][2] = pieceMap["bp1"]; testBoard[6][3] = pieceMap["wp1"];
                    break;
                case 5:
                    testBoard[1][3] = pieceMap["br1"]; testBoard[6][0] = pieceMap["br2"]; testBoard[7][3] = null; testBoard[7][2] = pieceMap["wk"];
                    break;
                case 6:
                    testBoard[3][7] = pieceMap["bq"]; testBoard[6][4] = pieceMap["wr1"];
                    break;
                case 7:
                    testBoard[3][7] = pieceMap["bq"]; testBoard[6][4] = pieceMap["wb1"];
                    break;
                case 8:
                    testBoard[3][7] = pieceMap["bq"]; testBoard[6][4] = pieceMap["wq"];
                    break;
                case 9:
                    testBoard[3][7] = pieceMap["bq"]; testBoard[6][4] = pieceMap["wkn1"];
                    break;
                case 10:
                    testBoard[3][7] = pieceMap["bq"]; testBoard[6][4] = pieceMap["wp1"]; testBoard[5][3] = pieceMap["bp1"];
                    break;
            }
        }

        private void TestAIEnPassant() 
        {
            testBoard[7][4] = pieceMap["wk"]; testBoard[0][4] = pieceMap["bk"];
            int test_case = 0;
            switch (test_case)
            {
                case 0: // Test if black AI can enPassant white pawn
                    testBoard[4][4] = pieceMap["bp1"]; testBoard[6][3] = pieceMap["wp1"]; testBoard[6][5] = pieceMap["wp2"];
                    break;
                case 1: // Test if white AI can enPassant black pawn
                    testBoard[3][4] = pieceMap["wp1"]; testBoard[1][3] = pieceMap["bp1"]; testBoard[1][5] = pieceMap["bp2"];
                    break;
            }
        }

        private void TestEnPassant()
        {
            testBoard[0][1] = pieceMap["bk"]; testBoard[7][3] = pieceMap["wk"]; testBoard[4][1] = pieceMap["bp1"]; testBoard[6][2] = pieceMap["wp1"];
        }

        private void TestCastling()
        {
            testBoard[0][1] = pieceMap["bk"]; testBoard[7][4] = pieceMap["wk"];
            int test_case = 5;
            switch (test_case)
            {
                case 0:
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][7] = pieceMap["wr2"];
                    break;
                case 1:
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[0][4] = pieceMap["br1"];
                    break;
                case 2:
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[0][3] = pieceMap["br2"];
                    break;
                case 3:
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[0][5] = pieceMap["br1"];
                    break;
                case 4:
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[6][0] = pieceMap["wp1"]; testBoard[6][7] = pieceMap["wp2"];
                    break;
                case 5:
                    testBoard[0][0] = pieceMap["br1"]; testBoard[0][7] = pieceMap["br2"]; testBoard[0][1] = null; testBoard[0][4] = pieceMap["bk"];
                    break;
            }
        }

        private void TestAIOptimization()
        {
            int test_case = 5;
            switch (test_case)
            {
                case 0: // AI Checkmate
                    testBoard[4][7] = pieceMap["bq"]; testBoard[0][4] = pieceMap["bk"]; testBoard[3][2] = pieceMap["bb1"]; testBoard[6][5] = pieceMap["wp1"];
                    testBoard[7][4] = pieceMap["wk"]; testBoard[4][4] = pieceMap["wp3"]; testBoard[7][3] = pieceMap["wq"];
                    break;
                case 1: // AI Stalemate
                    testBoard[5][3] = pieceMap["bk"]; testBoard[7][1] = pieceMap["wk"]; testBoard[6][2] = pieceMap["bq"]; testBoard[0][7] = pieceMap["bkn1"];
                    break;
                case 2: // AI Checkmate
                    testBoard[6][0] = pieceMap["wp1"]; testBoard[6][1] = pieceMap["wp2"]; testBoard[6][2] = pieceMap["wp3"]; testBoard[6][3] = pieceMap["wp4"];
                    testBoard[4][4] = pieceMap["wp5"]; testBoard[6][5] = pieceMap["wp6"]; testBoard[6][6] = pieceMap["wp7"]; testBoard[6][7] = pieceMap["wp8"];
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][2] = pieceMap["wb1"]; testBoard[7][3] = pieceMap["wq"]; testBoard[7][4] = pieceMap["wk"];
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][6] = pieceMap["wkn2"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[5][2] = pieceMap["wkn1"];
                    testBoard[4][2] = pieceMap["wb2"]; testBoard[0][3] = pieceMap["bk"]; testBoard[4][3] = pieceMap["bb1"]; testBoard[4][7] = pieceMap["bq"];
                    break;
                case 3: // Test if AI blunder pieces
                    testBoard[6][0] = pieceMap["wp1"]; testBoard[6][1] = pieceMap["wp2"]; testBoard[6][2] = pieceMap["wp3"]; testBoard[6][3] = pieceMap["wp4"];
                    testBoard[4][4] = pieceMap["wp5"]; testBoard[6][5] = pieceMap["wp6"]; testBoard[6][6] = pieceMap["wp7"]; testBoard[6][7] = pieceMap["wp8"];
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][2] = pieceMap["wb1"]; testBoard[7][3] = pieceMap["wq"]; testBoard[7][4] = pieceMap["wk"];
                    testBoard[7][0] = pieceMap["wr1"]; testBoard[7][6] = pieceMap["wkn2"]; testBoard[7][7] = pieceMap["wr2"]; testBoard[5][2] = pieceMap["wkn1"];
                    testBoard[4][2] = pieceMap["wb2"]; testBoard[0][4] = pieceMap["bk"]; testBoard[3][2] = pieceMap["bb1"]; testBoard[4][7] = pieceMap["bq"];
                    testBoard[1][0] = pieceMap["bp1"]; testBoard[1][1] = pieceMap["bp2"]; testBoard[1][2] = pieceMap["bp3"]; testBoard[1][3] = pieceMap["bp4"];
                    testBoard[3][4] = pieceMap["bp5"]; testBoard[1][5] = pieceMap["bp6"]; testBoard[1][6] = pieceMap["bp7"]; testBoard[1][7] = pieceMap["bp8"];
                    testBoard[0][0] = pieceMap["br1"]; testBoard[0][1] = pieceMap["bkn1"]; testBoard[0][2] = pieceMap["bb2"]; testBoard[0][6] = pieceMap["bkn2"];
                    testBoard[0][7] = pieceMap["br2"];
                    break;
                case 4: // Test if AI makes illegal move and confirms it (this will break if AI is white and it is set to white's turn which ia also an impossible scenario)
                    testBoard[1][4] = pieceMap["wr1"]; testBoard[2][0] = pieceMap["bp1"]; testBoard[2][2] = pieceMap["wr2"]; testBoard[4][4] = pieceMap["bk"];
                    testBoard[4][0] = pieceMap["wp2"]; testBoard[4][1] = pieceMap["wk"]; testBoard[4][3] = pieceMap["wp3"]; testBoard[4][7] = pieceMap["bp2"];
                    testBoard[7][0] = pieceMap["br2"];
                    break;
                case 5:
                    testBoard[0][6] = pieceMap["wk"]; testBoard[0][0] = pieceMap["bk"]; testBoard[2][6] = pieceMap["bq"];
                    break;
            }
        }
    }
}
