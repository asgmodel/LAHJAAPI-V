using Stripe;
using Stripe.Checkout;

namespace StripeGateway
{
    public interface IStripeSession
    {
        object ExpireSession(string sessionId);
        Task<Session> GetSession(string sessionId);
        Task<StripeList<Session>> GetSessions(string sessionId);
    }

    public class StripeSession : IStripeSession
    {
        public async Task<Session> GetSession(string sessionId)
        {
            //var service = new SessionService(this.client);
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);
            //var result = mapper.Map<Session>(session);
            return session;
        }

        public async Task<StripeList<Session>> GetSessions(string sessionId)
        {
            var options = new SessionListOptions { };
            var service = new SessionService();
            StripeList<Session> sessions = await service.ListAsync(options);
            //var result = mapper.Map<List<Session>>(sessions);

            return sessions;
        }
        public object ExpireSession(string sessionId)
        {
            var service = new SessionService();
            var session = service.Expire(sessionId);
            return session;
        }
    }
}
