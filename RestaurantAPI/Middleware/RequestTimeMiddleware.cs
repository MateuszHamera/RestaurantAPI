using System.Diagnostics;

namespace RestaurantAPI.Middleware
{
    public class RequestTimeMiddleware : IMiddleware
    {
        private readonly ILogger<RequestTimeMiddleware> _logger;
        private readonly Stopwatch _stopwatch;

        public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
        {
            _logger = logger;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            _stopwatch.Start();
            
            await next.Invoke(context);
            
            _stopwatch.Stop();

            if(_stopwatch.ElapsedMilliseconds > 4000)
            {
                var message = $"Request: {context.Request.Method} at " +
                    $"{context.Request.Path} took {_stopwatch.ElapsedMilliseconds} ms";

                _logger.LogInformation(message);
            }
        }
    }
}
