using System.Text.Json;

namespace San3a.WebApi.Middleware
{
    public class ValidationMiddleware
    {
        #region Fields
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;
        #endregion

        #region Constructors
        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task InvokeAsync(HttpContext context)
        {
            // Enable request body buffering to allow multiple reads
            context.Request.EnableBuffering();

            // Validate content type for POST, PUT, PATCH requests
            if (IsRequestWithBody(context.Request.Method))
            {
                if (!HasValidContentType(context.Request))
                {
                    _logger.LogWarning(
                        "[VALIDATION] Invalid Content-Type: {ContentType} for {Method} {Path}",
                        context.Request.ContentType,
                        context.Request.Method,
                        context.Request.Path
                    );

                    await WriteValidationErrorResponse(
                        context,
                        "Invalid Content-Type. Expected 'application/json' or 'multipart/form-data'."
                    );
                    return;
                }

                // Validate JSON payload for JSON requests
                if (context.Request.ContentType?.Contains("application/json") == true)
                {
                    if (!await IsValidJson(context.Request))
                    {
                        _logger.LogWarning(
                            "[VALIDATION] Invalid JSON payload for {Method} {Path}",
                            context.Request.Method,
                            context.Request.Path
                        );

                        await WriteValidationErrorResponse(
                            context,
                            "Invalid JSON format in request body."
                        );
                        return;
                    }

                    // Reset the stream position for the next middleware
                    context.Request.Body.Position = 0;
                }
            }

            // Validate request size
            if (context.Request.ContentLength > 10 * 1024 * 1024) // 10MB limit
            {
                _logger.LogWarning(
                    "[VALIDATION] Request size exceeds limit: {Size} bytes for {Method} {Path}",
                    context.Request.ContentLength,
                    context.Request.Method,
                    context.Request.Path
                );

                await WriteValidationErrorResponse(
                    context,
                    "Request size exceeds the maximum allowed limit of 10MB."
                );
                return;
            }

            await _next(context);
        }
        #endregion

        #region Private Methods
        private bool IsRequestWithBody(string method)
        {
            return method == HttpMethods.Post ||
                   method == HttpMethods.Put ||
                   method == HttpMethods.Patch;
        }

        private bool HasValidContentType(HttpRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ContentType))
                return false;

            var contentType = request.ContentType.ToLower();
            return contentType.Contains("application/json") ||
                   contentType.Contains("multipart/form-data") ||
                   contentType.Contains("application/x-www-form-urlencoded");
        }

        private async Task<bool> IsValidJson(HttpRequest request)
        {
            try
            {
                using var reader = new StreamReader(request.Body, leaveOpen: true);
                var body = await reader.ReadToEndAsync();

                if (string.IsNullOrWhiteSpace(body))
                    return true; // Empty body is valid

                // Try to parse as JSON
                JsonDocument.Parse(body);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[VALIDATION] Error reading request body");
                return false;
            }
        }

        private async Task WriteValidationErrorResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var errorResponse = new
            {
                Success = false,
                Message = message,
                StatusCode = StatusCodes.Status400BadRequest
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, options);
            await context.Response.WriteAsync(json);
        }
        #endregion
    }
}
