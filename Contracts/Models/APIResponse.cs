using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Models
{
    public sealed class APIResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; } = false;
        public int StatusCode { get; set; } = (int) HttpStatusCode.OK;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
