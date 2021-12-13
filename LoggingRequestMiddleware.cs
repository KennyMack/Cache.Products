using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cache.Products
{
    public class LoggingRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // IMyScopedService is injected into Invoke
        public async Task InvokeAsync(HttpContext context)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            // the code that you want to measure comes here
            
            System.Diagnostics.Debug.WriteLine($"Inicio: {watch.ElapsedMilliseconds}");

            // Call the next delegate/middleware in the pipeline
            await _next(context); 
            watch.Stop();
            System.Diagnostics.Debug.WriteLine($"Termino: {watch.ElapsedMilliseconds}");
        }
    }
}
