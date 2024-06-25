namespace PingmanTools.AspNet.EncryptWeMust.Certes
{
    public class PfxCertificate
    {
        public byte[] Bytes { get; }

        public PfxCertificate(byte[] bytes)
        {
            Bytes = bytes;
        }
    }
}