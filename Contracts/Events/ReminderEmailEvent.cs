using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class ReminderEmailEvent
    {
        public string UserEmail { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }
    }
}
