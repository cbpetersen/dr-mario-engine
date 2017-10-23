using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AI;
using AI.Algorithms;
using AI.Algorithms.Weights;
using Engine;
using Engine.Extensions;
using Move = Engine.Move;

namespace AIConsoleRunner
{
    public class Program
    {
        private static IDictionary<char, ConsoleColor> _colorMap;

        static void Main(string[] args)
        {
            var gameCount = 0;
            _colorMap = MapColors();
            AlgorithmSetting<AiWeights> algorithmSettings = null;
            var moves = new Stack<Move>();
            while (true)
            {
                algorithmSettings = new AlgorithmSetting<AiWeights>();
                if (gameCount % 2 == 0) {
                    algorithmSettings.Weights = new AiWeights()
                    {
                        BacteriasCleared = -10,
                        PillsCleared = -5,
                        ColumnTransitions = 2,
                        RowTransitions = 3,
                        NumberOfHoles = 10,
                        WellSums = 5,
                        LandingHeight = 1
                    };
                }

                var gameManager = new GameManager(20, 10);
                gameManager.AddBacterias(20, 3);
                var ai = new AiEngine(new FeatureAi(algorithmSettings.Weights));
                IEnumerator moveIterator = null;
                AI.Move aiMove = null;
                var blockNumber = -1;
                while (!gameManager.GameState.IsGameOver())
                {
                    PrintState(gameManager, moves, aiMove);
                    Thread.Sleep(50);

                    gameManager.OnGameLoopStep();

                    if (moveIterator != null && moveIterator.MoveNext())
                    {
                        gameManager.MoveBlock((Move) moveIterator.Current);
                        moves.Push((Move) moveIterator.Current);
                        continue;
                    }

                    // Make sure we only calculate best move once per spawned block.
                    if (blockNumber != gameManager.GameStats.PillsSpawned)
                    {
                        aiMove = ai.GetNextMove(gameManager.BoardManager);
                        moveIterator = aiMove.Moves.GetEnumerator();
                        blockNumber = gameManager.GameStats.PillsSpawned;
                    }
                }


                //Console.Clear();
                Console.Write("{0}, ", gameManager.GameStats.Fitness);
                Console.Write("Game Over {0}", gameManager.GameStats.Fitness);
                //Console.WriteLine("Game Over {0}", gameManager.GameStats.Fitness);
                //Console.WriteLine();
                //Console.WriteLine();
                //if (algorithmSettings.)
                //httpclient.PostStats(new GameResult<AiWeights>(algorithmSettings)
                //{
                //  PillsSpawned = gameManager.GameStats.PillsSpawned,
//                  Bacterias = gameManager.GameStats.TotalBacteriaClearings,
//                  Pills = gameManager.GameStats.TotalPillClearings,
                //  Fitness = gameManager.GameStats.Fitness
                //});
                gameCount++;
                Thread.Sleep(150);
                Thread.Sleep(5050);
            }
        }

        private static string BoardState(GameManager gameManager)
        {
            var tempBoardManager = gameManager.BoardManager;
            if (gameManager.ActiveBlock != null)
            {
                tempBoardManager = new BoardManager(gameManager.BoardManager.GameBoard.DeepClone(),
                    gameManager.BoardManager.ActivePill.Clone());
                tempBoardManager.Lockblock();
            }

            return tempBoardManager.GameBoard.MatrixToString(null);
//            return tempBoardManager.GameBoard.MatrixToString(tempBoardManager.ActivePill);
        }

        public static IDictionary<char, ConsoleColor> MapColors()
        {
            var set = new Dictionary<char, ConsoleColor>();
            var usedColors = new List<ConsoleColor> {ConsoleColor.Black};
            var gameColors = Enum.GetValues(typeof(Color)).Cast<Color>().ToArray();
            foreach (var gameColor in gameColors)
            {
                var color = Engine.Random.Instance().NextEnum(usedColors);
                usedColors.Add(color);

                set.Add(Char.Parse($"{(byte) gameColor}"), color);
            }

            return set;
        }
        
        private static void PrintState(GameManager gameManager, Stack<Move> moves, AI.Move aiMove)
        {
            Console.Clear();
            foreach (var c in BoardState(gameManager))
            {
                if (_colorMap.ContainsKey(c))
                {
                    Console.BackgroundColor = _colorMap[c];
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write(c);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Spawned: {0}", gameManager.GameStats.PillsSpawned);
            Console.WriteLine($"Move Fitness: {aiMove?.Fitness ?? 0}");
            Console.WriteLine($"Pills: {gameManager.GameStats.TotalPillClearings}");
            Console.WriteLine($"Bacterias: {gameManager.GameStats.TotalBacteriaClearings}");
            Console.WriteLine($"Fitness: {gameManager.GameStats.Fitness}");
            
            if (moves.Count > 0)
            {
                Console.WriteLine("Last 15 Moves: {0}", moves.Take(15).Select(x => x.ToString()).Aggregate((x, y) => $"{x},{y}"));
            }
        }
    }

    internal class AlgorithmSetting<T>
    {
        public T Weights { get; set; }
    }
}