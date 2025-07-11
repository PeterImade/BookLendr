using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events
{
    public class UserRegisteredEvent
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FullName { get; init; }
    }
}
