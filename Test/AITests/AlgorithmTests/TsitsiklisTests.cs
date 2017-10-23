using AI.Algorithms;
using AI.Algorithms.Weights;
using Engine.Extensions;
using NUnit.Framework;

namespace Test.AITests.AlgorithmTests
{
            [TestFixture]
    public class TsitsiklisTests : TestBase
    {
        [Test]
        [TestCase("0000000000000000", 0)]
        [TestCase(@"0000
                    0000
                    0000
                    0001", 1)]
        [TestCase(@"0000
                    0000
                    0001
                    0001", 2)]
        [TestCase(@"0000
                    0001
                    0001
                    0001", 3)]
        [TestCase(@"0001
                    0001
                    0001
                    0001", 4)]
        [TestCase(@"0001
                    0001
                    0001
                    0001", 4)]
        [TestCase(@"0000
                    0000
                    0001
                    0000", 5)]
        [TestCase(@"0001
                    0001
                    0001
                    0000", 7)]
        [TestCase(@"0000
                    0000
                    1001
                    0000", 8)]
        public void CalculateFitnessTest(string input, int expeceted)
        {
            var matrix = input.StringToByteMatrix(4);
            var weights = new TsitsiklisWeights(1, 3);
            var algorithm = new Tsitsiklis(weights);

            Assert.AreEqual(expeceted, algorithm.CalculateFitness(matrix , null, 0, 0));
        }
    }
}
