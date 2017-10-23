using Engine.Extensions;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class StringExtensionTest
    {
        private static readonly byte[][] Expected =
            {new byte[] {1, 0, 1, 0}, new byte[] {1, 0, 1, 0}, new byte[] {1, 0, 1, 0}, new byte[] {1, 0, 1, 0}};

        [Test]
        [TestCase("/da1010101010101010")]
        [TestCase("1010101010101010")]
        [TestCase("10asdas10101010101010")]
        [TestCase(@"1
0101
01010101010")]
        public void StringByteMatrixTests(string input)
        {
            var result = input.StringToByteMatrix(4);

            Assert.AreEqual(Expected.Length, result.Length, "row length");
            Assert.AreEqual(Expected[0].Length, result[0].Length, "column length");

            for (var rowIndex = 0; rowIndex < input.StringToByteMatrix(4).Length; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < Expected[0].Length; columnIndex++)
                {
                    Assert.That(result[rowIndex][columnIndex], Is.EqualTo(Expected[rowIndex][columnIndex]));
                }
            }
        }
    }
}