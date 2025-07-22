using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class CheckBookAvailabilityResponse
    {
        public string? BookTitle { get; set; }
        public bool IsAvailable { get; set; }
    }
}
