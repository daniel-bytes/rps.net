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
    public class ComputerGameMoveController : ApiController
    {
        private readonly IGameService gameService;

        public ComputerGameMoveController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [Route("api/game/{id}/computermove")]
        public async Task<HttpResponseMessage> Post(long id)
        {
            var results = new GameMoveResultsModel();
            var player = ApplicationUser.GetCurrentPlayer(this.User);
            
            var game = await this.gameService.PerformComputerMoveAsync(id);
            var computerPlayer = game.GetOtherPlayer(player.ID);

            results.Add(computerPlayer, game);

            return Request.CreateResponse(HttpStatusCode.OK, results);
        }
    }
}