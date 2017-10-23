using Engine;

namespace AI.Algorithms
{
    public class Features
    {
        private readonly byte[][] _gameBoard;

        public Features(byte[][] gameBoard)
        {
            this._gameBoard = gameBoard;
        }

        /// <summary>
        /// The total number of row transitions.
        /// A row transition occurs when an cell of type x is adjacent to another cell of a different type
        /// on the same row and vice versa.
        /// </summary>
        /// <returns></returns>
        public int RowTransitions()
        {
            var transitions = 0;
            for (var column = 0; column < this._gameBoard[0].Length; column++)
            {
                var prevValue = this._gameBoard[0][column];
                byte currentValue;
                for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
                {
                    currentValue = this._gameBoard[row][column];
                    if (currentValue != prevValue)
                    {
                        transitions++;
                        prevValue = currentValue;
                    }
                }
            }

            return transitions;
        }

        /// <summary>
        /// The total number of column transitions.
        /// A column transition occurs when a cell is adjacent to another type of cell
        /// on the same column and vice versa.
        /// </summary>
        /// <returns></returns>
        public int ColumnTransitions()
        {
            var transitions = 0;

            for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
            {
                var prevValue = this._gameBoard[row][0];
                byte currentValue;
                for (var column = 0; column < this._gameBoard[0].Length; column++)
                {
                    currentValue = this._gameBoard[row][column];
                    if (currentValue != prevValue)
                    {
                        transitions++;
                        prevValue = currentValue;
                    }
                }
            }

            return transitions;
        }

        /// <summary>
        /// Number of Holes.
        /// A hole is an empty cell that has at least one filled cell above it in the same column.
        /// </summary>
        /// <returns></returns>
        public int NumberOfHoles()
        {
            var holes = 0;

            for (var column = 0; column < this._gameBoard[0].Length; column++)
            {
                var reachedTopColumn = false;

                for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
                {
                    var field = this._gameBoard[row][column] != 0;
                    if (reachedTopColumn && !field)
                    {
                        holes++;
                    }

                    if (field)
                    {
                        reachedTopColumn = true;
                    }
                }
            }

            return holes;
        }

        /// <summary>
        /// Well sums
        /// A well is a sequence of empty cells above the top piece in a column such
        /// that the top cell in the sequence is surrounded (left and right) by occupied
        /// cells or a boundary of the board.
        ///
        ///
        /// Return:
        ///    The well sums. For a well of length n, we define the well sums as
        ///    1 + 2 + 3 + ... + n. This gives more significance to deeper holes.
        /// </summary>
        /// <returns></returns>
        public int WellSums()
        {
            var wellSum = 0;
            for (var column = 1; column < this._gameBoard[0].Length - 1; column++)
            {
                for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
                {
                    if (_gameBoard[row][column] != 0)
                    {
                        break;
                    }

                    if (this._gameBoard[row][column - 1] != 0 && this._gameBoard[row][column + 1] != 0)
                    {
                        wellSum++;
                    }
                }
            }

            // Check for wells most left columns
            for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
            {
                if (_gameBoard[row][0] != 0)
                {
                    break;
                }
                
                if (_gameBoard[row][1] != 0)
                {
                    wellSum++;
                }
            }

            // Check for wells most right columns
            for (var row = this._gameBoard.GetLength(0) - 1; row >= 0; row--)
            {
                if (_gameBoard[row][this._gameBoard[0].Length - 1] != 0)
                {
                    break;
                }
                
                if (_gameBoard[row][this._gameBoard[0].Length - 2] != 0)
                {
                    wellSum++;
                }
            }

            return wellSum;
        }

        /// <summary>
        /// Height of the last block placed
        /// </summary>
        /// <param name="pill"></param>
        /// <returns></returns>
        public int LandingHeight(Pill pill)
        {
            if (pill == null)
            {
                return 0;
            }

            return pill.Position.Row;
        }
    }
}
