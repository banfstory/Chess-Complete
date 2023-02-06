using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class Rules : Form
    {
        public Rules()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
