using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class PointTests
    {
        [TestMethod]
        public void Point_Test_Equals()
        {
            var p1 = new Point(1, 1);
            var p2 = new Point(1, 1);
            var p3 = new Point(1, 2);
            var p4 = new Point(2, 1);
            var p5 = new Point(3, 3);

            Assert.AreEqual(p1, p2, "p1 and p2 should be equal");
            Assert.AreNotEqual(p1, p3, "p1 and p3 should not be equal");
            Assert.AreNotEqual(p1, p4, "p1 and p4 should not be equal");
            Assert.AreNotEqual(p1, p5, "p1 and p5 should not be equal");

            Assert.IsTrue(p1 == p2, "p1 and p2 should be equal via equals operator");
            Assert.IsFalse(p1 == p3, "p1 and p3 should not be equal via equals operator");
            Assert.IsFalse(p1 == p4, "p1 and p4 should not be equal via equals operator");
            Assert.IsFalse(p1 == p5, "p1 and p5 should not be equal via equals operator");

            Assert.IsFalse(p1 != p2, "p1 and p2 not equals operator should be false");
            Assert.IsTrue(p1 != p3, "p1 and p3 not equals operator should be true");
            Assert.IsTrue(p1 != p4, "p1 and p4 not equals operator should be true");
            Assert.IsTrue(p1 != p5, "p1 and p5 not equals operator should be true");
        }

        [TestMethod]
        public void Point_Test_Add()
        {
            var pt1 = new Point(1, 2);
            var pt2 = new Point(3, 4);
            var pt3 = pt1 + pt2;

            Assert.AreEqual(4, pt3.X, "pt3 X should be correct");
            Assert.AreEqual(6, pt3.Y, "pt3 Y should be correct");
        }
    }
}
