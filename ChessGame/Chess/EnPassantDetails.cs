using System.Windows.Forms;

namespace Chess
{
    // move details of how the pawn enpassants
    public class EnPassantDetails
    {
        private int y;
        private int x;
        private PictureBox target;

        public int Y { get { return y; } }
        public int X { get { return x; } }
        public PictureBox Target { get { return target; } }


        public EnPassantDetails(int Y, int X, PictureBox Target) 
        {
            y = Y;
            x = X;
            target = Target;
        }
    }
}
