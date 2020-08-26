using CommandLine;
using Newtonsoft.Json;

namespace NostaleGfless.Example
{
    public class Options
    {
        [Option('i', "installation", Required = false, HelpText = "Installation guid. Can be obtained from regedit.")]
        public string InstallationId { get; set; }

        [Option('n', "nostale", Required = true, HelpText = "Path to nostale folder or NostaleClientX.exe")]
        public string NostalePath { get; set; }

        [Option('a', "account", Required = false, HelpText = "Name of the account to connect to. Otherwise the first one will be used.")]
        public string AccountName { get; set; }

        [Value(0, MetaName = "Email", Required = true, HelpText = "Gameforge account email")]
        public string Email { get; set; }

        [Value(1, MetaName = "Password", Required = true, HelpText = "Gameforge account password")]
        public string Password { get; set; }
    }
}