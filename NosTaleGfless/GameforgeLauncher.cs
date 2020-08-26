using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NostaleAuth.Api;
using NostaleAuth.Models;

namespace NosTaleGfless
{
    public class GameforgeLauncher : IDisposable
    {
        private Timer _watchTimer;
        
        public GameforgeLauncher(AuthorizedGameforgeApi api, IEnumerable<GameforgeAccount> accounts, string email)
        {
            Accounts = accounts.ToList().AsReadOnly();
            Email = email;
            Launcher = new NostaleLauncher();
            Api = api;
            
            ActiveProcesses = new ObservableCollection<NostaleProcess>();
        }
        
        public string Email { get; }
        
        public ReadOnlyCollection<GameforgeAccount> Accounts { get; }
        
        public AuthorizedGameforgeApi Api { get; }
        
        public NostaleLauncher Launcher { get; set; }
        
        public ObservableCollection<NostaleProcess> ActiveProcesses { get; }

        public async Task<NostaleProcess> Launch(GameforgeAccount account, string nostalePath)
        {
            NostaleProcess process = await Launcher.Launch(account, await Api.GetSessionToken(account, true), nostalePath);
            if (process != null)
            {
                ActiveProcesses.Add(process);
            }

            return process;
        }

        public void StartProcessWatch()
        {
            StopProcessWatch();
            
            _watchTimer = new Timer((state) =>
            {
                foreach (var process in ActiveProcesses
                    .Where(x => x.HasExited)
                    .ToArray())
                {
                    ActiveProcesses.Remove(process);
                }
            }, null, 1000, 1000);
        }

        public void StopProcessWatch()
        {
            if (_watchTimer != null)
            {
                _watchTimer.Dispose();
                _watchTimer = null;
            }
        }

        public void Dispose()
        {
            StopProcessWatch();
        }
    }
}