using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rps.Web.Models
{
    public enum GameTokenType
    {
        Empty = 0,
        Rock = 1,
        Paper = 2,
        Scissor = 3,
        Bomb = 4,
        Flag = 5,

        OtherPlayer = 100
    }
}