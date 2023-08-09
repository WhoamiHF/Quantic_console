using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal abstract class Player
    {
        List<Piece> _pieces;
        Piece.Owners _player;

        public Player(List<Piece> pieces,Piece.Owners player)
        {
            this._pieces = pieces;
            this._player = player;
        }

        public Player(Piece.Owners player)
        {
            this._pieces = new List<Piece>();
            for(int i = 0; i < 2; i++)
            {
                _pieces.Add(new Piece(player, Piece.Shapes.PYRAMID));
                _pieces.Add(new Piece(player, Piece.Shapes.CUBE));
                _pieces.Add(new Piece(player, Piece.Shapes.SPHERE));
                _pieces.Add(new Piece(player, Piece.Shapes.CYLINDER));
            }
            this._player = player;
        }

        public Player(Player other)
        {
            this._player = other.Owner;
            this._pieces = new List<Piece>();

            foreach (Piece piece in other._pieces)
            {
                this._pieces.Add(piece);
            }
        }

        public abstract Player copy();

        public abstract Move? SelectMove(GameLogic logic,Board board,Player current, Player other);

        public List<Piece> Pieces { get
            {
                return _pieces;
            }
        }

        public Piece.Owners Owner
        {
            get
            {
                return _player;
            }
        }
    }
}
