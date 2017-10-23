using System.Collections.Generic;
using System.Linq;
using AI.Algorithms;
using Engine;
using Engine.Extensions;
using Move = AI.Move;

namespace AI
{
    public class AiEngine
    {
        private readonly IAlgorithm _algorithm;

        public AiEngine(IAlgorithm algorithm)
        {
            this._algorithm = algorithm;
        }

        public Move GetNextMove(BoardManager manager)
        {
            if (manager.ActivePill == null)
            {
                return new Move();
            }

            var moves = this.GetMoves(manager);

            return moves.FirstOrDefault() ?? new Move();
        }

        public IOrderedEnumerable<Move> GetMoves(BoardManager manager)
        {
            var moves = new List<Move>();
            var previousBlock = manager.PreviousPill;
            var pillClearings = manager.GameStats.TotalPillClearings;
            var bacteriaClearings = manager.GameStats.TotalBacteriaClearings;
            for (var rotation = 0; rotation < manager.ActivePill.UniqueRotations; rotation++)
            {
                for (var column = 0; column < manager.NumberOfColumns; column++)
                {
                    var tempManager = new BoardManager(manager.GameBoard.DeepClone(), manager.ActivePill.Clone(), manager.GameStats.Clone());
                    var tempBlock = tempManager.ActivePill.Clone();
                    tempBlock.Move(Engine.Move.Down);
                    tempBlock.Move(Engine.Move.Down);
                    tempBlock.Move(Engine.Move.Down);

                    if (rotation != 0)
                    {
                        for (var i = 0; i < rotation; i++)
                        {
                            tempBlock.Move(Engine.Move.RotateLeft);
                        }
                    }

                    tempBlock.Position.Column = column;

                    if (tempManager.CheckPill(tempBlock))
                    {
                        while (true)
                        {
                            tempManager.ActivePill.Merge(tempBlock);

                            tempBlock.Move(Engine.Move.Down);
                            if (!tempManager.CheckPill(tempBlock))
                            {
                                break;
                            }
                        }

                        tempManager.LockAndCheck();
                        var canSpawnBlock = tempManager.CanSpawn();
                        var rowsCleared = tempManager.GameStats.TotalPillClearings - pillClearings;
                        var bacteriaCleared = tempManager.GameStats.TotalBacteriaClearings - bacteriaClearings;
                        moves.Add(new Move
                                {
                                    GameboardWidth = manager.NumberOfColumns,
                                    ColumnOffSet = column - manager.ActivePill.Position.Column,
                                    Fitness = canSpawnBlock ? this._algorithm.CalculateFitness(tempManager.GameBoard, previousBlock, bacteriaCleared, rowsCleared) : int.MaxValue,
                                    IsValid = true,
                                    Rotation = rotation
                                });
                    }
                }
            }

            return moves.OrderByDescending(x => x.IsValid).ThenBy(x => x.Fitness);
        }
    }
}
