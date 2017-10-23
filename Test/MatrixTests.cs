namespace Test
{
    using NUnit.Framework;
    using Engine;

    [TestFixture()]
    public class MatrixTests
    {
        [Test()]
        public void MatrixSetDefaults()
        {
            var matrix = new Matrix(1, 2);

            Assert.AreEqual(1, matrix.X1);
            Assert.AreEqual(2, matrix.X2);
            Assert.AreEqual(0, matrix.Y1);
            Assert.AreEqual(0, matrix.Y2);
        }

        [Test()]
        public void MatrixRotatedLeftOnce()
        {
            var matrix = new Matrix(1, 2);
            matrix.RotateLeft();

            Assert.AreEqual(2, matrix.X1);
            Assert.AreEqual(0, matrix.X2);
            Assert.AreEqual(1, matrix.Y1);
            Assert.AreEqual(0, matrix.Y2);
        }

        [Test()]
        public void MatrixRotatedLeftTwice()
        {
            var matrix = new Matrix(1, 2);
            matrix.RotateLeft();
            matrix.RotateLeft();

            Assert.AreEqual(2, matrix.X1);
            Assert.AreEqual(1, matrix.X2);
            Assert.AreEqual(0, matrix.Y1);
            Assert.AreEqual(0, matrix.Y2);
        }

        [Test()]
        public void MatrixRotatedLeftThreeTimes()
        {
            var matrix = new Matrix(1, 2);
            matrix.RotateLeft();
            matrix.RotateLeft();
            matrix.RotateLeft();

            Assert.AreEqual(1, matrix.X1);
            Assert.AreEqual(0, matrix.X2);
            Assert.AreEqual(2, matrix.Y1);
            Assert.AreEqual(0, matrix.Y2);
        }
    }
}