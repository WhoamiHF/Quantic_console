using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class ConsoleBoardViewer : BoardViewer
    {
        public override void ShowWin(Piece.Owners winner)
        {
            String message = winner == Piece.Owners.PLAYER_ONE ? "Player one has won!" : "Player two has won!";
            Console.WriteLine(message); 
        }

        public override void ViewBoard(Board board)
        {
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    Console.Write(board.Squares[x][y].GetSymbol());
                }
                Console.WriteLine();
            }
        }

        public override void ViewPlayerPieces(Player player)
        {
            foreach(Piece piece in player.Pieces)
            {
                Console.Write(piece.getSymbol() + " ");
            }
            Console.WriteLine();
        }
    }
}
