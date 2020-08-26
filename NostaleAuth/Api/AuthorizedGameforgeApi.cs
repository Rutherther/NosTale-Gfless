using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NostaleAuth.Extensions;
using NostaleAuth.Models;

namespace NostaleAuth.Api
{
    public class AuthorizedGameforgeApi
    {
        private List<GameforgeAccount> _accounts;
        
        public AuthorizedGameforgeApi(string authToken, Guid installationId)
        {
            AuthToken = authToken;
            InstallationId = installationId;
        }
        
        public Guid InstallationId { get; set; }
        
        public string AuthToken { get; }
        
        public async Task<IEnumerable<GameforgeAccount>> GetAccounts(bool cache = true)
        {
            if (cache && _accounts != null)
            {
                return _accounts;
            }
            
            var request = new GameforgeRequest<GameforgeAccount>(HttpMethod.Get, "/user/accounts", InstallationId, AuthToken);
            Dictionary<string, GameforgeAccount> response = await request.Send();

            if (response == null)
            {
                return new List<GameforgeAccount>();
            }
            
            _accounts = new List<GameforgeAccount>(response.Values);
            return _accounts;
        }

        public async Task<string> GetSessionToken(GameforgeAccount gameforgeAccount, bool raw = false)
        {
            var request = new GameforgeRequest<SessionRequest, string>(HttpMethod.Post, "/auth/thin/codes", InstallationId, AuthToken);
            var sessionRequest = new SessionRequest
            {
                PlatformGameAccountId = gameforgeAccount.Id
            };
            
            Dictionary<string, string> response = await request.Send(sessionRequest);

            if (response == null)
            {
                return string.Empty;
            }

            string data = (response.GetValueOrDefault("code") ?? string.Empty);
            return raw ? data : data.ToHex();
        }
    }
}