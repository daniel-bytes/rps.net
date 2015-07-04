using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Rps.Web.Models;
using Rps.Domain.Services;
using DomainModel = Rps.Domain.Model;

namespace Rps.Web.Api
{
    [Authorize]
    public class GameMoveController : ApiController
    {
        private readonly IGameService gameService;

        public GameMoveController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [Route("api/game/{id}/move")]
        public async Task<HttpResponseMessage> Post(GameMoveModel model)
        {
            var results = new GameMoveResultsModel();
            var player = ApplicationUser.GetCurrentPlayer(this.User);

            var point = new DomainModel.Point(model.MoveToX, model.MoveToY);
            var game = await this.gameService.PerformMoveAsync(model.GameID, player.ID, model.TokenID, point);

            results.Add(player, game);

            if (game.GameStatus.CurrentPlayer.IsComputerControlled)
            {
                game = await this.gameService.PerformComputerMoveAsync(model.GameID);

                results.Add(game.GameStatus.CurrentPlayer, game);
            }

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}