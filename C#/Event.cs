using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Company.Function
{
    public class Event
    {
        public string Name { get; internal set; }
        public HttpContext Context { get; internal set; }
        public Dictionary<string, string> Additional { get; internal set; }
    }
}