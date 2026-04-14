namespace Architecture.Web.Services
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var body = "";
            if (context.Request.ContentLength > 0 &&
                (context.Request.ContentType?.Contains("application/json") == true ||
                 context.Request.ContentType?.Contains("application/x-www-form-urlencoded") == true))
            {
                using var reader = new StreamReader(context.Request.Body, leaveOpen: true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var queryString = context.Request.QueryString.ToString();
            var path = context.Request.Path;

            _logger.LogInformation("Request Path: {Path}, Query: {QueryString}, Body: {Body}", path, queryString, body);

            await _next(context);
        }
    }
}
