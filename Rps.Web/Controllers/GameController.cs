using Rps.Domain.Services;
using Rps.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DomainModel = Rps.Domain.Model;

namespace Rps.Web.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        // GET: Game
        public async Task<ActionResult> Index()
        {
            var player = ApplicationUser.GetCurrentPlayer(this.HttpContext);
            var games = await this.gameService.GetPlayerActiveGamesAsync(player);
            var model = new GameIndexViewModel(games);

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> NewSinglePlayer()
        {
            var player = ApplicationUser.GetCurrentPlayer(this.HttpContext);
            var bounds = new DomainModel.Bounds(8, 8);
            var properties = new DomainModel.GameProperties(bounds, 3, 2);

            var game = await this.gameService.CreateSinglePlayerGameAsync(player, properties);
            return RedirectToAction("Play", new { id = game.ID });
        }

        public async Task<ActionResult> Play(long id)
        {
            ViewBag.GameID = id;
            return View();
        }
    }
}