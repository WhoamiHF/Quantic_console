using Quantic_console;
using System;

namespace Quantic
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.GetPlayers();
            
            ConsoleBoardViewer viewer = new ConsoleBoardViewer();
            viewer.ViewBoard(game.Board);

            //game.Board.Squares[1][1].SetPiece(new PlacedPiece(Piece.Owners.PLAYER_ONE, Piece.Shapes.SPHERE, 1, 1));
            //game.Board.Squares[0][0].SetPiece(new PlacedPiece(Piece.Owners.PLAYER_ONE, Piece.Shapes.PYRAMID, 0, 0));
            //game.Board.Squares[2][2].SetPiece(new PlacedPiece(Piece.Owners.PLAYER_ONE, Piece.Shapes.CUBE, 2, 2));
            //game.Board.Squares[3][3].SetPiece(new PlacedPiece(Piece.Owners.PLAYER_ONE, Piece.Shapes.CYLINDER, 3, 3));
            game.PlayGame(viewer);

            viewer.ViewBoard(game.Board);

        }
    }
}