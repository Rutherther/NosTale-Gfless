using System;
using System.Linq;
using System.Threading;
using CommandLine;
using Microsoft.Win32;
using NostaleAuth.Models;
using NosTaleGfless;

namespace NostaleGfless.Example
{
    internal class Program
    {
        static EventWaitHandle ewh = new ManualResetEvent(false);
        
        public static void Main(string[] args)
        {
            string installationIdString = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\Gameforge4d\TNTClient\MainApp",
                "InstallationId", null);
            
            Guid? installationId = null;
            if (installationIdString != null)
            {
                installationId = Guid.Parse(installationIdString);
            }
            
            Parser.Default.ParseArguments<Options>(args).WithParsed(async options =>
            {
                if (options.InstallationId != null)
                {
                    installationId = Guid.Parse(options.InstallationId);
                }
                
                var authenticator = new GameforgeAuthenticator();
                authenticator.InstallationId = installationId;
                var launcher  = await authenticator.Authenticate(options.Email, options.Password);

                GameforgeAccount account = launcher.Accounts.FirstOrDefault();
                if (account == null)
                {
                    throw new InvalidOperationException("There are no nostale account on the gameforge account");
                }

                if (options.AccountName != null)
                {
                    account = launcher.Accounts.FirstOrDefault(x => x.Name == options.AccountName);

                    if (account == null)
                    {
                        account = launcher.Accounts.FirstOrDefault(x => x.Name.Contains(options.AccountName));

                        if (account == null)
                        {
                            throw new InvalidOperationException("Account not found");
                        }
                    }
                }
                
                Console.WriteLine($"Selected account: {account.Name}");
                Console.WriteLine("Launching nostale with the selected account ...");
                    
                Console.WriteLine("Waiting for nostale ...");
                var result = await launcher.Launch(account, options.NostalePath);

                if (result != null && result.Initialized)
                {
                    Console.WriteLine($"Nostale launched successfully. Process id: {result.ProcessId}, Session id: {result.SessionId}");
                }
                else
                {
                    Console.WriteLine("There was an error launching nostale");
                }

                ewh.Set();
            });
            
            ewh.WaitOne();
        }
    }
}