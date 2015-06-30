using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rps.Domain.Repository.Entity;
using Moq;
using Rps.Domain.Repository;
using System.Collections.Generic;
using Rps.Domain.Tests.Helper;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Rps.Domain.Tests
{
    [TestClass]
    public class GameRepositoryTests
    {
        [TestMethod]
        public void GameRepository_Get()
        {
            var context = new Mock<IGameContext>();
            var gameSet = new Mock<IDbSet<Game>>();
            var tokenSet = new Mock<IDbSet<Token>>();

            var games = new List<Game>
                   {
                       new Repository.Entity.Game { ID = 1, Player1ID = "player1", Player2ID = "player2", NumRows = 1, NumCols = 1 }
                   };

            var tokens = new List<Token>
                   {
                       new Repository.Entity.Token { GameID = 1, ID = 100 },
                       new Repository.Entity.Token { GameID = 2, ID = 200 }
                   };

            gameSet.ConfigureDbSet(games);
            tokenSet.ConfigureDbSet(tokens);

            context.SetupGet(x => x.Games).Returns(gameSet.Object);
            context.SetupGet(x => x.Tokens).Returns(tokenSet.Object);

            var repo = new GameRepository(context.Object);

            // Test
            var resultGame = repo.GetAsync(1).Result;

            Assert.IsNotNull(resultGame, "Game should not be null");
            Assert.AreEqual(1, resultGame.ID, "Game id should be correct");
            Assert.IsNotNull(resultGame.GameBoard, "GameBoard should not be null");

            var resultTokens = resultGame.GameBoard.GetTokens().ToList();
            Assert.IsNotNull(resultTokens, "Tokens should not be null");
            Assert.AreEqual(1, resultTokens.Count, "There should be 1 token");
        }


        [TestMethod]
        public void GameRepository_Create()
        {
            var context = new Mock<IGameContext>();
            var gameSet = new Mock<IDbSet<Game>>();
            var tokenSet = new Mock<IDbSet<Token>>();

            gameSet.ConfigureDbSet(new List<Game>());
            tokenSet.ConfigureDbSet(new List<Token>());

            context.SetupGet(x => x.Games).Returns(gameSet.Object);
            context.SetupGet(x => x.Tokens).Returns(tokenSet.Object);

            gameSet.Setup(x => x.Add(It.IsAny<Game>()))
                   .Callback<Game>(g => g.ID = 1);

            var repo = new GameRepository(context.Object);
            var game = new Model.Game(
                            0,
                            new Model.Player("1", "player1", false),
                            new Model.Player("2", "player2", true),
                            false,
                            new Model.GameBoard(new Model.GameProperties(), Enumerable.Empty<Model.Token>()));

            var result = repo.CreateAsync(game).Result;

            gameSet.Verify(x => x.Add(It.IsAny<Game>()));
            context.Verify(x => x.SaveChangesAsync());

            Assert.AreNotEqual(result, game, "Result should be a new Game");
            Assert.AreEqual(1, result.ID, "ID should be set");
        }


        [TestMethod]
        public void GameRepository_Save()
        {
            var context = new Mock<IGameContext>();
            var gameSet = new Mock<IDbSet<Game>>();
            var tokenSet = new Mock<IDbSet<Token>>();

            var games = new List<Game>
                   {
                       new Repository.Entity.Game { ID = 1 }
                   };

            var tokens = new List<Token>
                   {
                       new Repository.Entity.Token { GameID = 1, ID = 100 },
                       new Repository.Entity.Token { GameID = 1, ID = 200 }
                   };

            gameSet.ConfigureDbSet(games);
            tokenSet.ConfigureDbSet(tokens);

            context.SetupGet(x => x.Games).Returns(gameSet.Object);
            context.SetupGet(x => x.Tokens).Returns(tokenSet.Object);

            gameSet.Setup(x => x.Add(It.IsAny<Game>()))
                   .Callback<Game>(g => g.ID = 1);

            var repo = new GameRepository(context.Object);
            var game = new Model.Game(
                            1,
                            new Model.Player("1", "player1", false),
                            new Model.Player("2", "player2", true),
                            true,
                            new Model.GameBoard(new Model.GameProperties(new Model.Bounds(4, 4), 1, 1), new List<Model.Token>
                            {
                                new Model.Token(100, "player1", Model.TokenType.Bomb, 1, 2),
                                new Model.Token(101, "player1", Model.TokenType.Rock, 0, 3)
                            }));

            repo.SaveAsync(game).Wait();
            
            context.Verify(x => x.SaveChangesAsync());
            tokenSet.Verify(x => x.Add(It.Is<Token>(t => t.ID == 101)));
            tokenSet.Verify(x => x.Remove(It.Is<Token>(t => t.ID == 200)));

            Assert.AreEqual("1", games[0].Player1ID, "player1 ID should be set");
            Assert.AreEqual("player1", games[0].Player1Name, "player1 Name should be set");
            Assert.AreEqual("2", games[0].Player2ID, "player2 ID should be set");
            Assert.AreEqual("player2", games[0].Player2Name, "player2 Name should be set");
            Assert.AreEqual(game.GameBoard.NumRows, games[0].NumRows, "NumRows should be set");
            Assert.AreEqual(game.GameBoard.NumCols, games[0].NumCols, "NumCols should be set");
            Assert.AreEqual(game.Active, games[0].Active, "GameOver should be set");

            Assert.AreEqual(1, tokens[0].Row, "Row should be set");
            Assert.AreEqual(2, tokens[0].Col, "Col should be set");
            Assert.AreEqual("player1", tokens[0].PlayerID, "PlayerID should be set");
            Assert.IsTrue(tokens[0].UpdatedAtUtc > DateTime.MinValue, "UpdatedAtUtc should be set");
        }
    }
}
