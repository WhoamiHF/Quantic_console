using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quantic_console
{
    internal class ArtificialPlayer : Player
    {
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public ArtificialPlayer(List<Piece> pieces, Piece.Owners player) : base(pieces, player)
        {
        }

        public ArtificialPlayer(Piece.Owners player) : base(player)
        {
        }

        public ArtificialPlayer(ArtificialPlayer other) : base(other)
        {
        }

        public override Player copy()
        {
            ArtificialPlayer result = new ArtificialPlayer(this);
           
            return result;
        }

        public override Move SelectMove(GameLogic gameLogic, Board board,Player current, Player other)
        {

            HashSet<Piece.Shapes> shapes = GetUsedShapes(board);

            return MiniMaxDepthZero(gameLogic, board,current,other,"",shapes,-1000,1000).Move!;
        }

        private HashSet<Piece.Shapes> GetUsedShapes(Board board)
        {
            HashSet<Piece.Shapes> shapes = new HashSet<Piece.Shapes>();
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(board.Squares[i][j].Piece != null)
                    {
                        shapes.Add(board.Squares[i][j].Piece.Shape);
                    }
                }
            }
            return shapes;
        }

        private HashSet<Piece.Shapes> DetermineWhichShapesToConsider(HashSet<Piece.Shapes> usedShapes)
        {
            if(usedShapes.Count == 4)
            {
                return usedShapes;
            }

            if (!usedShapes.Contains(Piece.Shapes.PYRAMID))
            {
                usedShapes.Add(Piece.Shapes.PYRAMID);
                return usedShapes;
            }

            if (!usedShapes.Contains(Piece.Shapes.CUBE))
            {
                usedShapes.Add(Piece.Shapes.CUBE);
                return usedShapes;
            }

            if (!usedShapes.Contains(Piece.Shapes.CYLINDER))
            {
                usedShapes.Add(Piece.Shapes.CYLINDER);
                return usedShapes;
            }

            if (!usedShapes.Contains(Piece.Shapes.SPHERE))
            {
                usedShapes.Add(Piece.Shapes.SPHERE);
                return usedShapes;
            }

            return usedShapes;
        }

        class MinimaxResult
        {
            double score;
            Move? move;
            public MinimaxResult(double score, Move move)
            {
                this.score = score;
                this.move = move;
            }

            public double Score
            {
                get { 
                    return score; 
                }

                set { 
                    score = value; 
                }
            }

            public Move? Move
            {
                get
                {
                    return move;
                }

                set
                {
                    move = value;
                }
            }
        }

        class MinimaxInfo
        {
            double alpha;
            double beta;
            public MinimaxInfo(double alpha, double beta)
            {
                this.alpha = alpha;
                this.beta = beta;
            }   

            public double Alpha
            {
                get
                {
                    return alpha;
                }
                set
                {
                    //Console.WriteLine("alpha:" + alpha);
                    alpha = value;
                }
            }
            public double Beta
            {
                get
                {
                    return beta;
                }
                set
                {
                    //Console.WriteLine("beta:" + beta);
                    beta = value;
                }
            }
        }

        private MinimaxResult MiniMaxDepthZero(GameLogic gameLogic, Board board,
            Player currentPlayer, Player otherPlayer, String previous, HashSet<Piece.Shapes> usedShapes,
            double alpha, double beta)
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = new CancellationTokenSource();

            List<Move> moves = gameLogic.GetPossibleMoves(this, DetermineWhichShapesToConsider(usedShapes));

            MinimaxResult result = new MinimaxResult(-1000, null);
            MinimaxInfo alphaBeta = new MinimaxInfo(alpha, beta);
     
            int i = 0;
            Console.WriteLine("number of moves" + moves.Count);
            foreach (Move move in moves)
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    singleThreadMiniMax(previous + " " + i, gameLogic, currentPlayer,
                    otherPlayer, board, move, usedShapes, alphaBeta, i, result);
                });        
                
            }
            Thread.Sleep(5000);
          
            return result;
        }

        private void singleThreadMiniMax(String previous, GameLogic gameLogic, Player currentPlayer,
            Player otherPlayer, Board board, Move move, HashSet<Piece.Shapes> usedShapes, MinimaxInfo alphaBeta, int i, MinimaxResult result)
        {
            if (_cancellationTokenSource.Token.IsCancellationRequested)
            {
                return;
            }

            double score = EvaluateMove(previous + " " + i, gameLogic, currentPlayer, otherPlayer, board, move, 0,
                usedShapes, alphaBeta.Alpha, alphaBeta.Beta);;

            Console.WriteLine(score +" " + move);
            lock (result)
            {
                if (score > result.Score || result.Move == null)
                {
                    result.Score = score;
                    result.Move = move;
                    Console.WriteLine(move);
                }
            }

            double max = 1000;
            lock (_cancellationTokenSource)
            {
                if (result.Score == max)
                {
                    Console.WriteLine("cancellation because of max!" + move);
                    _cancellationTokenSource.Cancel();
                }

                lock (alphaBeta)
                {
                    alphaBeta.Alpha = Math.Max(alphaBeta.Alpha, result.Score);
                    if (alphaBeta.Beta <= alphaBeta.Alpha)
                    {
                        //Console.WriteLine("prunning depth: " + depth);
                        Console.WriteLine("cancellation because of alphaBeta!" + move);
                        _cancellationTokenSource.Cancel();
                    }
                }
            }
        }

        private MinimaxResult MiniMax(GameLogic gameLogic,Board board, int depth, 
            Player currentPlayer, Player otherPlayer,String previous,HashSet<Piece.Shapes> usedShapes,
            double alpha, double beta)
        {

            if(depth == 4)
            {
                return new MinimaxResult(Evaluate(gameLogic, currentPlayer, otherPlayer),null);
            }

            List<Move> moves = gameLogic.GetPossibleMoves(this,DetermineWhichShapesToConsider(usedShapes));

            double bestScore = depth % 2 == 0 ? -1000 : 1000;

            Move? bestMove = null;
            int i = 0;
            foreach (Move move in moves)
            {
                double score = EvaluateMove(previous + " " + i, gameLogic, currentPlayer, otherPlayer, board, move, depth,usedShapes,alpha,beta);
                i++;

                if ((score < bestScore && depth % 2 == 1) ||(score > bestScore && depth % 2 == 0) || bestMove == null)
                {
                    bestScore = score;
                    bestMove = move; 
                }

                double max = depth % 2 == 0 ? 1000 : -1000;
                if (bestScore == max)
                {
                    break;
                }

                if (depth % 2 == 0)
                {
                    alpha = Math.Max(alpha, bestScore);
                }
                else
                {
                    beta = Math.Min(beta, bestScore);
                }

                if (beta <= alpha)
                {
                    //Console.WriteLine("prunning depth: " + depth);
                    break;
                }

            }
            return new MinimaxResult(bestScore,bestMove!);
        }

        private Double EvaluateMove(String previous,GameLogic gameLogic,Player currentPlayer, Player otherPlayer, 
            Board board, Move move, int depth,HashSet<Piece.Shapes> usedShapes, double alpha, double beta)
        {
            //Console.WriteLine(current);
            GameLogic logicCopy = new GameLogic(gameLogic);
            Player currentPlayerCopy = currentPlayer.copy();
            Player otherPlayerCopy = otherPlayer.copy();
            Board boardCopy = board.copy();


            logicCopy.MakeMove(move, boardCopy, currentPlayerCopy);

            if (GameLogic.CheckWin(boardCopy, move))
            {
                return depth % 2 == 0 ? 1000 : -1000;
            }

            HashSet<Piece.Shapes> usedShapesUpdated = new HashSet<Piece.Shapes>();

            foreach (Piece.Shapes shape in usedShapes)
            {
                usedShapesUpdated.Add(shape);
            }


            if (!usedShapesUpdated.Contains(move.Shape))
            {
                usedShapesUpdated.Add(move.Shape);
            }

            double score = MiniMax(logicCopy, boardCopy, depth + 1, otherPlayerCopy,
                currentPlayerCopy, previous, usedShapesUpdated, alpha, beta).Score;
            return score;
        }

        private double Evaluate(GameLogic gameLogic,Player currentPlayer,Player otherPlayer)
        {
            return gameLogic.GetPossibleMoves(currentPlayer,null).Count - gameLogic.GetPossibleMoves(otherPlayer,null).Count;
        }
    }
}
