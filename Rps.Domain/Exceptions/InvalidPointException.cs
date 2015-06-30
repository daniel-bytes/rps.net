using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rps.Domain.Exceptions
{
    public class InvalidPointException
        : Exception
    {
        public InvalidPointException(string message)
           : base(message)
        {
        }

        public InvalidPointException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
