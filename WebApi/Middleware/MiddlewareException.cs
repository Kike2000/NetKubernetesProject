using System.Net;

namespace NetKubernetes.Middleware;

public class MiddlewareException : Exception
{
    public HttpStatusCode StatusCode { get; set; }
    public object? Errors { get; set; }
    public MiddlewareException(HttpStatusCode code, object? errores = null)
    {
        StatusCode = code;
        Errors = errores;
    }
}