using NUnit.Framework;
using System;
using Engine;

namespace Test
{
    [TestFixture()]
    public class PillTests
    {
        Pill _pill;

        [SetUp]
        public void Setup()
        {
            _pill = new Pill(Color.Blue, Color.Yellow, new Position(0, 0));
        }

        [Test()]
        public void TestCase()
        {
            var a = _pill.Matrix;
            _pill.Matrix.RotateLeft();

            Assert.AreEqual(a.ToString(), _pill.Matrix.ToString());
        }

        [Test()]
        public void TestCase3()
        {
            var a = _pill.Matrix;
            var b = _pill.Clone().Matrix;

            Assert.AreEqual(a.ToString(), b.ToString());
            Assert.AreNotEqual(a, b);
        }

        [Test()]
        public void TestCase2()
        {
            var a = _pill;
            var b = _pill.Clone();
            a.Matrix.RotateLeft();

            Assert.AreNotEqual(a.Matrix.ToString(), b.Matrix.ToString());
        }
    }
}