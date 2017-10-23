using System;
using System.Linq;
using AI;
using AI.Algorithms;
using AI.Algorithms.Weights;
using Engine;
using Engine.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;
using Move = Engine.Move;

namespace Test.AITests
{
    [TestFixture]
    public class MoveTests : TestBase
    {
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    2000000000000000
                    2000000000000000", new[]
            {Move.RotateLeft, Move.RotateLeft, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1000000000000000
                    1000000000000000", new[]
            {Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0111111111111111", new[]
            {Move.RotateLeft, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1011111111111111", new[]
            {Move.RotateLeft, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1101111111111111", new[] {Move.RotateLeft, Move.Left, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1110111111111111", new[] {Move.RotateLeft, Move.Left, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111011111111111", new[] {Move.RotateLeft, Move.Left, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111101111111111", new[] {Move.RotateLeft, Move.Left, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111110111111111", new[] {Move.RotateLeft, Move.Left})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111011111111", new[] {Move.RotateLeft})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111111110", new[]
            {Move.RotateLeft, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111111101", new[]
            {Move.RotateLeft, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111111011", new[]
            {Move.RotateLeft, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111110111", new[] {Move.RotateLeft, Move.Right, Move.Right, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111101111", new[] {Move.RotateLeft, Move.Right, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111011111", new[] {Move.RotateLeft, Move.Right, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111110111111", new[] {Move.RotateLeft, Move.Right, Move.Right})]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111101111111", new[] {Move.RotateLeft, Move.Right})]
        public void MovesAreCorrect(string input, Move[] expeceted)
        {
            var matrix = input.StringToByteMatrix(8);
            var weights = new AiWeights()
            {
                PillsCleared = -100,
                WellSums = 1,
                NumberOfHoles = 10,
            };
            var algorithm = new FeatureAi(weights);
            var manager = new BoardManager(matrix);
            manager.SpawnPill(Color.Red, Color.Blue);
            var engine = new AiEngine(algorithm);

            var bestMove = engine.GetNextMove(manager);
            var moves = bestMove.Moves;

            Assert.IsTrue(bestMove.IsValid);

            Console.WriteLine(JsonConvert.SerializeObject(bestMove));
            foreach (var move in moves)
            {
                manager.Move(move);
                Console.WriteLine(manager.GameBoard.MatrixToString(manager.ActivePill));
            }
            while (manager.ActivePill != null)
            {
                manager.Move(Move.Down);
                Console.WriteLine(manager.GameBoard.MatrixToString(manager.ActivePill));
            }

            Assert.AreEqual(expeceted.Where(x => x == Move.Right).Count(), moves.Where(x => x == Move.Right).Count());
            Assert.AreEqual(expeceted.Where(x => x == Move.Left).Count(), moves.Where(x => x == Move.Left).Count());
            Assert.AreEqual(expeceted.Where(x => x == Move.RotateLeft).Count(),
                moves.Where(x => x == Move.RotateLeft).Count());
            Assert.AreEqual(expeceted.Where(x => x == Move.RotateRight).Count(),
                moves.Where(x => x == Move.RotateRight).Count());
            Assert.That(bestMove.Fitness, Is.AtMost(1), "Fitness");
        }
    }
}