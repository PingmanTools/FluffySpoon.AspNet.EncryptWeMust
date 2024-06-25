using System;

namespace FluffySpoon.AspNet.EncryptWeMust.Certificates
{
    /// <summary>
    /// The most generic form of certificate, metadata provision only
    /// </summary>
    public interface IAbstractCertificate
    {
        DateTime NotAfter { get; }
        DateTime NotBefore { get; }
        string Thumbprint { get; }
    }
}