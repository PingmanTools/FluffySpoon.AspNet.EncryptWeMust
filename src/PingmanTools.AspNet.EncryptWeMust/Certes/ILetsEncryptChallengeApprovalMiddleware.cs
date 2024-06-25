using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PingmanTools.AspNet.EncryptWeMust.Certes
{
	public interface ILetsEncryptChallengeApprovalMiddleware
	{
		Task InvokeAsync(HttpContext context);
	}
}