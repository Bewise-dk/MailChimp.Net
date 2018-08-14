using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace MailChimp.Net.Core
{
    public class Http2CustomHandler : WinHttpHandler
    {
        public Http2CustomHandler()
        {
            this.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls11 | SslProtocols.Tls;
            this.ServerCertificateValidationCallback = (message, cert, chain, errors) => true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            request.Version = new Version("2.0");

            return base.SendAsync(request, cancellationToken);
        }
    }
}
