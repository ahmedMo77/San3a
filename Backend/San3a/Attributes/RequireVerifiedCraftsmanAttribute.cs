using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using San3a.Infrastructure.Data;
using System.Security.Claims;

namespace San3a.WebApi.Attributes
{
    /// <summary>
    /// Authorization attribute to ensure that craftsmen are verified before performing certain actions
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class RequireVerifiedCraftsmanAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            // Check if user is authenticated
            if (!user.Identity?.IsAuthenticated ?? true)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Success = false,
                    Message = "User is not authenticated"
                });
                return;
            }

            // Check if user is a craftsman
            var roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Contains("Craftsman"))
            {
                context.Result = new ForbidResult();
                return;
            }

            // Get craftsman ID
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    Success = false,
                    Message = "User ID not found"
                });
                return;
            }

            // Check if craftsman is verified
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
            var craftsman = await dbContext.Craftsmen.FindAsync(userId);

            if (craftsman == null)
            {
                context.Result = new NotFoundObjectResult(new
                {
                    Success = false,
                    Message = "Craftsman profile not found"
                });
                return;
            }

            if (!craftsman.IsVerified)
            {
                context.Result = new ObjectResult(new
                {
                    Success = false,
                    Message = "Your account is pending verification. Please wait for admin approval before you can perform this action.",
                    IsVerified = false
                })
                {
                    StatusCode = 403
                };
                return;
            }

            // Craftsman is verified, continue
        }
    }
}
