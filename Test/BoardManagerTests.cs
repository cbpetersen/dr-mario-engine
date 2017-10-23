// ReSharper disable InconsistentNaming

using System;
using System.Linq;
using Engine;
using Engine.Extensions;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class BoardManagerTests : TestBase
    {
        // 1 ~ Red
        [TestCase(0, 0, Color.Red, Color.Red, false, 5)]
        [TestCase(1, 0, Color.Red, Color.Red, false, 3)]
        [TestCase(2, 0, Color.Red, Color.Red, false, 3)]
        [TestCase(0, 1, Color.Red, Color.Red, false, 3)]
        [TestCase(0, 2, Color.Red, Color.Red, false, 3)]
        // 2 ~ Blue
        [TestCase(7, 0, Color.Blue, Color.Blue, false, 6)]
        [TestCase(7, 0, Color.Blue, Color.Red, false, 4)]
        [TestCase(6, 0, Color.Blue, Color.Blue, false, 4)]
        [TestCase(5, 0, Color.Blue, Color.Blue, false, 4)]
        [TestCase(4, 0, Color.Red, Color.Blue, false, 4)]
        [TestCase(7, 1, Color.Red, Color.Blue, false, 3)]
        [TestCase(7, 2, Color.Red, Color.Blue, false, 3)]
        [TestCase(7, 3, Color.Red, Color.Blue, false, 0)]
        // 3
        [TestCase(0, 6, 3, Color.Blue, false, 6)]
        [TestCase(0, 5, 3, Color.Blue, false, 4)]
        [TestCase(0, 4, 3, Color.Blue, false, 4)]
        [TestCase(0, 3, 3, Color.Blue, false, 4)]
        [TestCase(0, 2, 3, 3, false, 0)]
        [TestCase(1, 6, 3, Color.Blue, false, 3)]
        [TestCase(2, 6, 3, Color.Blue, false, 3)]
        [TestCase(3, 6, 3, Color.Blue, false, 0)]
        [TestCase(1, 5, 3, Color.Blue, false, 0)]
        // 4
        [TestCase(7, 6, 9, 4, false, 5)]
        [TestCase(7, 5, 9, 4, false, 3)]
        [TestCase(7, 4, 9, 4, false, 3)]
        [TestCase(7, 3, 9, 4, false, 0)]
        [TestCase(6, 6, 9, 4, false, 3)]
        [TestCase(5, 6, 9, 4, false, 3)]
        [TestCase(4, 6, 9, 4, false, 0)]
        [TestCase(6, 5, 9, 4, false, 0)]
        public void CheckBoard(int column, int row, Color x1, Color x2, bool rotated, int expectedLength)
        {
            var position = new Position(column, row);
            var pill = new Pill(x1, x2, position);
            if (rotated)
            {
                pill.Move(Move.RotateLeft);
            }

            var board = @"333030444
                          300030004
                          300035004
                          300055500
                          100035002
                          100030002
                          111032222".StringToByteMatrix(7);

            var boardManager = new BoardManager(board);
            var result = boardManager.CheckBoard(pill);
            Assert.AreEqual(expectedLength, result.Count());
        }

        [TestCase(0, 6, 1, 3)]
        [TestCase(0, 5, 1, 1)]
        [TestCase(0, 4, 1, 1)]
        [TestCase(1, 6, 1, 3)]
        [TestCase(2, 6, 1, 3)]
        [TestCase(0, 3, 5, 2)]
        [TestCase(1, 3, 5, 2)]
        [TestCase(7, 6, 2, 4)]
        [TestCase(6, 6, 2, 4)]
        [TestCase(5, 6, 2, 4)]
        [TestCase(7, 5, 2, 1)]
        [TestCase(7, 4, 2, 1)]
        [TestCase(7, 0, 3, 5)]
        [TestCase(6, 0, 3, 5)]
        [TestCase(5, 0, 3, 5)]
        [TestCase(4, 0, 3, 5)]
        [TestCase(3, 0, 3, 5)]
        [TestCase(7, 1, 2, 0)]
        [TestCase(7, 2, 2, 0)]
        public void PositionsLeftWithSameType(int column, int row, int type, int expectedLength)
        {
            var position = new Position(column, row);
            var board = @"11102222
                          10000002
                          10000002
                          55000000
                          40000003
                          40000003
                          44433333".StringToByteMatrix(7);

            var boardManager = new BoardManager(board);
            var result = boardManager.PositionsOnSameRowWithSameType(position, type);
            Assert.AreEqual(expectedLength, result.Count);
        }

        [TestCase(0, 0, 1, 3)]
        [TestCase(0, 1, 1, 3)]
        [TestCase(1, 2, 1, 5)]
        [TestCase(1, 3, 1, 5)]
        [TestCase(1, 4, 1, 5)]
        [TestCase(2, 3, 1, 0)]
        [TestCase(3, 4, 1, 2)]
        [TestCase(4, 4, 1, 1)]
        public void PositionsBelowWithSameType(int column, int row, int type, int expectedLength)
        {
            var position = new Position(column, row);
            var board = @"11011
                          01010
                          11000
                          11020
                          11022".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            var result = boardManager.PositionsBelowWithSameType(position, type);
            Assert.AreEqual(expectedLength, result.Count);
        }

        [Test]
        public void Move_to_end_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                  0000000000
                  0000000000
                  0000000000
                  0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                  0000000000
                  0000000000
                  0000000000
                  0000120000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            for (var i = 0; i < board.GetLength(0); i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_two_right_then_to_end_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000001200".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.Right));
            Assert.IsTrue(boardManager.Move(Move.Right));
            for (var i = 0; i < board.GetLength(0); i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_two_left_then_to_end_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0012000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.Left));
            Assert.IsTrue(boardManager.Move(Move.Left));
            for (var i = 0; i < board.GetLength(0); i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_pass_end_is_auto_locked_and_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000120000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            for (var i = 0; i < board.GetLength(0) + 2; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
                if (boardManager.ActivePill == null)
                {
                    break;
                }
            }

            board = boardManager.GetBoard();
            Assert.AreEqual(0, boardManager.PreviousPill.Position.Row);
            this.AssertBoard(board, exspected);
        }
        
        [Test]
        public void Move_pass_end_when_the_field_below_x1_is_blocked_is_auto_locked_and_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000100000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000120000
                0000100000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            for (var i = 0; i < board.GetLength(0) + 2; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
                if (boardManager.ActivePill == null)
                {
                    break;
                }
            }

            board = boardManager.GetBoard();
            this.AssertBoard(board, exspected);
            Assert.AreEqual(1, boardManager.PreviousPill.Position.Row);
        }
        
        [Test]
        public void Move_pass_end_when_the_field_below_x2_is_blocked_is_auto_locked_and_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000010000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000120000
                0000010000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            for (var i = 0; i < board.GetLength(0) + 2; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
                if (boardManager.ActivePill == null)
                {
                    break;
                }
            }

            board = boardManager.GetBoard();
            this.AssertBoard(board, exspected);
            Assert.AreEqual(1, boardManager.PreviousPill.Position.Row);
        }

        [Test]
        public void Move_pass_end_when_the_field_below_y1_is_blocked_is_auto_locked_and_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000100000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000200000
                0000400000
                0000100000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Yellow, Color.Blue);
            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            for (var i = 0; i < board.GetLength(0) + 2; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
                if (boardManager.ActivePill == null)
                {
                    break;
                }
            }

            board = boardManager.GetBoard();
            this.AssertBoard(board, exspected);
            Assert.AreEqual(2, boardManager.PreviousPill.Position.Row);
        }
        
        [Test]
        public void Move_pass_end_when_rotated_once_is_auto_locked_and_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000200000
                0000100000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            for (var i = 0; i < board.GetLength(0) + 2; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
                if (boardManager.ActivePill == null)
                {
                    break;
                }
            }

            board = boardManager.GetBoard();
            Assert.AreEqual(1, boardManager.PreviousPill.Position.Row);
            this.AssertBoard(board, exspected);
        }
        
        [Test]
        public void Move_to_end_with_one_rotation_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000200000
                0000100000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            for (var i = 0; i < board.GetLength(0) - 1; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_to_end_with_two_rotation_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000210000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            for (var i = 0; i < board.GetLength(0); i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_to_end_with_tree_rotations_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000000000
                0000100000
                0000200000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);

            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            Assert.IsTrue(boardManager.Move(Move.RotateLeft));
            Assert.IsTrue(boardManager.Move(Move.RotateLeft));

            for (var i = 0; i < board.GetLength(0) - 1; i++)
            {
                Assert.IsTrue(boardManager.Move(Move.Down));
            }

            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_one_down_and_lock_is_placed_correctly()
        {
            var board = @"0000000000
                          0000000000
                          0000000000
                          0000000000
                          0000000000".StringToByteMatrix(5);
            var exspected = @"0000000000
                              0000120000
                              0000000000
                              0000000000
                              0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            Console.WriteLine(boardManager.ActivePill.Matrix);
            Console.WriteLine(boardManager.Move(Move.Down));
            Console.WriteLine(boardManager.ActivePill.Matrix);
            boardManager.Lockblock();
            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_two_down_and_lock_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000000000
                0000000000
                0000120000
                0000000000
                0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            Console.WriteLine(boardManager.ActivePill.Matrix);
            Console.WriteLine(boardManager.Move(Move.Down));
            Console.WriteLine(boardManager.Move(Move.Down));
            Console.WriteLine(boardManager.ActivePill.Matrix);
            boardManager.Lockblock();
            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void Move_one_down_and_lock_when_a_block_is_already_there()
        {
            var board =
                @"0000000000
                0000330000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000120000
                0000330000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            Console.WriteLine(boardManager.ActivePill.Matrix);
            Console.WriteLine(boardManager.Move(Move.Down));
            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void SpawnBlock_rotated_left_twice_b_b_block_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000210000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            Console.WriteLine(boardManager.ActivePill.Matrix);

            Console.WriteLine(boardManager.Move(Move.RotateLeft));
            Console.WriteLine(boardManager.Move(Move.RotateLeft));
            Console.WriteLine(boardManager.ActivePill.Matrix);
            boardManager.Lockblock();
            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void SpawnBlock_rotated_b_b_block_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var expected =
                @"0000200000
                0000100000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            Console.WriteLine(boardManager.ActivePill.Matrix);
            Console.WriteLine(boardManager.Move(Move.RotateLeft));
            Console.WriteLine(boardManager.ActivePill.Matrix);
            boardManager.Lockblock();
            board = boardManager.GetBoard();

            this.AssertBoard(board, expected);
        }

        [Test]
        public void SpawnBlock_z_block_is_placed_correctly()
        {
            var board =
                @"0000000000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);
            var exspected =
                @"0000120000
                0000000000
                0000000000
                0000000000
                0000000000".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Red, Color.Blue);
            boardManager.Lockblock();
            board = boardManager.GetBoard();

            this.AssertBoard(board, exspected);
        }

        [Test]
        public void SpawnBlock_cant_spawn_top_row()
        {
            var board =
                @"0000100000
                1111111111
                1111111111
                1111111111
                1111111111".StringToByteMatrix(5);
            var exspected =
                @"0000220000
                1111111111
                1111111111
                1111111111
                1111111111".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            Assert.Throws<InvalidOperationException>(() => boardManager.SpawnPill(Color.Blue, Color.Blue));
        }

        [Test]
        public void CheckPill_can_spawn_top_row()
        {
            var board =
                @"0000000000
                1111111111
                1111111111
                1111111111
                1111111111".StringToByteMatrix(5);
            var exspected =
                @"0000220000
                1111111111
                1111111111
                1111111111
                1111111111".StringToByteMatrix(5);

            var boardManager = new BoardManager(board);
            boardManager.SpawnPill(Color.Blue, Color.Blue);
            boardManager.Lockblock();
            board = boardManager.GetBoard();
            this.AssertBoard(board, exspected);
        }

//        [TestCase(0, 0, 9, 4, false, @"00000
//                                       00000
//                                       00000
//                                       00000
//                                       00000")]
//        public void Lockblock(int column, int row, PillType x1, PillType x2, bool rotated, int expectedBoard)
//        {
//            var position = new Position(column, row);
//            var pill = new Pill(x1, x2, position);
//            if (rotated)
//            {
//                pill.Move(Move.RotateLeft);
//            }
//
//            var board = @"00000
//                          00000
//                          00000
//                          00000
//                          00000".StringToByteMatrix(5);
//
//            var boardManager = new BoardManager(board);
//            boardManager.SpawnPill(x1, x2);
//            
//               
//            boardManager.Lockblock();
//            Assert.AreEqual(expectedLength, result.Count());
//        }
    }
}