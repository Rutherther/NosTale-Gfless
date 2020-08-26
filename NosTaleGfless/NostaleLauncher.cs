using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NostaleAuth.Models;

namespace NosTaleGfless
{
    public class NostaleLauncher
    {
        private readonly object _lock;
        
        public NostaleLauncher()
        {
            _lock = new object();
        }
        
        public Task<NostaleProcess> Launch(GameforgeAccount account, string sessionToken, string nostalePath)
        {
            return Task.Run(() =>
            {
                lock (_lock)
                {
                    NostaleProcess process = RunProcess(account, nostalePath);
                    process.Initialized = ProcessPipeServer(process, sessionToken).GetAwaiter().GetResult();

                    return process;
                }
            });
        }
        
        public NostaleProcess RunProcess(GameforgeAccount account, string nostalePath)
        {
            Guid sessionId = Guid.NewGuid();
            
            Environment.SetEnvironmentVariable("_TNT_CLIENT_APPLICATION_ID", "d3b2a0c1-f0d0-4888-ae0b-1c5e1febdafb");
            Environment.SetEnvironmentVariable("_TNT_SESSION_ID", sessionId.ToString());
            
            string path = GetNostaleClientPath(nostalePath);
            Process process = Process.Start(path, $"gf {(int)account.GetRegionType()}");

            return new NostaleProcess(process, account)
            {
                SessionId = sessionId
            };
        }
        
        public async Task<bool> ProcessPipeServer(NostaleProcess process, string sessionToken)
        {
            var pipeServer = new NostalePipeServer(process, sessionToken);
            await pipeServer.Start();

            return pipeServer.Successful;
        }

        private string GetNostaleClientPath(string nostalePath)
        {
            if (Directory.Exists(nostalePath))
            {
                nostalePath = Path.Combine(nostalePath, "NostaleClientX.exe");
            }

            if (!File.Exists(nostalePath))
            {
                throw new InvalidOperationException("Nostale was not found.");
            }

            return nostalePath;
        }
    }
}