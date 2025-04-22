using Microsoft.Extensions.Caching.Memory;

namespace LAHJAAPI.Services2
{
    public class TokenListService
    {
        private readonly IMemoryCache _cache;

        public TokenListService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddTokenToList(string token, DateTime expiration)
        {
            _cache.Set(token, true, expiration);
        }

        public bool IsTokenListed(string token)
        {
            return _cache.TryGetValue(token, out _);
        }
    }

}
