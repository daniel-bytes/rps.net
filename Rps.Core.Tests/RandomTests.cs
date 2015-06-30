using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace Rps.Core.Tests
{
    [TestClass]
    public class RandomTests
    {
        [TestMethod]
        public void Random_New()
        {
            var random1 = Core.Utility.Random.New();
            Thread.Sleep(1);
            var random2 = Core.Utility.Random.New();

            Assert.AreNotEqual(random1.Next(), random2.Next(), "Initial random values from 2 different random generators should be different");
        }
    }
}
