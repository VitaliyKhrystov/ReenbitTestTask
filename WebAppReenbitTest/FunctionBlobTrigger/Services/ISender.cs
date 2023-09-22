

using Microsoft.Extensions.Configuration;

namespace FunctionBlobTrigger.Models
{
    internal interface ISender
    {
        void Send(string to, string subject, string body, IConfigurationRoot config);
    }
}
