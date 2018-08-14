using System;

namespace MailChimp.Net.Core
{
    public class MailChimpComplianceRelatedException : Exception
    {
        public MailChimpComplianceRelatedException(string message) : base(message)
        {
        }
    }
}
