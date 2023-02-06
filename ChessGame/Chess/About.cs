using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
