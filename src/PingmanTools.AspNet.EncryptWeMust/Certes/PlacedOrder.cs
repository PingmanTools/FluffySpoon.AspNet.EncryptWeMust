using Certes.Acme;
using PingmanTools.AspNet.EncryptWeMust.Persistence;

namespace PingmanTools.AspNet.EncryptWeMust.Certes
{
    public class PlacedOrder
    {
        public ChallengeDto[] Challenges { get; }
        public IOrderContext Order { get; }
        public IChallengeContext[] ChallengeContexts { get; }

        public PlacedOrder(
            ChallengeDto[] challenges,
            IOrderContext order,
            IChallengeContext[] challengeContexts)
        {
            Challenges = challenges;
            Order = order;
            ChallengeContexts = challengeContexts;
        }
    }
}