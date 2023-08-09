using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Square
    {
        PlacedPiece? _piece;
        public Square() {
            _piece = null;
        }

        public Square(PlacedPiece? piece)
        {
            this._piece = piece;
        }

        public void SetPiece(PlacedPiece piece)
        {
            this._piece = piece; 
        }

        public char GetSymbol()
        {
            if(_piece == null)
            {
                return '.';
            }
            else
            {
                return _piece.getSymbol();
            }
        }
        
        public PlacedPiece Piece
        {
            get
            {
                return _piece;
            }
        }
    }
}
