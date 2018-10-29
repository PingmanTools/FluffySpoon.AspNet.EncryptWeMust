﻿using FluffySpoon.AspNet.LetsEncrypt.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FluffySpoon.AspNet.LetsEncrypt
{
	public static class RegistrationExtensions
	{
		internal static bool IsLetsEncryptUsed { get; private set; }

		public static void AddFluffySpoonLetsEncryptRenewalLifecycleHook<TCertificateRenewalLifecycleHook>(
			this IServiceCollection services) where TCertificateRenewalLifecycleHook : class, ICertificateRenewalLifecycleHook
		{
			services.AddSingleton<ICertificateRenewalLifecycleHook, TCertificateRenewalLifecycleHook>();
		}

		public static void AddFluffySpoonLetsEncryptCertificatePersistence(
			this IServiceCollection services,
			Func<string, byte[], Task> persistAsync,
			Func<string, Task<byte[]>> retrieveAsync)
		{
			AddFluffySpoonLetsEncryptCertificatePersistence(services,
				new CustomPersistenceStrategy(
					persistAsync,
					retrieveAsync));
		}

		public static void AddFluffySpoonLetsEncryptCertificatePersistence(
		  this IServiceCollection services,
		  ICertificatePersistenceStrategy certificatePersistenceStrategy)
		{
			AddFluffySpoonLetsEncryptCertificatePersistence(services,
				(p) => certificatePersistenceStrategy);
		}

		public static void AddFluffySpoonLetsEncryptCertificatePersistence(
		  this IServiceCollection services,
		  Func<IServiceProvider, ICertificatePersistenceStrategy> certificatePersistenceStrategyFactory)
		{
			services.AddSingleton(certificatePersistenceStrategyFactory);
		}

		public static void AddFluffySpoonLetsEncryptFileCertificatePersistence(
		  this IServiceCollection services,
		  string relativeFilePath = "FluffySpoonAspNetLetsEncryptCertificate")
		{
			AddFluffySpoonLetsEncryptCertificatePersistence(services,
				new FilePersistenceStrategy(relativeFilePath));
		}

		public static void AddFluffySpoonLetsEncryptChallengePersistence(
			this IServiceCollection services,
			Func<string, byte[], Task> persistAsync,
			Func<string, Task<byte[]>> retrieveAsync)
		{
			AddFluffySpoonLetsEncryptChallengePersistence(services,
				new CustomPersistenceStrategy(
					persistAsync,
					retrieveAsync));
		}

		public static void AddFluffySpoonLetsEncryptChallengePersistence(
		  this IServiceCollection services,
		  IChallengePersistenceStrategy certificatePersistenceStrategy)
		{
			AddFluffySpoonLetsEncryptChallengePersistence(services,
				(p) => certificatePersistenceStrategy);
		}

		public static void AddFluffySpoonLetsEncryptChallengePersistence(
		  this IServiceCollection services,
		  Func<IServiceProvider, IChallengePersistenceStrategy> certificatePersistenceStrategyFactory)
		{
			services.AddSingleton(certificatePersistenceStrategyFactory);
		}

		public static void AddFluffySpoonLetsEncryptFileChallengePersistence(
		  this IServiceCollection services,
		  string relativeFilePath = "FluffySpoonAspNetLetsEncryptChallenge")
		{
			AddFluffySpoonLetsEncryptCertificatePersistence(services,
				new FilePersistenceStrategy(relativeFilePath));
		}

		public static void AddFluffySpoonLetsEncrypt(
		  this IServiceCollection services,
		  LetsEncryptOptions options)
		{
			services.AddSingleton(options);
		}

		public static void AddFluffySpoonLetsEncryptMemoryChallengePersistence(
		  this IServiceCollection services)
		{	
			AddFluffySpoonLetsEncryptChallengePersistence(
				services,
				new MemoryPersistenceStrategy());
		}

		public static void AddFluffySpoonLetsEncryptRenewalService(
		  this IServiceCollection services)
		{
			services.AddHostedService<LetsEncryptRenewalService>();
		}

		public static void UseFluffySpoonLetsEncrypt(
			this IApplicationBuilder app)
		{
			IsLetsEncryptUsed = true;
			app.UseMiddleware<LetsEncryptMiddleware>();
		}
	}
}
