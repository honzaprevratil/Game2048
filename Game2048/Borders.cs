using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    class Borders
    {
        // start Cols (X) index
        public int StartX { get; set; }

        // end Cols (X) index 
        public int EndX { get; set; }

        // start Rows (Y) index
        public int StartY { get; set; }

        // end Rows (Y) index 
        public int EndY { get; set; }

        // index change by
        public int Change { get; set; }

        public Borders(int startX, int endX, int startY, int endY, int change)
        {
            StartX = startX;
            EndX = endX;
            StartY = startY;
            EndY = endY;
            Change = change;
        }
    }
}
