namespace AuthHub.Api.Middleware;

public class PermissionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next) => await next(context);
}
