using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cw4.Middlewares
{
    public class LoggingMiddleware
    {

        //2. [..].Path
        //3. [..].Body
        //4. [..].QueryString
        //.....
        //await _next(httpContext);
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var bodyStream = string.Empty;
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStream = await reader.ReadToEndAsync();
            }
            var queryStream = httpContext.Request.QueryString.ToString();

            var zadanie = httpContext.Request.Method.ToString();
            var sciezka = httpContext.Request.Path.ToString();
            using (StreamWriter sw = File.AppendText("Middlewares/requestsLog.txt")) 
            {
                sw.WriteLine("Data: " + DateTime.Now.ToString());
                sw.WriteLine(zadanie);
                sw.WriteLine(sciezka);
                if (bodyStream.Length == 0)
                {
                    sw.WriteLine("Body is null");
                }
                else
                    sw.WriteLine(bodyStream);
                if (queryStream.Length == 0)
                {
                    sw.WriteLine("Query is null");
                }
                else
                    sw.WriteLine(queryStream);
                sw.WriteLine("-------------------------");
            }
            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }

}