using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class PlacedPiece : Piece      
    {
        int x;
        int y;
        public PlacedPiece(Owners owner, Shapes shape,int x, int y) : base(owner, shape)
        {
            
        }
    }
}
