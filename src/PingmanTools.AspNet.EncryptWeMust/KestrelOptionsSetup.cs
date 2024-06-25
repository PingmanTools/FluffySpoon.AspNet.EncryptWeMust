using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PingmanTools.AspNet.EncryptWeMust.Certes;
using PingmanTools.AspNet.EncryptWeMust.Certificates;

namespace PingmanTools.AspNet.EncryptWeMust
{
    internal class KestrelOptionsSetup : IConfigureOptions<KestrelServerOptions>
    {
        readonly ILogger<KestrelOptionsSetup> _logger;

        public KestrelOptionsSetup(ILogger<KestrelOptionsSetup> logger)
        {
            _logger = logger;
        }
        
        public void Configure(KestrelServerOptions options)
        {
            if (LetsEncryptRenewalService.Certificate is LetsEncryptX509Certificate x509Certificate)
            {
                options.ConfigureHttpsDefaults(o =>
                {
                    o.ServerCertificateSelector = (_a, _b) => x509Certificate.GetCertificate();
                });
            }
            else if(LetsEncryptRenewalService.Certificate != null)
            {
                _logger.LogError("This certificate cannot be used with Kestrel");
            }
        }
    }
}
