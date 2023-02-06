using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    class LastMoveMade
    {
        int sizeOfBox = 45;
        Color backcolor;
        Graphics gObject;
        Brush brush;

        public LastMoveMade(Graphics GObject, Color color, Dictionary<PictureBox, PieceStateDetails> PieceStateMapping)
        {
            backcolor = color;
            gObject = GObject;
            brush = new SolidBrush(color);
        }

        public void DisplayLastMove(PictureBox[][] board, History history) 
        {
            History next = history.Next;
            if (next != null)
                next.Source.BackColor = Color.Transparent;
            if (history.Prev != null)
            {
                gObject.FillRectangle(brush, PieceDetails.ToCoordinate(history.SourceX), PieceDetails.ToCoordinate(history.SourceY), sizeOfBox, sizeOfBox);
                if (history.Source != null)
                    history.Source.BackColor = backcolor;
            }
            History prev = history.Prev;
            if (prev != null && prev.Prev != null && prev.Source != null) 
                prev.Source.BackColor = Color.Transparent;
        }
    }
}
