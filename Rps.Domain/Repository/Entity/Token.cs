using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Repository.Entity
{
    public class Token
    {
        public long ID { get; set; }
        public long GameID { get; set; }
        public string PlayerID { get; set; }
        public int TokenType { get; set; }
        public int Row { get; set; }
        public int Col { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime UpdatedAtUtc { get; set; }

        public Game Game { get; set; }
    }
}
