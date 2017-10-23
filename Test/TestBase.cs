using System;
using System.Linq;
using Engine.Extensions;
using NUnit.Framework;

namespace Test
{
    public abstract class TestBase
    {
        protected void AssertBoard(byte[][] board, byte[][] exspected)
        {
            try
            {
                Assert.AreEqual(board.GetLength(0), exspected.GetLength(0), "rows");
                Assert.AreEqual(board[0].Length, exspected[0].Length, "columns");

                for (var row = 0; row < board.GetLength(0); row++)
                {
                    for (var column = 0; column < board[0].Length; column++)
                    {
                        Assert.That(board[row][column], Is.EqualTo(exspected[row][column]));
                    }
                }
            }
            catch (Exception)
            {
                this.PrintBoardDifferences(board, exspected);
                throw;
            }
        }

        [Obsolete("use string matrixes")]
        protected byte[][] ReverseRows(byte[][] gameBoard)
        {
            var clone = gameBoard.DeepClone();
            return clone.Reverse().ToArray();
        }

        protected void PrintBoardDifferences(byte[][] gameBoard, byte[][] expectedBoard)
        {
            Console.WriteLine(this.ReverseRows(gameBoard).MatrixToString(null));

            Console.WriteLine();

            Console.WriteLine(this.ReverseRows(expectedBoard).MatrixToString(null));
        }
    }
}