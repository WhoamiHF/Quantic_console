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
            game.PlayGame(viewer);
            viewer.ViewBoard(game.Board);
        }
    }
}