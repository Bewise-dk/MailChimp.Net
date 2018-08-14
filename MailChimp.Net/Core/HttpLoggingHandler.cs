using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MailChimp.Net.Core
{
    public enum LoggingLevel
    {
        Trace,
        Info,
        Off
    }

    public class HttpLoggingHandler : DelegatingHandler
    {
        private NLog.ILogger logger;
        private LoggingLevel logLevel;

        public HttpLoggingHandler(HttpClientHandler innerHandler, NLog.ILogger logger, LoggingLevel logLevel)
            : base(innerHandler)
        {
            this.logger = logger;
            this.logLevel = logLevel;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logRequest = string.Empty;
            var logResponse = string.Empty;
            var headersOutput = string.Empty;
            var traceLog = logLevel == LoggingLevel.Info || logLevel == LoggingLevel.Trace;
            foreach (var headerItem in request.Headers)
            {
                headersOutput += $"{headerItem.Key}: {headerItem.Value.FirstOrDefault()} \n";
            }

            logRequest += $"Request {request.Method.Method} {request.Version} {request.RequestUri} - Headers {headersOutput}";
            if (traceLog && request.Content != null)
            {
                var requestContent = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                logRequest += $"\n Request Content {requestContent}\n \n";
            }

            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            var responseHeadersOutput = string.Empty;
            foreach (var headerItem in response.Headers)
            {
                responseHeadersOutput += $"{headerItem.Key}: {headerItem.Value.FirstOrDefault()} \n";
            }
            logResponse += $"\n Response from {request.RequestUri} - {response.StatusCode} {response.Version} - Headers {responseHeadersOutput}";

            if (traceLog && response.Content != null)
            {
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                logResponse += $"\n Response Content {responseContent}\n \n";
            }

            if (traceLog)
            {
                logger.Trace(logRequest);
                logger.Trace(logResponse);
            }
            else
            {
                logger.Info(logRequest);
                logger.Info(logResponse);
            }

            return response;
        }
    }
}
