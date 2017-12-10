using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AI;
using AI.Algorithms;
using AI.Algorithms.Weights;
using ApiClient.Entities;
using Engine;
using Engine.Extensions;
using Move = Engine.Move;

namespace AIConsoleRunner
{
    public class Program
    {
        private const int BacteriaCount = 30;
        private const int BacteriaDiversityCount = 3;
        private static IDictionary<char, ConsoleColor> _colorMap;

        static void Main(string[] args)
        {
            var uiEnabled = false;
            var gameCount = 0;
            Engine.Random.Instance().SetNewSeed(123);
            _colorMap = MapColors();
            var httpclient = new ApiClient.ApiClient(new Uri("http://localhost:3000"));
            var algorithmSettings = new AlgorithmSetting<AiWeights>();
            var moves = new Stack<Move>();
            while (true)
            {
                try
                {
                    algorithmSettings = httpclient.GetAlgorithmSettings<FeatureAi, AiWeights>(
                        new AlgorithmSetting<AiWeights> {Name = "Dr Mario - Engine"});
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine($"Error: retrying in a sec");
                    Thread.Sleep(1000);
                    continue;
                }

                var gameManager = new GameManager(20, 10);
                gameManager.AddBacterias(15, 3);
                var ai = new AiEngine(new FeatureAi(algorithmSettings.Weights));
                IEnumerator moveIterator = null;
                AI.Move aiMove = null;
                var blockNumber = -1;
                while (!gameManager.GameState.IsGameOver())
                {
                    if (uiEnabled)
                    {
                        PrintState(gameManager, moves, aiMove);                        
                    }

                    Thread.Sleep(2);

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

                    if (gameManager.Bacterias.Count < BacteriaCount)
                    {
                        var color = gameManager.Bacterias.GroupBy(x => x.Color).OrderByDescending(o => o.Count()).First().Key;
                        gameManager.AddBacteria(color);
                    }
                }

                if (uiEnabled)
                {
                    Console.Clear();
                    Console.Write("Game Over {0}", gameManager.GameStats.Fitness);
                    Console.WriteLine();
                    Console.WriteLine();
                }
                else
                {
                    Console.Write("{0}, ", gameManager.GameStats.Fitness);                    
                }

                try
                {
                    httpclient.PostStats(new GameResult<AiWeights>(algorithmSettings)
                    {
                        PillsSpawned = gameManager.GameStats.PillsSpawned,
                        Bacterias = gameManager.GameStats.TotalBacteriaClearings,
                        Pills = gameManager.GameStats.TotalPillClearings,
                        Fitness = gameManager.GameStats.Fitness
                    });
                    gameCount++;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Thread.Sleep(1000);
                }

                Thread.Sleep(50);
                if (uiEnabled)
                {
                    Thread.Sleep(5000);
                }
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
}