using System.Diagnostics;
using System.Security.Claims;

namespace San3a.WebApi.Middleware
{
    public class AuditMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly ILogger<AuditMiddleware> _logger;
        #endregion

        #region Constructors
        public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestPath = context.Request.Path;
            var requestMethod = context.Request.Method;
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var userEmail = context.User?.FindFirst(ClaimTypes.Email)?.Value ?? "N/A";

            try
            {
                _logger.LogInformation(
                    "[AUDIT] Request Started: {Method} {Path} | User: {UserId} ({Email}) | IP: {IPAddress}",
                    requestMethod,
                    requestPath,
                    userId,
                    userEmail,
                    context.Connection.RemoteIpAddress
                );

                await _next(context);

                stopwatch.Stop();

                _logger.LogInformation(
                    "[AUDIT] Request Completed: {Method} {Path} | Status: {StatusCode} | Duration: {Duration}ms | User: {UserId}",
                    requestMethod,
                    requestPath,
                    context.Response.StatusCode,
                    stopwatch.ElapsedMilliseconds,
                    userId
                );
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(
                    ex,
                    "[AUDIT] Request Failed: {Method} {Path} | Duration: {Duration}ms | User: {UserId} | Error: {Error}",
                    requestMethod,
                    requestPath,
                    stopwatch.ElapsedMilliseconds,
                    userId,
                    ex.Message
                );
                throw;
            }
        }
        #endregion
    }
}
