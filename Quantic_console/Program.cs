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
           
            //game.SetComputerPlayer(null);

            ConsoleBoardViewer viewer = new ConsoleBoardViewer();
            GameLogic logic = new GameLogic();
            /*logic.MakeMove(new Move(1, 2, Piece.PlayerID.PLAYER_ONE, Piece.ShapeType.PYRAMID), game.Board, game.Player1);
            logic.MakeMove(new Move(0, 0, Piece.PlayerID.PLAYER_TWO, Piece.ShapeType.CUBE), game.Board, game.Player2);
            logic.MakeMove(new Move(1, 1, Piece.PlayerID.PLAYER_ONE, Piece.ShapeType.SPHERE), game.Board, game.Player1);
            logic.MakeMove(new Move(2, 3, Piece.PlayerID.PLAYER_TWO, Piece.ShapeType.SPHERE), game.Board, game.Player2);
            logic.MakeMove(new Move(2, 1, Piece.PlayerID.PLAYER_ONE, Piece.ShapeType.CUBE), game.Board, game.Player1);
            logic.MakeMove(new Move(0, 2, Piece.PlayerID.PLAYER_TWO, Piece.ShapeType.CYLINDER), game.Board, game.Player2);
            logic.MakeMove(new Move(3, 1, Piece.PlayerID.PLAYER_ONE, Piece.ShapeType.CUBE), game.Board, game.Player1);
            viewer.ViewBoard(game.Board); 
            Move move = game.Player2.SelectMove(logic,game.Board,game.Player2,game.Player1); 
            Console.WriteLine(move);*/

            viewer.ViewBoard(game.Board);
            game.PlayGame(viewer);
            viewer.ViewBoard(game.Board);
        }
    }
}