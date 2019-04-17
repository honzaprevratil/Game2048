using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    public class Direction
    {
        // move cols value
        public int X { get; set; }
        
        // move rows value
        public int Y { get; set; }
        
        // maximum distance move
        public int MaxDistance { get; set; }

        public Direction(int x, int y, int maxDistance)
        {
            X = x;
            Y = y;
            MaxDistance = maxDistance;
        }
    }
}
