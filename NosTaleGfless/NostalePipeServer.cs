using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NosTaleGfless.Pipes;

namespace NosTaleGfless
{
    public class NostalePipeServer
    {
        protected string _sessionToken;
        protected CancellationTokenSource _cancellationTokenSource;

        public NostalePipeServer(NostaleProcess process, string sessionToken)
        {
            _sessionToken = sessionToken;
            Process = process;
            _cancellationTokenSource = new CancellationTokenSource();

            Process.Process.Exited += OnNostaleExit;
        }

        public bool Successful { get; protected set; }

        public NostaleProcess Process { get; }

        public async Task Start()
        {
            byte[] buffer = new byte[1024];
            List<NamedPipeServerStream> servers = new List<NamedPipeServerStream>();
            servers.Add(CreateServer());
            
            while (!Successful)
            {
                NamedPipeServerStream serverStream = servers.Last();
                try
                {
                    await serverStream.WaitForConnectionAsync(_cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    serverStream.Dispose();
                    break;
                }

                int read = await serverStream.ReadAsync(buffer, 0, 1024);
                ParamsMessage message = ParseMessage(buffer, read);

                string output = null;
                switch (message.Method)
                {
                    case "ClientLibrary.isClientRunning":
                        output = CreateResult<bool, ResultMessage<bool>>(message, true);
                        break;
                    case "ClientLibrary.initSession":
                        output = CreateResult<string, ResultMessage<string>>(message, message.Params.SessionId);
                        break;
                    case "ClientLibrary.queryAuthorizationCode":
                        output = CreateResult<string, ResultMessage<string>>(message, _sessionToken);
                        _sessionToken = null;
                        break;
                    case "ClientLibrary.queryGameAccountName":
                        output = CreateResult<string, ResultMessage<string>>(message, Process.Account.Name);
                        Process.Process.Exited -= OnNostaleExit;
                        Successful = true;
                        break;
                }

                if (!Successful)
                {
                    servers.Add(CreateServer());
                }

                if (output != null)
                {
                    byte[] data = Encoding.ASCII.GetBytes(output);
                    serverStream.Write(data, 0, data.Length);
                }
            }

            foreach (NamedPipeServerStream server in servers)
            {
                server.Dispose();
            }
        }

        protected NamedPipeServerStream CreateServer()
        {
            return new NamedPipeServerStream("GameforgeClientJSONRPC", PipeDirection.InOut, 254,
                PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        }

        protected ParamsMessage ParseMessage(byte[] data, int count)
        {
            return JsonConvert.DeserializeObject<ParamsMessage>(Encoding.ASCII.GetString(data, 0, count));
        }

        protected string SerializeResult<T>(ResultMessage<T> result)
        {
            return JsonConvert.SerializeObject(result);
            ;
        }

        protected string CreateResult<T, U>(ParamsMessage receivedMessage, T result)
            where U : ResultMessage<T>, new()
        {
            return SerializeResult(new U
            {
                Id = receivedMessage.Id,
                Jsonrpc = receivedMessage.Jsonrpc,
                Result = result
            });
        }

        protected void OnNostaleExit(object sender, EventArgs args)
        {
            Process.Process.Exited -= OnNostaleExit;
            _cancellationTokenSource.Cancel();
        }
    }
}