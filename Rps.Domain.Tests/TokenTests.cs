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


        [TestMethod]
        public void Token_Test_Attack()
        {
            var attackerRock = new Token(1, "1", TokenType.Rock, new Point(1, 1));
            var attackerPaper = new Token(1, "1", TokenType.Paper, new Point(1, 1));
            var attackerScissor = new Token(1, "1", TokenType.Scissor, new Point(1, 1));

            var defenderRock = new Token(2, "2", TokenType.Rock, new Point(0, 1));
            var defenderPaper = new Token(2, "2", TokenType.Paper, new Point(0, 1));
            var defenderScissor = new Token(2, "2", TokenType.Scissor, new Point(0, 1));
            var defenderEmpty = new Token(2, "2", TokenType.Empty, new Point(0, 1));
            var defenderBomb = new Token(2, "2", TokenType.Bomb, new Point(0, 1));
            var defenderFlag = new Token(2, "2", TokenType.Flag, new Point(0, 1));

            // Rock Attacks
            var result = attackerRock.Attack(defenderRock);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Rock vs Rock should have correct result");

            result = attackerRock.Attack(defenderPaper);
            Assert.AreEqual(GameMoveResultType.DefenderWins, result, "Rock vs Paper should have correct result");

            result = attackerRock.Attack(defenderScissor);
            Assert.AreEqual(GameMoveResultType.AttackerWins, result, "Rock vs Scissor should have correct result");

            result = attackerRock.Attack(defenderEmpty);
            Assert.AreEqual(GameMoveResultType.TokenMove, result, "Rock vs Empty should have correct result");

            result = attackerRock.Attack(defenderBomb);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Rock vs Bomb should have correct result");

            result = attackerRock.Attack(defenderFlag);
            Assert.AreEqual(GameMoveResultType.FlagCapturedByAttacker, result, "Rock vs Flag should have correct result");

            // Paper attacks
            result = attackerPaper.Attack(defenderRock);
            Assert.AreEqual(GameMoveResultType.AttackerWins, result, "Paper vs Rock should have correct result");

            result = attackerPaper.Attack(defenderPaper);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Paper vs Paper should have correct result");

            result = attackerPaper.Attack(defenderScissor);
            Assert.AreEqual(GameMoveResultType.DefenderWins, result, "Paper vs Scissor should have correct result");

            result = attackerPaper.Attack(defenderEmpty);
            Assert.AreEqual(GameMoveResultType.TokenMove, result, "Paper vs Empty should have correct result");

            result = attackerPaper.Attack(defenderBomb);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Paper vs Bomb should have correct result");

            result = attackerPaper.Attack(defenderFlag);
            Assert.AreEqual(GameMoveResultType.FlagCapturedByAttacker, result, "Paper vs Flag should have correct result");

            // Scissor attacks
            result = attackerScissor.Attack(defenderRock);
            Assert.AreEqual(GameMoveResultType.DefenderWins, result, "Scissor vs Rock should have correct result");

            result = attackerScissor.Attack(defenderPaper);
            Assert.AreEqual(GameMoveResultType.AttackerWins, result, "Scissor vs Paper should have correct result");

            result = attackerScissor.Attack(defenderScissor);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Scissor vs Scissor should have correct result");

            result = attackerScissor.Attack(defenderEmpty);
            Assert.AreEqual(GameMoveResultType.TokenMove, result, "Scissor vs Empty should have correct result");

            result = attackerScissor.Attack(defenderBomb);
            Assert.AreEqual(GameMoveResultType.BothLose, result, "Scissor vs Bomb should have correct result");

            result = attackerScissor.Attack(defenderFlag);
            Assert.AreEqual(GameMoveResultType.FlagCapturedByAttacker, result, "Scissor vs Flag should have correct result");
        }
    }
}
