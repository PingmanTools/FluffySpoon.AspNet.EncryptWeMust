using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PingmanTools.AspNet.EncryptWeMust.Certes;
using PingmanTools.AspNet.EncryptWeMust.Certificates;
using PingmanTools.AspNet.EncryptWeMust.Persistence;

namespace PingmanTools.AspNet.EncryptWeMust
{
	public static class RegistrationExtensions
	{
		private static void AddLetsEncryptPersistenceService(
			this IServiceCollection services)
		{
			if(services.Any(x => x.ServiceType == typeof(IPersistenceService)))
				return;
			
			services.AddSingleton<IPersistenceService, PersistenceService>();
		}

		public static void AddLetsEncryptRenewalLifecycleHook<TCertificateRenewalLifecycleHook>(
			this IServiceCollection services) where TCertificateRenewalLifecycleHook : class, ICertificateRenewalLifecycleHook
		{
			services.AddLetsEncryptPersistenceService();
			services.AddSingleton<ICertificateRenewalLifecycleHook, TCertificateRenewalLifecycleHook>();
		}

		public static void AddLetsEncryptCertificatePersistence(
			this IServiceCollection services,
			Func<CertificateType, byte[], Task> persistAsync,
			Func<CertificateType, Task<byte[]>> retrieveAsync)
		{
			AddLetsEncryptCertificatePersistence(services,
				new CustomCertificatePersistenceStrategy(
					persistAsync,
					retrieveAsync));
		}

		public static void AddLetsEncryptCertificatePersistence(
		  this IServiceCollection services,
		  ICertificatePersistenceStrategy certificatePersistenceStrategy)
		{
			AddLetsEncryptCertificatePersistence(services,
				(p) => certificatePersistenceStrategy);
		}

		public static void AddLetsEncryptCertificatePersistence(
		  this IServiceCollection services,
		  Func<IServiceProvider, ICertificatePersistenceStrategy> certificatePersistenceStrategyFactory)
		{
			services.AddLetsEncryptPersistenceService();
			services.AddSingleton(certificatePersistenceStrategyFactory);
		}

		public static void AddLetsEncryptFileCertificatePersistence(
		  this IServiceCollection services,
		  string relativeFilePath = "PingmnanToolsAspNetLetsEncryptCertificate")
		{
			AddLetsEncryptCertificatePersistence(services,
				new FileCertificatePersistenceStrategy(relativeFilePath));
		}

		public static void AddLetsEncryptChallengePersistence(
			this IServiceCollection services,
			PersistChallengesDelegate persistAsync,
			RetrieveChallengesDelegate retrieveAsync,
			DeleteChallengesDelegate deleteAsync)
		{
			AddLetsEncryptChallengePersistence(services,
				new CustomChallengePersistenceStrategy(
					persistAsync,
					retrieveAsync,
					deleteAsync));
		}

		public static void AddLetsEncryptChallengePersistence(
		  this IServiceCollection services,
		  IChallengePersistenceStrategy certificatePersistenceStrategy)
		{
			AddLetsEncryptChallengePersistence(services,
				(p) => certificatePersistenceStrategy);
		}

		public static void AddLetsEncryptChallengePersistence(
		  this IServiceCollection services,
		  Func<IServiceProvider, IChallengePersistenceStrategy> certificatePersistenceStrategyFactory)
		{
			services.AddLetsEncryptPersistenceService();
			services.AddSingleton(certificatePersistenceStrategyFactory);
		}

		public static void AddLetsEncryptFileChallengePersistence(
		  this IServiceCollection services,
		  string relativeFilePath = "PingmanToolsAspNetLetsEncryptChallenge")
		{
			AddLetsEncryptChallengePersistence(services,
				new FileChallengePersistenceStrategy(relativeFilePath));
		}

		public static void AddLetsEncryptMemoryChallengePersistence(
		  this IServiceCollection services)
		{	
			AddLetsEncryptChallengePersistence(
				services,
				new MemoryChallengePersistenceStrategy());
		}

		public static void AddLetsEncryptMemoryCertficatesPersistence(
		  this IServiceCollection services)
		{
			AddLetsEncryptCertificatePersistence(
				services,
				new MemoryCertificatePersistenceStrategy());
		}

		public static void AddLetsEncrypt(
		  this IServiceCollection services,
		  LetsEncryptOptions options)
		{
            services.AddTransient<IConfigureOptions<KestrelServerOptions>, KestrelOptionsSetup>();

			services.AddLetsEncryptPersistenceService();

			services.AddSingleton(options);

			services.AddSingleton<ILetsEncryptClientFactory, LetsEncryptClientFactory>();
			services.AddSingleton<ICertificateValidator, CertificateValidator>();
			services.AddSingleton<ICertificateProvider, CertificateProvider>();

			services.AddTransient<ILetsEncryptRenewalService, LetsEncryptRenewalService>();
			services.AddTransient<IHostedService, LetsEncryptRenewalService>();			
		}

		public static void UseLetsEncrypt(
			this IApplicationBuilder app)
		{
			app.UseMiddleware<LetsEncryptChallengeApprovalMiddleware>();
		}
	}
}
