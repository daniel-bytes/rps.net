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
            var player = ApplicationUser.GetCurrentPlayer(this.User);

            var point = new DomainModel.Point(model.MoveToX, model.MoveToY);
            var moveResult = await this.gameService.PerformMoveAsync(model.GameID, player.ID, model.TokenID, point);
            var result = new GameMoveResultModel(model.GameID, moveResult);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}