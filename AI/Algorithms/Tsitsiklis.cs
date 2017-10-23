using AI.Algorithms.Weights;
using Engine;

namespace AI.Algorithms
{
    /// <summary>
    /// Simple tetris algorithm
    /// </summary>
    public class Tsitsiklis : IAlgorithm
    {
        private readonly float _height;
        private readonly float _holes;

        public Tsitsiklis(TsitsiklisWeights tsitsiklisWeights)
        {
            this._height = tsitsiklisWeights.Height;
            this._holes = tsitsiklisWeights.Holes;
        }

        public float CalculateFitness(byte[][] gameBoard, Pill previousBlock, int bacteriasCleared, int pillsCleared)
        {
            var fitness = 0f;
            var maxHeight = 0;
            for (var column = 0; column < gameBoard[0].Length; column++)
            {
                var reachedTopColumn = false;

                for (var row = gameBoard.GetLength(0) - 1; row >= 0; row--)
                {
                    var field = gameBoard[row][column];
                    if (reachedTopColumn && field == 0)
                    {
                        fitness += this._holes;
                    }

                    if (field != 0)
                    {
                        reachedTopColumn = true;

                        if (row + 1 > maxHeight)
                        {
                            maxHeight = row + 1;
                        }
                    }
                }
            }

            fitness += maxHeight * this._height;

            return fitness;
        }
    }
}
