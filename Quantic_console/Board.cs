using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Board
    {
       

        private Square[][] _squares;

        public Board()
        {
            CreateSquares();
        }

        public Board(Board other)
        {
            _squares = new Square[4][];
            for (int x = 0; x < 4; x++)
            {
                _squares[x] = new Square[4];
                for (int y = 0; y < 4; y++)
                {
                    _squares[x][y] = new Square();
                    _squares[x][y].SetPiece(other.Squares[x][y].Piece);
                }
            }
        }

        public Board copy()
        {
            return new Board(this);
        }
        
        public Board(Square[][] squares)
        {
            this._squares = squares;
        }

        public Square[][] Squares
        {
            get
            {
                return _squares;
            }
        }

        private void CreateSquares()
        {
            _squares = new Square[4][];
            for (int x  = 0; x < 4; x++)
            {
                _squares[x] = new Square[4];
                for (int y = 0; y < 4; y++)
                {
                    _squares[x][y] = new Square();
                }
            }
        }
    }
}
