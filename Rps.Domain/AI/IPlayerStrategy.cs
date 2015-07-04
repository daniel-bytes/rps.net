using Rps.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.AI
{
    public interface IPlayerStrategy
    {
        GameMove GetNextMove(GameBoard gameBoard, Player player);
    }
}
