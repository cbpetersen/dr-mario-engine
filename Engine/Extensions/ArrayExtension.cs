namespace Engine.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public static class ArrayExtension
    {
        public static byte[][] Split(this byte[] array, int size)
        {
            if (array.Length % size != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(array));
            }

            var matrix = new List<byte[]>();
            var length = array.Length / size;

            for (var i = 0; i < size; i++)
            {
                matrix.Add(array.Skip(i * length).Take(length).ToArray());
            }

            return matrix.ToArray();
        }

        public static bool[][] Split(this bool[] array, int size)
        {
            if (array.Length % size != 0)
            {
                throw new ArgumentOutOfRangeException(nameof(array));
            }

            var matrix = new List<bool[]>();
            var length = array.Length / size;

            for (var i = 0; i < size; i++)
            {
                matrix.Add(array.Skip(i * length).Take(length).ToArray());
            }

            return matrix.ToArray();
        }

        public static byte[][] DeepClone(this byte[][] array)
        {
            var list = new List<byte[]>();
            for (var i = 0; i < array.GetLength(0); i++)
            {
                list.Add(array[i].Cast<byte>().ToArray());
            }

            return list.ToArray();
        }

        public static string MatrixToString(this byte[][] gameBoard, Pill active)
        {
            var fieldChars = new StringBuilder();
            for (var row = gameBoard.GetLength(0) - 1; row >= 0; row--)
            {
                fieldChars.Append("|");
                for (var column = 0; column < gameBoard[row].Length; column++)
                {
                    if (active != null && row == active.Position.Row && column == active.Position.Column)
                    {
                        fieldChars.Append($"{active.Matrix.X1}");
                    }
                    else if (active != null && row == active.Position.Row && column == active.Position.Column + 1 &&
                             !active.Rotated)
                    {
                        fieldChars.Append($"{active.Matrix.X2}");
                    }
                    else if (active != null && row == active.Position.Row - 1 && column == active.Position.Column &&
                             active.Rotated)
                    {
                        fieldChars.Append($"{active.Matrix.Y1}");
                    }
                    else
                    {
                        fieldChars.Append(gameBoard[row][column] != 0 ? $"{gameBoard[row][column]}" : " ");
                    }
                    if (column + 1 < gameBoard[row].Length)
                    {
                        fieldChars.Append(" ");
                    }
                }

                fieldChars.Append("|");
                fieldChars.AppendLine();
            }

            return fieldChars.ToString();
        }
    }
}