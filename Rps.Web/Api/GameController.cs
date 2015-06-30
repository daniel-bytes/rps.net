using Rps.Domain.Services;
using Rps.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Rps.Web.Api
{
    [Authorize]
    public class GameController : ApiController
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [Route("api/game/{id}")]
        public async Task<HttpResponseMessage> Get(long id)
        {
            var player = ApplicationUser.GetCurrentPlayer(this.User);
            
            var game = await this.gameService.GetGameAsync(id);
            var result = new GameBoardModel(game, player);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }
    }
}