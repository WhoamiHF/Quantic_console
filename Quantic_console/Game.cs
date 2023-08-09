using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class Game
    {
        Player player1;
        Player player2;
        GameLogic rules;
        Board board;
        GameState state;

        enum GameState { PLAYING,FIRST_PLAYER_WON,SECOND_PLAYER_WON}

        public Game() {
            player1 = new User(Piece.Owners.PLAYER_ONE);
            player2 = new User(Piece.Owners.PLAYER_TWO);
            rules = new GameLogic();
            board = new Board();
            state = new GameState();
        }

        public Board Board { get { return board; } }

        public void GetPlayers()
        {
            bool selected = false;
            while (!selected)
            {
                Console.WriteLine("Do you want to play against other user or against computer?");
                Console.WriteLine("Press \"u\" to play against another user or \"c\" to play against computer");
                char choice = Console.ReadKey().KeyChar;
                if (choice.Equals('u'))
                {
                    Console.WriteLine("\nYou have pressed: " + choice + " and chosen to play against another user");
                    selected = true;
                   
                }
                else if (choice.Equals('c'))
                {
                    Console.WriteLine("\nYou have pressed: " + choice + " and chosen to play against computer");
                    player2 = new ArtificialPlayer(Piece.Owners.PLAYER_TWO);
                    selected = true;
                }
                else
                {
                    Console.WriteLine("\nYou have pressed " + choice + " which is not a valid choice");
                }
            }
        }

        public void PlayGame(BoardViewer viewer)
        {
            state = GameState.PLAYING;
            GameLogic gameLogic = new GameLogic();
            while(state == GameState.PLAYING)
            {
                Console.WriteLine("First player's turn");

                if (gameLogic.CheckLoss(Piece.Owners.PLAYER_ONE))
                {
                    state = GameState.SECOND_PLAYER_WON;
                    viewer.ShowWin(Piece.Owners.PLAYER_TWO);
                    break;
                }

                viewer.ViewPlayerPieces(player1);
                Move? move = player1.SelectMove(gameLogic,board,player1,player2);

                while (!GameLogic.CheckMove(move, board, player1))
                {
                    move = player1.SelectMove(gameLogic, board, player1, player2);
                }
                gameLogic.MakeMove(move, board,player1);

                /*List<Move> moves = gameLogic.GetPossibleMoves(player1,null);
                foreach(Move move2 in moves)
                {
                    Console.WriteLine($"Move {move2}");
                }*/

                if (GameLogic.CheckWin(board, move))
                {
                    state = GameState.FIRST_PLAYER_WON;
                    viewer.ShowWin(Piece.Owners.PLAYER_ONE);
                }

                if(state != GameState.PLAYING)
                {
                    break;
                }

                viewer.ViewBoard(board);

                Console.WriteLine("Second player's turn");
                if (gameLogic.CheckLoss(Piece.Owners.PLAYER_TWO))
                {
                    state = GameState.FIRST_PLAYER_WON;
                    viewer.ShowWin(Piece.Owners.PLAYER_ONE);
                    break;
                }

                viewer.ViewPlayerPieces(player2);
                
                move = player2.SelectMove(gameLogic, board, player2, player1);
                
                while (!GameLogic.CheckMove(move, board, player2))
                {
                    move = player2.SelectMove(gameLogic, board, player2, player1);
                }
                gameLogic.MakeMove(move, board,player2);
                
                if (GameLogic.CheckWin(board, move))
                {
                    state = GameState.SECOND_PLAYER_WON;
                    viewer.ShowWin(Piece.Owners.PLAYER_TWO);
                }

                /*List<Move> moves2 = gameLogic.GetPossibleMoves(player2,null);
                foreach (Move move2 in moves2)
                {
                    Console.WriteLine($"Move {move2}");
                }*/

                viewer.ViewBoard(board);
            }
        }
    }
}
