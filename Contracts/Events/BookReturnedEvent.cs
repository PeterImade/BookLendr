using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class BookReturnedEvent
    {
        public int LendingId { get; set; }
        public int BookId { get; set; }
        public DateTime ReturnedAt { get; set; }
    }
}
