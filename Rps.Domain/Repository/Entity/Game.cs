using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rps.Domain.Repository.Entity
{
    public class Game
    {
        public long ID { get; set; }

        [StringLength(128)]
        public string Player1ID { get; set; }

        [StringLength(256)]
        public string Player1Name { get; set; }

        [StringLength(128)]
        public string Player2ID { get; set; }

        [StringLength(256)]
        public string Player2Name { get; set; }

        public bool SinglePlayerMode { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }

        public int NumRows { get; set; }

        public int NumCols { get; set; }

        public int RowsPerPlayer { get; set; }

        public int BombsPerPlayer { get; set; }

        public bool Active { get; set; }

        [StringLength(128)]
        public string CurrentPlayerID { get; set; }

        [StringLength(128)]
        public string WinnerID { get; set; }

        public int? GameResultID { get; set; }

        public ICollection<Token> Tokens { get; set; }

        public Game()
        {
            this.Tokens = new HashSet<Token>();
        }
        
    }
}
