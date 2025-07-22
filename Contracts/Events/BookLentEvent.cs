using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class BookLentEvent
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime DueDate { get; set; }
        public string UserEmail { get; set; }
    }
}
