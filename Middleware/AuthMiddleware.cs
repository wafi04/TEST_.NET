using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using CrudApi.Services;
using Microsoft.AspNetCore.Authorization;
using CrudApi.Common;
using System.Security.Claims;

namespace CrudApi.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(
        RequestDelegate next, 
        IServiceScopeFactory scopeFactory,
        ILogger<JwtMiddleware> logger)
    {
        _next = next;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() is not null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["accessToken"];

            if (!string.IsNullOrEmpty(token))
            {
                using var scope = _scopeFactory.CreateScope();
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
                var userId = authService.ValidateToken(token);
                
                if (userId.HasValue)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()),
                    };
                    var identity = new ClaimsIdentity(claims, "jwt");
                    context.User = new ClaimsPrincipal(identity);
                    context.Items["UserId"] = userId.Value;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in JWT middleware");
        }

        await _next(context);
    }
}

// AuthorizeAttribute.cs
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        var userId = context.HttpContext.Items["UserId"];

        if (user == null || !user.Identity?.IsAuthenticated == true || userId == null)
        {
            context.Result = new JsonResult(ApiResponse<object>.Fail("Unauthorized"))
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
