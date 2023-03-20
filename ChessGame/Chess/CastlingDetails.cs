using System.Windows.Forms;

namespace Chess
{
    // move details of how the king castles
    public class CastlingDetails
    {
        private int sourceY;
        private int sourceX;
        private int destinationY;
        private int destinationX;
        private PictureBox source;
        private PictureBox destination;

        public int SourceY { get { return sourceY; } }
        public int SourceX { get { return sourceX; } }
        public int DestinationY { get { return destinationY; } }
        public int DestinationX { get { return destinationX; } }
        public PictureBox Source { get { return source; } }
        public PictureBox Destination { get { return destination; } }

        public CastlingDetails(int SourceY, int SourceX, int DestinationY, int DestinationX, PictureBox Source, PictureBox Destination)
        {
            sourceY = SourceY;
            sourceX = SourceX;
            destinationY = DestinationY;
            destinationX = DestinationX;
            source = Source;
            destination = Destination;
        }
    }
}
