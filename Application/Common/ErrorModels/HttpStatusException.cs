using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ErrorModels
{
    public class HttpStatusException : Exception
    {
        public int StatusCode { get; set; }
        public string ContentType { get; set; } = @"text/plain";

        public HttpStatusException(int statusCode)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusException(int statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }

        public HttpStatusException(int statusCode, Exception inner) : this(statusCode, inner.ToString())
        {
        }

        public HttpStatusException(int statusCode, JObject errorObject) : this(statusCode, errorObject.ToString())
        {
            this.ContentType = @"application/json";
        }
    }
}