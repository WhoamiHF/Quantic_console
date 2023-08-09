using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Move
    {
        int _x;
        int _y;
        Piece.Owners _player;
        Piece.Shapes _shape;

        public Move(int x, int y, Piece.Owners player, Piece.Shapes shape)
        {
            this._x = x;
            this._y = y;
            this._player = player;
            this._shape = shape;
        }

        public void Copy(Move other)
        {
            this._x = other.X; 
            this._y = other.Y;
            this._player = other.Player;
            this._shape = other.Shape;
        }
        public int X { get { return _x; } }
        public int Y { get { return _y;} }
        public Piece.Owners Player
        {
            get
            {
                return _player;
            }
        }
        public Piece.Shapes Shape
        {
            get
            {
                return _shape;
            }
        }

        public override String ToString()
        {
            String player = _player == Piece.Owners.PLAYER_ONE ? "one" : "two";
            return "Player " + player + " can place " + Shape + " to coordinates [" + X + ", " + Y + "]"; 
        }
    }

}
