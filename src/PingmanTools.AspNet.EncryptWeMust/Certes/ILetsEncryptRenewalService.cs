﻿using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace PingmanTools.AspNet.EncryptWeMust.Certes
{
	public interface ILetsEncryptRenewalService: IHostedService, IDisposable
	{
		Uri LetsEncryptUri { get; }
		Task RunOnceAsync();
	}
}