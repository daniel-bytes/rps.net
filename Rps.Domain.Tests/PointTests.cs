using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class GameMoveTests
    {
        [TestMethod]
        public void GameMove_Test_Equals()
        {
            var m1 = new GameMove();
            var m2 = new GameMove();
            var m3 = new GameMove(null, new Point(1, 1));

            Assert.AreEqual(m1, m2, "m1 and m2 should be equal");
            Assert.AreNotEqual(m1, m3, "m1 and m3 should not be equal");

            Assert.IsTrue(m1 == m2, "m1 and m2 should be equal via equals operator");

            Assert.IsTrue(m1 != m3, "m1 and m3 not equals operator should be true");
        }
    }
}
