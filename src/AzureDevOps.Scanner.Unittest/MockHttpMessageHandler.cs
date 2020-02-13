using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AzureDevOps.Scanner.Unittest
{
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            throw new NotImplementedException($"This call was not mocked: {request.RequestUri.AbsoluteUri} ");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
