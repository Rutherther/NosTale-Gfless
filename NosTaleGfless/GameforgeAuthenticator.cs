using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NostaleAuth.Api;
using NostaleAuth.Models;

namespace NosTaleGfless
{
    public class GameforgeAuthenticator
    {
        public GameforgeAuthenticator(GameforgeApi api = null, Locales locale = null, Guid? installationId = null)
        {
            Api = api ?? new GameforgeApi();
            InstallationId = installationId;
            Locale = locale ?? Locales.Germany;
        }
        
        public Guid? InstallationId { get; set; }
        
        public Locales Locale { get; set; }
        
        public GameforgeApi Api { get; }
        
        public async Task<GameforgeLauncher> Authenticate(string email, string password)
        {
            Guid installationId = InstallationId ?? Api.GenerateIntallationId(email, password);
            AuthorizedGameforgeApi authorizedGameforgeApi = await Api.Login(email, password, Locale, installationId);

            IEnumerable<GameforgeAccount> accounts = await authorizedGameforgeApi.GetAccounts();
            
            return new GameforgeLauncher(authorizedGameforgeApi, accounts, email);
        }
    }
}