namespace AuthApiDemo.Utils;

using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AuthApiDemo.Services.Interfaces;
using System;

public class SessionValidFilter : IAsyncActionFilter
{
    private readonly ISessionService _sessionService;

    public SessionValidFilter(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var username = context.HttpContext.User.Identity?.Name;
        if (string.IsNullOrEmpty(username))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var sessionIdClaim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sessionId");
        if (sessionIdClaim == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var sessionId = sessionIdClaim.Value;
        var isValidSession = await _sessionService.IsSessionValidAsync(Guid.Parse(sessionId));

        if (!isValidSession)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
