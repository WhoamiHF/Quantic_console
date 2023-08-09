using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal abstract class BoardViewer
    {
        public BoardViewer() { }

        public abstract void ViewBoard(Board board);
        public abstract void ViewPlayerPieces(Player player);

        public abstract void ShowWin(Piece.Owners winner);
    }
}
