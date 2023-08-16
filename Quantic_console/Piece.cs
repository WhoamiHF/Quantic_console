using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Piece
    {
        public enum ShapeType { CUBE,SPHERE,PYRAMID,CYLINDER}
        public enum PlayerID { PLAYER_ONE,PLAYER_TWO}
        readonly ShapeType _shape;
        readonly PlayerID _owner;

        public ShapeType Shape
        {
            get
            {
                return _shape;
            }
        }

        public PlayerID Owner
        {
            get { 
                return _owner; 
            }
        }


        public Piece(PlayerID owner,ShapeType shape)
        {
            this._owner = owner;
            this._shape = shape;
        }

        public char GetSymbol()
        {
            switch (Shape)
            {
                case ShapeType.CUBE:
                    return _owner == PlayerID.PLAYER_ONE ? ('c') : ('C');
                case ShapeType.SPHERE:
                    return _owner == PlayerID.PLAYER_ONE ? ('s') : ('S');
                case ShapeType.PYRAMID:
                    return _owner == PlayerID.PLAYER_ONE ? ('p') : ('P');
                case ShapeType.CYLINDER:
                    return _owner == PlayerID.PLAYER_ONE ? ('r') : ('R');
                }
            return 'x';
        }
    }
}
