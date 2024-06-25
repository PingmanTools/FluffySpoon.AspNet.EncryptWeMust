using Certes;

namespace PingmanTools.AspNet.EncryptWeMust.Certificates
{
    /// <summary>
    /// A certificate which can be persisted as a stream of bytes
    /// </summary>
    public interface IPersistableCertificate : IAbstractCertificate
    {
        byte[] RawData { get; }
    }
    
    /// <summary>
    /// A certificate which can return an IKey
    /// </summary>
    public interface IKeyCertificate
    {
        IKey Key { get; }
    }
}