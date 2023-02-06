using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class NewGame : Form
    {
        ChessGame Main;
        public NewGame(ChessGame main)
        {
            InitializeComponent();
            Main = main;
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PvP_Click(object sender, EventArgs e)
        {
            Main.ResetGame();
            Main.type.Text = "Player vs Player";
            Main.opponent = ChessGame.Opponent.Player;
            this.Close();
        }

        private void PvE_Click(object sender, EventArgs e)
        {
            Main.ResetGame();
            AISpecifications aiSpecs = new AISpecifications(Main, this);
            DialogResult show = aiSpecs.ShowDialog();
        }
    }
}
