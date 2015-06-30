using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class BoundsTests
    {
        [TestMethod]
        public void Bounds_Test_Equals()
        {
            var p1 = new Bounds(1, 1);
            var p2 = new Bounds(1, 1);
            var p3 = new Bounds(1, 2);
            var p4 = new Bounds(2, 1);
            var p5 = new Bounds(3, 3);

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
    }
}
