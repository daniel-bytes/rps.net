using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Model
{
    public class Game
    {
        public long ID { get; private set; }
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public bool Active { get; private set; }
        public GameBoard GameBoard { get; private set; }

        public Game(long id,
                    Player player1,
                    Player player2,
                    bool active,
                    GameBoard gameBoard)
        {
            this.ID = id;
            this.Player1 = player1;
            this.Player2 = player2;
            this.Active = active;
            this.GameBoard = gameBoard;
        }

        public Player GetPlayer(string id)
        {
            if (Player1.ID == id)
            {
                return Player1;
            }
            else if (Player2.ID == id)
            {
                return Player2;
            }

            throw new ArgumentException("Invalid player ID " + id + ".", "id");
        }

        public Game SetGameOver()
        {
            this.Active = false;
            return this;
        }
    }
}
