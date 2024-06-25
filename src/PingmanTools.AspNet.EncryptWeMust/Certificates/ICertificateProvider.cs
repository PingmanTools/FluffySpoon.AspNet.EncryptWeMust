using System.Threading.Tasks;

namespace PingmanTools.AspNet.EncryptWeMust.Certificates
{
    public interface ICertificateProvider
    {
        Task<CertificateRenewalResult> RenewCertificateIfNeeded(IAbstractCertificate current = null);
    }
}