using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Piece
    {
        public enum Shapes { CUBE,SPHERE,PYRAMID,CYLINDER}
        public enum Owners { PLAYER_ONE,PLAYER_TWO}
        readonly Shapes _shape;
        readonly Owners _owner;

        public Shapes Shape
        {
            get
            {
                return _shape;
            }
        }

        public Owners Owner
        {
            get { 
                return _owner; 
            }
        }


        public Piece(Owners owner,Shapes shape)
        {
            this._owner = owner;
            this._shape = shape;
        }

        public Char getSymbol()
        {
            switch (Shape)
            {
                case Shapes.CUBE:
                    return ('c');
                case Shapes.SPHERE:
                    return ('s');
                case Shapes.PYRAMID:
                    return ('p');
                case Shapes.CYLINDER:
                    return ('r');
                }
            return 'x';
        }
    }
}
