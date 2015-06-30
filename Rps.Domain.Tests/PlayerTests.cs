using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;

namespace Rps.Domain.Tests
{
    /// <summary>
    /// Summary description for PlayerTests
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void Player_Equals()
        {
            var player1 = new Player("1", "player1", true);
            var player2 = new Player("1", "player1", true);

            Assert.AreEqual(player1, player2);
        }

        [TestMethod]
        public void Player_NotEquals()
        {
            var player1 = new Player("1", "player1", true);
            var player2 = new Player("2", "player2", true);
            var player3 = new Player("1", "player1", false);

            Assert.AreNotEqual(player1, player2, "Player1 and Player2 should not be equal");
            Assert.AreNotEqual(player1, player3, "Player1 and Player3 should not be equal");
        }
    }
}
