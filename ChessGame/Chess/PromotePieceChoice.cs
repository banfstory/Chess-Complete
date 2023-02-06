using System;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace Chess
{
    public partial class PromotePieceChoice : Form
    {
        private Promote Promote;

        public PromotePieceChoice(Promote promote)
        {
            InitializeComponent();
            this.ControlBox = false;
            queen.Checked = true;
            Promote = promote;
        }

        private void promote_Click(object sender, EventArgs e)
        {
            if (queen.Checked)
            {
                Promote.PiecePromoted = ChessGame.pieceName.Queen;
            }
            else if (rook.Checked)
            {
                Promote.PiecePromoted = ChessGame.pieceName.Rook;
            }
            else if (bishop.Checked)
            {
                Promote.PiecePromoted = ChessGame.pieceName.Bishop;
            }
            else if (knight.Checked) 
            {
                Promote.PiecePromoted = ChessGame.pieceName.Knight;
            }
            this.Close();
        }
    }
}
