using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rps.Domain.Repository.Entity;

namespace Rps.Domain.Repository
{
    public class GameRepository
        : IGameRepository
    {
        private readonly IGameContext context;

        public GameRepository(IGameContext context)
        {
            this.context = context;
        }

        public async Task<Model.Game> GetAsync(long id)
        {
            // Fetch data
            var entity = await context.Games.AsNoTracking().SingleAsync(x => x.ID == id);
            entity.Tokens = await context.Tokens.AsNoTracking().Where(x => x.GameID == id).ToListAsync();

            var player1 = new Model.Player(entity.Player1ID, entity.Player1Name, false);
            var player2 = new Model.Player(entity.Player2ID, entity.Player2Name, entity.SinglePlayerMode);

            var bounds = new Model.Bounds(entity.NumRows, entity.NumCols);
            var properties = new Model.GameProperties(bounds, entity.RowsPerPlayer, entity.BombsPerPlayer);
            var tokens = entity.Tokens.Select(x => new Model.Token(x.ID, x.PlayerID, (Model.TokenType)x.TokenType, x.Row, x.Col));
            var gameBoard = new Model.GameBoard(properties, tokens);

            var game = new Model.Game(entity.ID, player1, player2, entity.Active, gameBoard);

            return game;
        }

        public async Task<IEnumerable<Model.Game>> GetPlayerActiveGamesAsync(string playerID)
        {
            var results = new Dictionary<string, Game>();

            var query = from game in context.Games
                        where (game.Player1ID == playerID || game.Player2ID == playerID)
                           && game.Active
                        orderby game.UpdatedAtUtc descending
                        select game;

            var games = await query.ToListAsync();

            return (from result in games
                    select new Model.Game(result.ID,
                                          new Model.Player(result.Player1ID, result.Player1Name, false),
                                          new Model.Player(result.Player2ID, result.Player2Name, result.SinglePlayerMode),
                                          result.Active,
                                          Model.GameBoard.Empty())).ToList();
        }

        public async Task<Model.Game> CreateAsync(Model.Game game)
        {
            var timestamp = DateTime.UtcNow;

            var model = new Game
            {
                Player1ID = game.Player1.ID,
                Player1Name = game.Player1.Name,
                Player2ID = game.Player2.ID,
                Player2Name = game.Player2.Name,
                SinglePlayerMode = game.Player2.IsComputerControlled,
                CreatedAtUtc = timestamp,
                UpdatedAtUtc = timestamp,
                NumRows = game.GameBoard.NumRows,
                NumCols = game.GameBoard.NumCols,
                Active = true,
                Tokens = (from token in game.GameBoard.GetTokens()
                            select new Token
                            {
                                ID = token.ID,
                                PlayerID = token.PlayerID,
                                Row = token.Row,
                                Col = token.Col,
                                TokenType = (int)token.TokenType,
                                CreatedAtUtc = timestamp,
                                UpdatedAtUtc = timestamp
                            }).ToList()
            };

            context.Games.Add(model);

            await context.SaveChangesAsync();

            return new Model.Game(model.ID, game.Player1, game.Player2, game.Active, game.GameBoard);
        }

        public async Task SaveAsync(Model.Game game)
        {
            var timestamp = DateTime.UtcNow;

            var entity = await context.Games.SingleAsync(x => x.ID == game.ID);
            var tokens = await context.Tokens.Where(x => x.GameID == game.ID).ToListAsync();

            entity.Player1ID = game.Player1.ID;
            entity.Player1Name = game.Player1.Name;
            entity.Player2ID = game.Player2.ID;
            entity.Player2Name = game.Player2.Name;
            entity.Active = game.Active;
            entity.NumRows = game.GameBoard.NumRows;
            entity.NumCols = game.GameBoard.NumCols;
            entity.SinglePlayerMode = game.Player2.IsComputerControlled;
            entity.UpdatedAtUtc = timestamp;

            var currentTokens = (from token in game.GameBoard.GetTokens()
                                 select new Token
                                 {
                                     ID = token.ID,
                                     GameID = game.ID,
                                     PlayerID = token.PlayerID,
                                     Row = token.Row,
                                     Col = token.Col,
                                     TokenType = (int)token.TokenType
                                 }).ToList();

            foreach (var currentToken in currentTokens)
            {
                var existingToken = tokens.SingleOrDefault(x => x.ID == currentToken.ID);

                if (existingToken != null)
                {
                    existingToken.Row = currentToken.Row;
                    existingToken.Col = currentToken.Col;
                    existingToken.PlayerID = currentToken.PlayerID;
                    existingToken.TokenType = currentToken.TokenType;
                    existingToken.UpdatedAtUtc = timestamp;
                }
                else
                {
                    context.Tokens.Add(currentToken);
                }
            }

            foreach (var existingToken in tokens)
            {
                if (!currentTokens.Any(x => x.ID == existingToken.ID))
                {
                    context.Tokens.Remove(existingToken);
                }
            }
            
            await context.SaveChangesAsync();
        }
    }
}
