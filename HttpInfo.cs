using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Linq;

namespace nettest
{
    public class HttpInfo
    {
        public Dictionary<string, string> HttpHeaders;

        public HttpInfo()
        {
            HttpHeaders = new Dictionary<string, string>();
            HttpHeadersOfInterest = new List<string> {
                "X-Forwarded-For",
                "X-Azure-ClientIP"
            };
        }

        private List<string> HttpHeadersOfInterest;

        public void Update(HttpRequest request)
        {
            foreach (var header in HttpHeadersOfInterest)
            {
                StringValues values = new StringValues();
                if (request.Headers.TryGetValue(header, out values))
                {
                    HttpHeaders.Add(header, values.ElementAt(0));
                }
            }
        }
    }
}
