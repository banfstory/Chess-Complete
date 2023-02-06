using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class AISpecifications : Form
    {
        ChessGame Main;
        NewGame GameMode;

        public AISpecifications(ChessGame main, NewGame gamemode)
        {
            InitializeComponent();
            normal.Checked = true;
            white.Checked = true;
            Main = main;
            GameMode = gamemode;
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            Main.type.Text = "Player vs AI";
            Main.opponent = ChessGame.Opponent.AI;
            foreach (RadioButton radio in level.Controls) 
            {
                if (radio.Checked)
                {
                    if (radio.Name == "easy")
                        Main.AIComplexity = 3;
                    else if (radio.Name == "normal")
                        Main.AIComplexity = 4;
                    else
                        Main.AIComplexity = 5;
                    break;
                }
            }
            foreach (RadioButton radio in color.Controls) 
            {
                if (radio.Checked) 
                {
                    if (radio.Name == "white")
                    {
                        Main.aiColor = ChessGame.AIColor.Black;
                        Main.type.Text = "(White) " + Main.type.Text;
                    }
                    else
                    {
                        Main.aiColor = ChessGame.AIColor.White;
                        Main.type.Text = "(Black) " + Main.type.Text;
                    }
                    break;
                }
            }
            this.Close();
            GameMode.Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
