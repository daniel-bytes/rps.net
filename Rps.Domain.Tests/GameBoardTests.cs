using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Model;
using System.Collections.Generic;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void GameBoard_Get()
        {
            var rows = 8;
            var cols = 8;
            var props = new GameProperties(new Bounds(rows, cols), 2, 2);
            var tokens = new[] { new Token(1, "player1", TokenType.Flag, 1, 2), new Token(1, "player2", TokenType.Flag, 3, 4) };
            var gameBoard = new GameBoard(props, tokens);

            Assert.AreEqual(rows, gameBoard.NumRows, "NumRows should be correct");
            Assert.AreEqual(cols, gameBoard.NumCols, "NumCols should be correct");
            Assert.AreEqual(tokens[0], gameBoard.Get(1, 2), "1,2 should return the first token");
            Assert.AreEqual(tokens[1], gameBoard.Get(3, 4), "3,4 should return the second token");
            Assert.IsNull(gameBoard.Get(1, 1), "1,1 should return null");
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.InvalidPointException))]
        public void GameBoard_Get_InvalidMove_Negative()
        {
            var rows = 8;
            var cols = 8;
            var props = new GameProperties(new Bounds(rows, cols), 2, 2);
            var tokens = new[] { new Token(1, "player1", TokenType.Flag, 1, 2), new Token(1, "player2", TokenType.Flag, 3, 4) };
            var gameBoard = new GameBoard(props, tokens);
            gameBoard.Get(-1, -1); // throws
        }

        [TestMethod]
        [ExpectedException(typeof(Domain.Exceptions.InvalidPointException))]
        public void GameBoard_Get_InvalidMove_Positive()
        {
            var rows = 8;
            var cols = 8;
            var props = new GameProperties(new Bounds(rows, cols), 2, 2);
            var tokens = new[] { new Token(1, "player1", TokenType.Flag, 1, 2), new Token(1, "player2", TokenType.Flag, 3, 4) };
            var gameBoard = new GameBoard(props, tokens);
            gameBoard.Get(100, 100); // throws
        }

        [TestMethod]
        public void GameBoard_GetTokens()
        {
            int rows = 6;
            int cols = 6;
            var props = new GameProperties(new Bounds(rows, cols), 2, 2);
            var tokens = new[] { new Token(1, "player1", TokenType.Flag, 1, 2), new Token(1, "player2", TokenType.Flag, 3, 4) };
            var gameBoard = new GameBoard(props, tokens);

            CollectionAssert.AreEquivalent(tokens.OrderBy(x => x.Row).ThenBy(x => x.Col).ToArray(), 
                                          gameBoard.GetTokens().OrderBy(x => x.Row).ThenBy(x => x.Col).ToArray(), 
                                          "GetTokens should return an equivelant collection to input collection");
        }

        [TestMethod]
        public void GameBoard_New()
        {
            int rows = 6;
            int cols = 6;
            int rowsPerPlayer = 2;
            int bombsPerPlayer = 2;
            string player1 = "1";
            string player2 = "1";

            var board = GameBoard.New(
                            properties: new GameProperties(new Bounds(rows, cols), rowsPerPlayer, bombsPerPlayer),            
                            player1: new Player("1", "player1", false), 
                            player2: new Player("2", "player2", true));

            var tokens = board.GetTokens().ToList();
            Assert.AreEqual(rowsPerPlayer * cols * 2, tokens.Count, "Total token count should be correct");

            foreach (var playerID in new[] { player1, player2 })
            {
                var playerTokens = tokens.Where(x => x.PlayerID == playerID).ToList();
                Assert.AreEqual(rowsPerPlayer * cols, playerTokens.Count, "Player tokens count should be correct for " + playerID);

                var flagTokens = playerTokens.Where(x => x.TokenType == TokenType.Flag).ToList();
                Assert.AreEqual(1, flagTokens.Count, "Bomb tokens count should be correct for " + playerID);

                var bombTokens = playerTokens.Where(x => x.TokenType == TokenType.Bomb).ToList();
                Assert.AreEqual(bombsPerPlayer, bombTokens.Count, "Bomb tokens count should be correct for " + playerID);
                
                var rockTokens = playerTokens.Where(x => x.TokenType == TokenType.Rock).ToList();
                Assert.AreEqual(3, rockTokens.Count, "Rock tokens count should be correct for " + playerID);

                var paperTokens = playerTokens.Where(x => x.TokenType == TokenType.Paper).ToList();
                Assert.AreEqual(3, paperTokens.Count, "Paper tokens count should be correct for " + playerID);

                var scissorTokens = playerTokens.Where(x => x.TokenType == TokenType.Scissor).ToList();
                Assert.AreEqual(3, scissorTokens.Count, "Scissor tokens count should be correct for " + playerID);
            }
        }

        [TestMethod]
        public void GameBoard_Test_GetValidMoves()
        {
            int rows = 3;
            int cols = 3;
            int rowsPerPlayer = 1;
            int bombsPerPlayer = 1;

            var tokens = new List<Token>
                                {
                                    /* 
                                     * |_|F1|_|
                                     * |_|R1|_|
                                     * |_|F2|_|
                                     */
                                    new Token(1, "p1", TokenType.Flag, new Point(0, 1)),
                                    new Token(2, "p1", TokenType.Rock, new Point(1, 1)),
                                    new Token(3, "p2", TokenType.Flag, new Point(2, 1))
                                };

            var playerToken = tokens[1];

            var board = new GameBoard(
                            properties: new GameProperties(new Bounds(rows, cols), rowsPerPlayer, bombsPerPlayer),
                            tokens: tokens);

            var points = board.GetValidMoves(playerToken).OrderBy(x => x.X).ThenBy(x => x.Y).ToList();

            Assert.AreEqual(3, points.Count, "Number of moves should be correct");
            Assert.AreEqual(new Point(1, 0), points[0], "First point should be correct");
            Assert.AreEqual(new Point(1, 2), points[1], "Second point should be correct");
            Assert.AreEqual(new Point(2, 1), points[2], "Third point should be correct");
        }
    }
}
