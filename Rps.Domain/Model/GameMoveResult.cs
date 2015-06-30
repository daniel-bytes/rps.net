using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public enum GameMoveResult
    {
        Nothing = 0,
        TokenMove = 1,
        AttackerWins = 2,
        DefenderWins = 3,
        BothLose = 4,
        GameOver = 5
    }
}
