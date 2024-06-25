using System.Threading.Tasks;
using Certes;
using PingmanTools.AspNet.EncryptWeMust.Certificates;

namespace PingmanTools.AspNet.EncryptWeMust.Persistence
{
	public interface IPersistenceService
	{
		Task<IKey> GetPersistedAccountCertificateAsync();
		Task<ChallengeDto[]> GetPersistedChallengesAsync();
		Task<IAbstractCertificate> GetPersistedSiteCertificateAsync();
		Task PersistAccountCertificateAsync(IKey certificate);
		Task PersistChallengesAsync(ChallengeDto[] challenges);
		Task PersistSiteCertificateAsync(IPersistableCertificate certificate);
		Task DeleteChallengesAsync(ChallengeDto[] challenges);
	}
}