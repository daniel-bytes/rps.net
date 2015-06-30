using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class TokenTests
    {
        [TestMethod]
        public void Token_Equals()
        {
            var token1 = new Token(1, "player1", TokenType.Rock, 2, 3);
            var token2 = new Token(1, "player1", TokenType.Rock, 2, 3);

            Assert.AreEqual(token1, token2);
        }

        [TestMethod]
        public void Token_NotEquals()
        {
            var token1 = new Token(1, "player1", TokenType.Rock, 2, 3);
            var token2 = new Token(2, "player1", TokenType.Rock, 2, 3);
            var token3 = new Token(1, "player2", TokenType.Rock, 2, 3);
            var token4 = new Token(1, "player1", TokenType.Paper, 2, 3);
            var token5 = new Token(1, "player1", TokenType.Rock, 23, 3);
            var token6 = new Token(1, "player1", TokenType.Rock, 2, 33);

            Assert.AreNotEqual(token1, token2, "Token1 and Token2 should not be equal");
            Assert.AreNotEqual(token1, token3, "Token1 and Token3 should not be equal");
            Assert.AreNotEqual(token1, token4, "Token1 and Token4 should not be equal");
            Assert.AreNotEqual(token1, token5, "Token1 and Token5 should not be equal");
            Assert.AreNotEqual(token1, token6, "Token1 and Token6 should not be equal");
        }
    }
}
