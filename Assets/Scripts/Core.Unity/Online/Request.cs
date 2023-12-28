using System;

namespace Core.Unity.Online
{
    public class Request
    {
        public string Path { get; private set; }
        public Method Method { get; private set; }

        public Action<Request, Response> OnCompleted;

        public bool Successful;
        public uint Error;

        public Request(string path, Method method = Method.GET)
        {

        }
    }
    
    public enum Method
    {
        GET,
        POST,
        PATCH,
        DELETE,
        PUT,
    }
}