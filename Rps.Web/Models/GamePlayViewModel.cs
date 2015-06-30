using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rps.Web.Models
{
    public class GameIndexViewModel
    {
        public GameIndexViewModel()
        {
        }

        public GameIndexViewModel(IEnumerable<Game> games)
        {
            var singlePlayer = (from game in games
                                where game.Player2.IsComputerControlled
                                orderby game.ID descending
                                select game.ID).FirstOrDefault();

            this.CurrentSinglePlayerGame = singlePlayer;
        }

        public long CurrentSinglePlayerGame { get; set; }
        public bool HasCurrentSinglePlayerGame { get { return CurrentSinglePlayerGame > 0; } }
    }
}