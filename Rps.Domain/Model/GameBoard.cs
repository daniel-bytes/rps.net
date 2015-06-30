using Rps.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public class GameBoard
    {
        private GameProperties properties;
        private Token[,] tokens;

        public GameBoard(GameProperties properties, IEnumerable<Token> tokens)
        {
            this.properties = properties;
            this.tokens = new Token[properties.Bounds.Rows, properties.Bounds.Cols];

            foreach (var token in tokens)
            {
                this.tokens[token.Row, token.Col] = token;
            }
        }

        public Bounds Bounds { get { return properties.Bounds; } }
        public int NumRows { get { return properties.Bounds.Rows; } }
        public int NumCols { get { return properties.Bounds.Cols; } }

        public IEnumerable<Token> GetTokens()
        {
            for(int r = 0; r < NumRows; r++)
            {
                for (int c = 0; c < NumCols; c++)
                {
                    var token = tokens[r, c];

                    if (token != null)
                    {
                        yield return tokens[r, c];
                    }
                }
            }
        }

        public Token Get(Point point)
        {
            Bounds.AssertContains(point);

            return tokens[point.X, point.Y];
        }

        public Token Get(int row, int col)
        {
            return Get(new Point(row, col));
        }

        public Token GetByID(long tokenID)
        {
            return GetTokens().FirstOrDefault(x => x.ID == tokenID);
        }

        public void Remove(Point point)
        {
            tokens[point.X, point.Y] = null;
        }

        public IEnumerable<Point> GetValidMoves(Token token)
        {
            var points = new List<Point>();

            if (token.Point.X > 0)
            {
                points.Add(token.Point.AddX(-1));
            }
            
            if (token.Point.Y > 0)
            {
                points.Add(token.Point.AddY(-1));
            }
            
            if (token.Point.X < (Bounds.Rows - 1))
            {
                points.Add(token.Point.AddX(1));
            }
            
            if (token.Point.Y < (Bounds.Cols - 1))
            {
                points.Add(token.Point.AddY(1));
            }

            return (from point in points
                    let tokenAtPoint = Get(point)
                    where tokenAtPoint == null
                       || tokenAtPoint.PlayerID != token.PlayerID
                    select point
                    ).ToList();
        }

        public GameMoveResult MoveToken(Token token, Point moveTo)
        {
            if (token.TokenType == TokenType.Empty || 
                token.TokenType == TokenType.Flag ||
                token.TokenType == TokenType.Bomb)
            {
                throw new InvalidMoveException(string.Format("Invalid move.  Token type {0} cannot be moved.", token.TokenType));
            }

            var defender = this.Get(moveTo);

            var result = GetTokenAttackResult(token, defender);

            switch(result)
            {
                case GameMoveResult.AttackerWins:
                    Remove(defender.Point);
                    token.SetPoint(moveTo);
                    break;
                case GameMoveResult.DefenderWins:
                    Remove(token.Point);
                    break;
                case GameMoveResult.BothLose:
                    Remove(token.Point);
                    Remove(defender.Point);
                    break;
                case GameMoveResult.TokenMove:
                    token.SetPoint(moveTo);
                    break;
            }

            return result;
        }

        public static GameBoard Empty()
        {
            return new GameBoard(new GameProperties(), Enumerable.Empty<Token>());
        }

        public static GameMoveResult GetTokenAttackResult(Token attacker, Token defender)
        {
            if (defender == null || defender.TokenType == TokenType.Empty)
            {
                return GameMoveResult.TokenMove;
            }
            else
            {
                switch (defender.TokenType)
                {
                    case TokenType.Bomb:
                        return GameMoveResult.BothLose;

                    case TokenType.Flag:
                        return GameMoveResult.GameOver;

                    case TokenType.Rock:
                        if (attacker.TokenType == TokenType.Paper)
                            return GameMoveResult.AttackerWins;
                        else if (attacker.TokenType == TokenType.Scissor)
                            return GameMoveResult.DefenderWins;
                        else
                            return GameMoveResult.BothLose;

                    case TokenType.Paper:
                        if (attacker.TokenType == TokenType.Scissor)
                            return GameMoveResult.AttackerWins;
                        else if (attacker.TokenType == TokenType.Rock)
                            return GameMoveResult.DefenderWins;
                        else
                            return GameMoveResult.BothLose;

                    case TokenType.Scissor:
                        if (attacker.TokenType == TokenType.Rock)
                            return GameMoveResult.AttackerWins;
                        else if (attacker.TokenType == TokenType.Paper)
                            return GameMoveResult.DefenderWins;
                        else
                            return GameMoveResult.BothLose;
                }

                throw new InvalidOperationException(string.Format("Invalid move, unknown state.  Token {0} to Token {1}.", attacker.ID, defender.ID));
            }
        }

        public static GameBoard New(
                            GameProperties properties,
                            Player player1,
                            Player player2)
        {
            var rows = properties.Bounds.Rows;
            var cols = properties.Bounds.Cols;
            var rowsPerPlayer = properties.RowsPerPlayer;
            var bombsPerPlayer = properties.BombsPerPlayer;
            var tokens = new List<Token>();
            var random = Core.Utility.Random.New();

            foreach (var player in new[] { player1, player2 })
            {
                var playerTokenCount = (cols * rowsPerPlayer);
                var rpsTokenCount = (playerTokenCount - 1 - bombsPerPlayer) / 3;
                var coordinates = (from row in Enumerable.Range(0, rowsPerPlayer)
                                   from col in Enumerable.Range(0, cols)
                                   select Tuple.Create(
                                       (player == player2) ? (rows - row - 1) : row,
                                       col)).ToList();

                var tokenTypes = new Dictionary<TokenType, int>
                {
                    { TokenType.Flag, 1 },
                    { TokenType.Bomb, bombsPerPlayer },
                    { TokenType.Rock, rpsTokenCount },
                    { TokenType.Paper, rpsTokenCount },
                    { TokenType.Scissor, rpsTokenCount }
                };

                tokenTypes[TokenType.Bomb] += (playerTokenCount - tokenTypes.Values.Sum()); // rounding errors go to bombs

                Func<Tuple<int, int>> popRandomCoordinate = () => {
                    var idx = random.Next() % coordinates.Count;
                    var results = coordinates[idx];
                    coordinates.RemoveAt(idx);
                    return results;
                };

                foreach (var tokenType in tokenTypes)
                {
                    for (int i = 0; i < tokenType.Value; i++)
                    {
                        var point = popRandomCoordinate();
                        tokens.Add(
                            new Token(0, player.ID, tokenType.Key, point.Item1, point.Item2)
                            );
                    }
                }
            }

            var board = new GameBoard(properties, tokens);

            return board;
        }
    }
}
