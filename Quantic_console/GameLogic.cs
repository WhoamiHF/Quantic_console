using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class GameLogic
    {
        Dictionary<Piece.Shapes, HashSet<Coordinates>> possibleTurnsPlayerOne;
        Dictionary<Piece.Shapes, HashSet<Coordinates>> possibleTurnsPlayerTwo;

        public GameLogic()
        {
            possibleTurnsPlayerOne = new Dictionary<Piece.Shapes, HashSet<Coordinates>>();
            possibleTurnsPlayerTwo = new Dictionary<Piece.Shapes, HashSet<Coordinates>>();

            GetAllPossibleMoves(possibleTurnsPlayerOne);
            GetAllPossibleMoves(possibleTurnsPlayerTwo);
        }

        public GameLogic(GameLogic other)
        {
            possibleTurnsPlayerOne = new Dictionary<Piece.Shapes, HashSet<Coordinates>>();
            foreach (var entry in other.possibleTurnsPlayerOne)
            {
                possibleTurnsPlayerOne[entry.Key] = new HashSet<Coordinates>(entry.Value);
            }

            possibleTurnsPlayerTwo = new Dictionary<Piece.Shapes, HashSet<Coordinates>>();
            foreach (var entry in other.possibleTurnsPlayerTwo)
            {
                possibleTurnsPlayerTwo[entry.Key] = new HashSet<Coordinates>(entry.Value);
            }
        }

        public void GetAllPossibleMoves(Dictionary<Piece.Shapes, HashSet<Coordinates>> possibleMoves)
        {
            foreach (Piece.Shapes shape in Enum.GetValues(typeof(Piece.Shapes)))
            {
                HashSet<Coordinates> coordinates = new HashSet<Coordinates>();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        coordinates.Add(new Coordinates(i, j));
                    }
                }
                possibleMoves.Add(shape, coordinates);

            }
        }

        public List<Move> GetPossibleMoves(Player player,HashSet<Piece.Shapes>? shapes)
        {
            shapes ??= GetPlayersShapes(player);

            List<Move> result = new List<Move>();
            foreach (Piece.Shapes shape in shapes)
            {
                HashSet<Coordinates> possibilities = GetPossibleMoves(player, shape);
                foreach (Coordinates coordinates in possibilities)
                {
                    result.Add(new Move(coordinates.X, coordinates.Y,player.Owner,shape));
                }
            }
            return result;
        }

        public HashSet<Coordinates> GetPossibleMoves(Player player, Piece.Shapes shape)
        {
            if(player.Owner == Piece.Owners.PLAYER_ONE)
            {
                if (possibleTurnsPlayerOne.ContainsKey(shape))
                {
                    return possibleTurnsPlayerOne[shape];
                }
                return new HashSet<Coordinates>();
            }

            if (possibleTurnsPlayerTwo.ContainsKey(shape))
            {
                return possibleTurnsPlayerTwo[shape];

            }
        
            return new HashSet<Coordinates>();
          
        }

        public static HashSet<Piece.Shapes> GetPlayersShapes(Player player)
        {
            HashSet<Piece.Shapes> shapes = new HashSet<Piece.Shapes>();
            foreach (Piece piece in player.Pieces)
            {
                if (!shapes.Contains(piece.Shape))
                {
                    shapes.Add(piece.Shape);
                }
            }
            return shapes;
        }

        public static bool CheckMove(Move move, Board board, Player player)
        {
            return CheckAvaibility(move, board, player) && CheckTurn(move, player.Owner) && CheckEmptiness(move, board)
                && CheckColumn(move, board) && CheckRow(move, board) && CheckArea(move, board);
        }

        private static bool CheckTurn(Move move, Piece.Owners player)
        {
            return move.Player == player;
        }

        private static bool CheckEmptiness(Move move, Board board)
        {
            return board.Squares[move.X][move.Y].Piece == null;
        }

        private static bool CheckColumn(Move move, Board board)
        {
            bool allOkay = true;

            for (int i = 0; i < 4; i++)
            {
                //Console.WriteLine("Checking " + move.X + "," + i);
                bool oneOkay = board.Squares[move.X][i].Piece == null;
                oneOkay = oneOkay || board.Squares[move.X][i].Piece.Shape != move.Shape;
                oneOkay = oneOkay || board.Squares[move.X][i].Piece.Owner == move.Player;

                allOkay = allOkay && oneOkay;
            }

            return allOkay;
        }

        private static bool CheckRow(Move move, Board board)
        {
            bool allOkay = true;

            for (int i = 0; i < 4; i++)
            {
                //Console.WriteLine("Checking " + i + "," + move.Y);
                bool oneOkay = board.Squares[i][move.Y].Piece == null;
                oneOkay = oneOkay || board.Squares[i][move.Y].Piece.Shape != move.Shape;
                oneOkay = oneOkay || board.Squares[i][move.Y].Piece.Owner == move.Player;

                allOkay = allOkay && oneOkay;
            }

            return allOkay;
        }

        private static bool CheckArea(Move move, Board board)
        {
            bool allOkay = true;

            int startOfAreaX = ((int)(move.X / 2)) * 2;
            int startOfAreaY = ((int)(move.Y / 2)) * 2;

            for (int x = startOfAreaX; x <= startOfAreaX + 1; x++)
            {
                for (int y = startOfAreaY; y <= startOfAreaY + 1; y++)
                {
                    //Console.WriteLine("Checking " + x + "," + y); 
                    bool oneOkay = board.Squares[x][y].Piece == null;
                    oneOkay = oneOkay || board.Squares[x][y].Piece.Shape != move.Shape;
                    oneOkay = oneOkay || board.Squares[x][y].Piece.Owner == move.Player;

                    allOkay = allOkay && oneOkay;
                }
            }

            return allOkay;
        }

        private static bool CheckAvaibility(Move move, Board board, Player player)
        {
            foreach (Piece piece in player.Pieces)
            {
                if (piece.Shape == move.Shape)
                {
                    return true;
                }
            }
            return false;
        }

        public void MakeMove(Move move, Board? board, Player player)
        {
            board?.Squares[move.X][move.Y].SetPiece(new PlacedPiece(move.Player, move.Shape, move.X, move.Y));

            foreach (Piece piece in player.Pieces)
            {
                if (piece.Shape == move.Shape)
                {
                    player.Pieces.Remove(piece);
                    break;
                }
            }

            RemoveNotValidMoves(move, player);
        }

        public void RemoveNotValidMoves(Move move, Player player)
        {
            Dictionary<Piece.Shapes, HashSet<Coordinates>> possibilities = player.Owner == Piece.Owners.PLAYER_ONE ?
                possibleTurnsPlayerTwo : possibleTurnsPlayerOne;

            RemoveCoordinates(new Coordinates(move.X, move.Y), possibleTurnsPlayerOne);
            RemoveCoordinates(new Coordinates(move.X, move.Y), possibleTurnsPlayerTwo);

            RemoveArea(move.Shape, possibilities, move);
            RemoveColumn(move.Shape, possibilities, move);
            RemoveRow(move.Shape, possibilities, move);
        }

        private void RemoveRow(Piece.Shapes shape, Dictionary<Piece.Shapes, HashSet<Coordinates>> possibilities, Move move)
        {
            if (possibilities.ContainsKey(shape))
            {
                List<Coordinates> coordinatesToRemove = new List<Coordinates>();
                foreach (Coordinates coordinates in possibilities[shape])
                {
                    if (coordinates.X == move.X)
                    {
                        coordinatesToRemove.Add(coordinates);
                    }
                }



                foreach (Coordinates coordinates in coordinatesToRemove)
                {
                    possibilities[shape].Remove(coordinates);
                }

                if (possibilities[shape].Count == 0)
                {
                    possibilities.Remove(shape);
                }
            }
        }

        private void RemoveColumn(Piece.Shapes shape, Dictionary<Piece.Shapes, HashSet<Coordinates>> possibilities, Move move)
        {
            if (possibilities.ContainsKey(shape))
            {
                List<Coordinates> coordinatesToRemove = new List<Coordinates>();
                foreach (Coordinates coordinates in possibilities[shape])
                {
                    if (coordinates.Y == move.Y)
                    {
                        coordinatesToRemove.Add(coordinates);
                    }
                }


                foreach (Coordinates coordinates in coordinatesToRemove)
                {
                    possibilities[shape].Remove(coordinates);
                }

                if (possibilities[shape].Count == 0)
                {
                    possibilities.Remove(shape);
                }
            }
        }

        private void RemoveArea(Piece.Shapes shape, Dictionary<Piece.Shapes, HashSet<Coordinates>> possibilities, Move move)
        {
            if (possibilities.ContainsKey(shape))
            {
                List<Coordinates> coordinatesToRemove = new List<Coordinates>();
                foreach (Coordinates coordinates in possibilities[shape])
                {
                    if (coordinates.X / 2 == move.X / 2 && coordinates.Y / 2 == move.Y / 2)
                    {
                        coordinatesToRemove.Add(coordinates);
                    }
                }


                foreach (Coordinates coordinates in coordinatesToRemove)
                {
                    possibilities[shape].Remove(coordinates);
                }

                if (possibilities[shape].Count == 0)
                {
                    possibilities.Remove(shape);
                }
            }
        }

        private void RemoveCoordinates(Coordinates coordinates, Dictionary<Piece.Shapes, HashSet<Coordinates>> possibilities)
        {
            foreach (Piece.Shapes shape in Enum.GetValues(typeof(Piece.Shapes)))
            {
                if (!possibilities.ContainsKey(shape))
                {
                    continue;
                }

                foreach(Coordinates coordinates2 in possibilities[shape])
                {
                    if (coordinates.X == coordinates2.X && coordinates.Y == coordinates2.Y)
                    {
                        possibilities[shape].Remove(coordinates2);

                        if (possibilities[shape].Count == 0)
                        {
                            possibilities.Remove(shape);
                        }
                        break;
                    }
                }
            }

        }

        public static bool CheckWin(Board board,Move move)
        {
            return CheckColumnForWin(move,board) || CheckRowForWin(move,board) || CheckAreaForWin(move, board);
        }

        public bool CheckLoss(Piece.Owners player)
        {
            if(player == Piece.Owners.PLAYER_ONE)
            {
                return possibleTurnsPlayerOne.Count == 0;
            }
            else
            {
                return possibleTurnsPlayerTwo.Count == 0;

            }
        }

        private static bool CheckColumnForWin(Move move, Board board)
        {
            HashSet<Piece.Shapes> shapes = new HashSet<Piece.Shapes>();

            for (int i = 0; i < 4; i++)
            {
                if(board.Squares[move.X][i].Piece == null)
                {
                    return false;
                }

                if (shapes.Contains(board.Squares[move.X][i].Piece.Shape))
                {
                    return false;
                }

                shapes.Add(board.Squares[move.X][i].Piece.Shape);
            }

            return true;
        }

        private static bool CheckRowForWin(Move move, Board board)
        {
            HashSet<Piece.Shapes> shapes = new HashSet<Piece.Shapes>();

            for (int i = 0; i < 4; i++)
            {
                if (board.Squares[i][move.Y].Piece == null)
                {
                    return false;
                }

                if (shapes.Contains(board.Squares[i][move.Y].Piece.Shape))
                {
                    return false;
                }

                shapes.Add(board.Squares[i][move.Y].Piece.Shape);
            }

            return true;
        }

        private static bool CheckAreaForWin(Move move, Board board)
        {
            HashSet<Piece.Shapes> shapes = new HashSet<Piece.Shapes>();

            int startOfAreaX = ((int)(move.X / 2)) * 2;
            int startOfAreaY = ((int)(move.Y / 2)) * 2;

            for (int x = startOfAreaX; x <= startOfAreaX + 1; x++)
            {
                for (int y = startOfAreaY; y <= startOfAreaY + 1; y++)
                {
                    if (board.Squares[x][y].Piece == null)
                    {
                        return false;
                    }

                    if (shapes.Contains(board.Squares[x][y].Piece.Shape))
                    {
                        return false;
                    }

                    shapes.Add(board.Squares[x][y].Piece.Shape);
                }
            }

            return true;
        }

    }
}
