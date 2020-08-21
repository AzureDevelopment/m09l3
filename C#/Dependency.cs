using System.Net.Http;

namespace Company.Function
{
    public class Dependency
    {
        public string Name { get; internal set; }
        public HttpRequestMessage Request { get; internal set; }
        public HttpResponseMessage Response { get; internal set; }
        public double Duration { get; internal set; }
    }
}