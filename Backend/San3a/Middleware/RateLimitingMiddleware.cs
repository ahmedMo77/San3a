using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;

namespace San3a.WebApi.Middleware
{
    public class RateLimitingMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly ILogger<RateLimitingMiddleware> _logger;
        private static readonly ConcurrentDictionary<string, ClientRequestInfo> _clientRequests = new();
        private readonly int _requestLimit;
        private readonly TimeSpan _timeWindow;
        #endregion

        #region Constructors
        public RateLimitingMiddleware(
            RequestDelegate next, 
            ILogger<RateLimitingMiddleware> logger,
            int requestLimit = 100,
            int timeWindowMinutes = 1)
        {
            _next = next;
            _logger = logger;
            _requestLimit = requestLimit;
            _timeWindow = TimeSpan.FromMinutes(timeWindowMinutes);
        }
        #endregion

        #region Public Methods
        public async Task InvokeAsync(HttpContext context)
        {
            var clientId = GetClientIdentifier(context);
            
            var clientInfo = _clientRequests.GetOrAdd(clientId, _ => new ClientRequestInfo());

            bool isRateLimited = false;
            int retryAfterSeconds = 0;

            lock (clientInfo)
            {
                var now = DateTime.UtcNow;

                // Clean up old requests outside the time window
                clientInfo.RequestTimestamps.RemoveAll(timestamp => now - timestamp > _timeWindow);

                if (clientInfo.RequestTimestamps.Count >= _requestLimit)
                {
                    isRateLimited = true;
                    retryAfterSeconds = (int)(clientInfo.RequestTimestamps[0] + _timeWindow - now).TotalSeconds;
                }
                else
                {
                    clientInfo.RequestTimestamps.Add(now);
                }
            }

            if (isRateLimited)
            {
                _logger.LogWarning(
                    "[RATE_LIMIT] Rate limit exceeded for client: {ClientId} | IP: {IPAddress} | Path: {Path}",
                    clientId,
                    context.Connection.RemoteIpAddress,
                    context.Request.Path
                );

                context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("Retry-After", retryAfterSeconds.ToString());

                var errorResponse = new
                {
                    Success = false,
                    Message = $"Rate limit exceeded. Maximum {_requestLimit} requests per {_timeWindow.TotalMinutes} minute(s).",
                    StatusCode = StatusCodes.Status429TooManyRequests,
                    RetryAfter = $"{retryAfterSeconds} seconds"
                };

                var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
                return;
            }

            await _next(context);
        }
        #endregion

        #region Private Methods
        private string GetClientIdentifier(HttpContext context)
        {
            // Try to get user ID from claims if authenticated
            var userId = context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
                return $"user:{userId}";

            // Fall back to IP address
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            return $"ip:{ipAddress}";
        }
        #endregion

        #region Nested Classes
        private class ClientRequestInfo
        {
            public List<DateTime> RequestTimestamps { get; } = new();
        }
        #endregion
    }
}
