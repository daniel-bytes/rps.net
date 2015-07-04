using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public enum GameMoveResultType
    {
        Nothing = 0,
        TokenMove = 1,
        AttackerWins = 2,
        DefenderWins = 3,
        BothLose = 4,
        FlagCapturedByAttacker = 5,
        AttackerOutOfPieces = 6,
        DefenderOutOfPieces = 7,
        BothOutOfPieces = 8
    }
}
