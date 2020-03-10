// -----------------------------------------------------------------------
// <copyright file="MockHttpMessageHandler.cs" company="Freek Giele">
//    This code is licensed under the CC BY License.
//    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF
//    ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
//    TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR
//    A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// </copyright>
// -----------------------------------------------------------------------

namespace AzureDevOps.Scanner.Unittest
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public virtual HttpResponseMessage Send(HttpRequestMessage request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            throw new NotImplementedException($"This call was not mocked: {request.RequestUri.AbsoluteUri} ");
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            return Task.FromResult(this.Send(request));
        }
    }
}
