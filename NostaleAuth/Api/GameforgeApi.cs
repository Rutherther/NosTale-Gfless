using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NostaleAuth.Extensions;
using NostaleAuth.Models;
using NostaleAuth.Utils;

namespace NostaleAuth.Api
{
    public class GameforgeApi
    {
        private readonly HttpClient _httpClient;

        public GameforgeApi()
        {
            _httpClient = new HttpClient();
        }

        public Guid GenerateIntallationId(string email, string password)
        {
            return Guid.Parse(Cryptography.ToMd5(email + password));
        }

        public async Task<int> GetLatestVersion()
        {
            var request = new GameforgeRequest<int>(HttpMethod.Get, "/patching/download/nostale/default?branchToken");
            Dictionary<string, int> response = await request.Send();

            return response.GetValueOrDefault("latest");
        }

        public async Task<AuthorizedGameforgeApi> Login(string email, string password, Locales locale, Guid installationId)
        {
            var request = new GameforgeRequest<AuthRequest, string>(HttpMethod.Post, "/auth/sessions", installationId);
            
            var authRequest = new AuthRequest
            {
                Locale = locale.Value,
                Email = email,
                Password = password
            };

            Dictionary<string, string> response = await request.Send(authRequest);
            if (response == null)
            {
                return null;
            }
            
            string authToken = response.GetValueOrDefault("token") ?? string.Empty;
            
            return new AuthorizedGameforgeApi(authToken, installationId);
        }

        public Task<AuthorizedGameforgeApi> Login(string email, string password, Locales locale)
        {
            return Login(email, password, locale, GenerateIntallationId(email, password));
        }
    }
}
