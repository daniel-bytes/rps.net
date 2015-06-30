using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Rps.Web.Models
{
    [DebuggerDisplay("{GameID} {TokenID} - {MoveToX} { MoveToY}")]
    public class GameMoveModel
    {
        public long GameID { get; set; }
        public long TokenID { get; set; }
        public int MoveToX { get; set; }
        public int MoveToY { get; set; }
    }
}