using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class User : Player
    {
        public User(Piece.Owners player) : base(player)
        {
        }

        public User(List<Piece> pieces, Piece.Owners player) : base(pieces, player)
        {
        }

        public User(User other) : base(other)
        {
        }

        public override Player copy()
        {
            User result = new User(this);

            return result;
        }

        public override Move? SelectMove(GameLogic gameLogic, Board board, Player current, Player other)
        {
            int x = -1;
            int y = -1;
            Console.WriteLine("Select x and y coordinate (0..3)");
            while (!int.TryParse(Console.ReadKey().KeyChar.ToString(), out x) ||
                !int.TryParse(Console.ReadKey().KeyChar.ToString(), out y) || x <0 || x >3 || y < 0 || y > 3)
            {
                Console.WriteLine("Select x and y coordinate (0..3)");
            }

            Console.WriteLine("\nSelect shape - c p r s");

            Piece.Shapes shape = Piece.Shapes.CUBE;
            char key = Console.ReadKey().KeyChar;
            switch (key)
            {
                case 'c':
                    shape = Piece.Shapes.CUBE;
                    break;
                case 'p':
                    shape = Piece.Shapes.PYRAMID;
                    break;
                case 'r':
                    shape = Piece.Shapes.CYLINDER;
                    break;
                case 's':
                    shape = Piece.Shapes.SPHERE;
                    break;
            }

            Console.WriteLine();
            return new Move(x, y, Owner, shape);
        }
    }
}
