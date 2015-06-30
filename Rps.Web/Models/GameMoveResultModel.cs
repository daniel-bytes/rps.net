using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DomainModel = Rps.Domain.Model;

namespace Rps.Web.Models
{
    public class GameMoveResultModel
    {
        public long GameID { get; set; }
        public DomainModel.GameMoveResult Result { get; set; }

        public GameMoveResultModel()
        {
        }

        public GameMoveResultModel(long gameID, DomainModel.GameMoveResult result)
        {
            this.GameID = gameID;
            this.Result = result;
        }
    }
}