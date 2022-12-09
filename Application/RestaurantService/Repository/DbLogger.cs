using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
using Serilog.Events;

namespace RestaurantService.Repository
{
    public interface IDbLogger
    {
        public void Information(string Message, int statusCode);
        public void Error(string Message, int statusCode);
    }

    /// <summary>Class <c> DbLogger</c> Class used for logging to the database
    /// .</summary>
    public class DbLogger : IDbLogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DbLogger(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>Logs information levels to the database</summary>
        /// <param name="Message"></param>
        /// <param name="statusCode"></param>
        public void Information(string Message, int statusCode)
        {
            var RequestUri = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
            var endPoint = _httpContextAccessor.HttpContext.Request.Path.Value;
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

            Log.Information("{Msg}{Level}{IP}{RequestUri}{EndPoint}{StatusCode}{CreatedDate}", Message, LogEventLevel.Information, ip, RequestUri, endPoint, statusCode, DateTime.Now);
        }

        /// <summary>Logs error levels to the database</summary>
        /// <param name="Message"></param>
        /// <param name="statusCode"></param>
        public void Error(string Message, int statusCode)
        {
            var RequestUri = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
            var endPoint = _httpContextAccessor.HttpContext.Request.Path.Value;
            var ip = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress;

            Log.Error("{Msg}{Level}{IP}{RequestUri}{EndPoint}{StatusCode}{CreatedDate}", Message, LogEventLevel.Information, ip, RequestUri, endPoint, statusCode, DateTime.Now);
        }
    }
}
