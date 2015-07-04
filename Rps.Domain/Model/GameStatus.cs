using System;
using System.Linq;
using Rps.Domain.Extensions;
using System.Collections.Generic;
using Entity = Rps.Domain.Repository.Entity;

namespace Rps.Domain.Model
{
    public class GameStatus
    {
        public Player CurrentPlayer { get; private set; }
        public Player OtherPlayer { get; private set; }
        public Player Winner { get; private set; }
        public bool GameActive { get; private set; }
        public GameMove CurrentMove { get; private set; }
        public GameMoveResultType CurrentMoveResult { get; private set; }
        public Token AttackerToken { get; private set; }
        public Token DefenderToken { get; private set; }

        public GameStatus(
            Player currentPlayer, 
            Player otherPlayer,
            Player winner,
            bool gameActive,
            GameMove currentMove,
            GameMoveResultType currentMoveResult,
            Token attackerToken,
            Token defenderToken)
        {
            this.CurrentPlayer = currentPlayer;
            this.OtherPlayer = otherPlayer;
            this.Winner = winner;
            this.GameActive = gameActive;
            this.CurrentMove = currentMove;
            this.CurrentMoveResult = currentMoveResult;
            this.AttackerToken = attackerToken;
            this.DefenderToken = defenderToken;
        }

        public static GameStatus CreateActive(Player currentPlayer, Player otherPlayer)
        {
            return new GameStatus(currentPlayer, otherPlayer, null, true, GameMove.Empty, GameMoveResultType.Nothing, null, null);
        }

        public static GameStatus CreateForMove(GameStatus status, GameMove move, GameMoveResultType result, Token attacker, Token defender)
        {
            var active = !result.IsGameOver();
            var winner = result.GetWinner(status.CurrentPlayer, status.OtherPlayer);
            
            // switch current player
            var currentPlayer = status.OtherPlayer;
            var otherPlayer = status.CurrentPlayer;

            return new GameStatus(currentPlayer, otherPlayer, winner, active, move, result, attacker, defender);
        }

        public static GameStatus CreateFromEntity(Entity.Game entity, IEnumerable<Player> players)
        {
            return new Model.GameStatus(
                        players.Single(x => x.ID == entity.CurrentPlayerID),
                        players.Single(x => x.ID != entity.CurrentPlayerID),
                        players.FirstOrDefault(x => x.ID == entity.WinnerID),
                        entity.Active,
                        Model.GameMove.Empty,
                        Model.GameMoveResultType.Nothing,
                        null,
                        null);
        }
    }
}
