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

            var players = new[] { 
                                new Model.Player(entity.Player1ID, entity.Player1Name, false), 
                                new Model.Player(entity.Player2ID, entity.Player2Name, entity.SinglePlayerMode) 
                            };

            var bounds = new Model.Bounds(entity.NumRows, entity.NumCols);
            var properties = new Model.GameProperties(bounds, entity.RowsPerPlayer, entity.BombsPerPlayer);
            var status = Model.GameStatus.CreateFromEntity(entity, players);

            var tokens = entity.Tokens.Select(x => new Model.Token(x.ID, x.PlayerID, (Model.TokenType)x.TokenType, x.Row, x.Col));
            var gameBoard = new Model.GameBoard(properties, tokens);

            var game = new Model.Game(entity.ID, players[0], players[1], status, gameBoard);

            return game;
        }

        public async Task<IEnumerable<Model.Game>> GetPlayerActiveGamesAsync(string playerID)
        {
            var results = new Dictionary<string, Game>();

            var query = from game in context.Games.AsNoTracking()
                        where (game.Player1ID == playerID || game.Player2ID == playerID)
                           && game.Active
                        orderby game.UpdatedAtUtc descending
                        select game;

            var games = await query.ToListAsync();

            return (from entity in games
                    let players = new[] { 
                                new Model.Player(entity.Player1ID, entity.Player1Name, false), 
                                new Model.Player(entity.Player2ID, entity.Player2Name, entity.SinglePlayerMode) 
                            }
                    select new Model.Game(entity.ID,
                                          players[0],
                                          players[1],
                                          Model.GameStatus.CreateFromEntity(entity, players),
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
                RowsPerPlayer = game.GameBoard.Properties.RowsPerPlayer,
                BombsPerPlayer = game.GameBoard.Properties.BombsPerPlayer,
                CurrentPlayerID = game.Player1.ID,
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

            var players = new[] { game.Player1, game.Player2 };

            return new Model.Game(model.ID, game.Player1, game.Player2, Model.GameStatus.CreateFromEntity(model, players), game.GameBoard);
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
            entity.Active = game.GameStatus.GameActive;
            entity.CurrentPlayerID = (game.GameStatus.CurrentPlayer != null ? game.GameStatus.CurrentPlayer.ID : null);
            entity.GameResultID = (int)game.GameStatus.CurrentMoveResult;
            entity.WinnerID = (game.GameStatus.Winner != null ? game.GameStatus.Winner.ID : null);
            entity.NumRows = game.GameBoard.NumRows;
            entity.NumCols = game.GameBoard.NumCols;
            entity.RowsPerPlayer = game.GameBoard.Properties.RowsPerPlayer;
            entity.BombsPerPlayer = game.GameBoard.Properties.BombsPerPlayer;
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
                    if (existingToken.Row != currentToken.Row ||
                        existingToken.Col != currentToken.Col ||
                        existingToken.PlayerID != currentToken.PlayerID ||
                        existingToken.TokenType != currentToken.TokenType)
                    {
                        existingToken.Row = currentToken.Row;
                        existingToken.Col = currentToken.Col;
                        existingToken.PlayerID = currentToken.PlayerID;
                        existingToken.TokenType = currentToken.TokenType;
                        existingToken.UpdatedAtUtc = timestamp;
                    }
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
