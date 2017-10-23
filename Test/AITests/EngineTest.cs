using AI;
using AI.Algorithms;
using AI.Algorithms.Weights;
using Engine;
using Engine.Extensions;
using NUnit.Framework;

namespace Test.AITests
{
    [TestFixture]
    public class EngineTest : TestBase
    {
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000", Color.Red, Color.Red, 1)]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000", Color.Blue, Color.Red, 1)]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000001100000
                    0000000000110000", Color.Blue, Color.Yellow, 5)]
        [TestCase(@"0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    0000000000000000
                    1111111111110000", Color.Blue, Color.Blue, 1)]
        public void GetBestMoveTest(string input, Color color1, Color color2, int expeceted)
        {
            var matrix = input.StringToByteMatrix(10);
            var weights = new TsitsiklisWeights(1, 3);
            var algorithm = new Tsitsiklis(weights);
            var manager = new BoardManager(matrix);
            manager.SpawnPill(color1, color2);
            var engine = new AiEngine(algorithm);
            var bestMove = engine.GetNextMove(manager);

            Assert.AreEqual(expeceted, bestMove.Fitness);
        }
    }
}
