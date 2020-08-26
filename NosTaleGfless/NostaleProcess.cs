using System;
using System.Diagnostics;
using NostaleAuth.Models;

namespace NosTaleGfless
{
    public class NostaleProcess
    {
        public NostaleProcess(Process process, GameforgeAccount account)
        {
            Account = account;
            Process = process;
        }
        
        public Process Process { get; }

        public int ProcessId => Process.Id;
        
        public string SessionToken { get; set; }
        
        public GameforgeAccount Account { get; }
        
        public Guid? SessionId { get; internal set; }
        
        public bool Initialized { get; internal set; }

        public bool HasExited => Process.HasExited;
    }
}