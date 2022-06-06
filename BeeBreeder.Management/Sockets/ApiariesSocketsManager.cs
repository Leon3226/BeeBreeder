using BeeBreeder.Management.Extensions;
using BeeBreeder.Management.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Sockets
{
    public class ApiariesSocketsManager
    {
        private const int BUFFER_SIZE = 2048;

        private readonly SocketManager _socketManager;
        private readonly IIdentifierGenerator _identifierGenerator;
        public event EventHandler<SocketConnectedEventArgs> Connected = (sender, e) => { };
        private Dictionary<string, Socket> _sockets = new Dictionary<string, Socket>();

        public ApiariesSocketsManager(SocketManager socketManager, IIdentifierGenerator identifierGenerator)
        {
            _socketManager = socketManager;
            _identifierGenerator = identifierGenerator;
            _socketManager.Connected += (sender, args) =>
            {
                HandleConnection(args.Socket);
            };
        }

        public void BeginAccept()
        {
            _socketManager.BeginAccept();
        }

        public string[] AllApiaries()
        {
            return _sockets.Where(x => x.Value.IsConnected()).Select(x => x.Key).ToArray();
        }

        public bool IsActive(string apiary)
        {
            return _sockets.ContainsKey(apiary) && _sockets[apiary].IsConnected();
        }

        private void HandleConnection(Socket socket)
        {
            var identifier = Identifier(socket);
            if (identifier == "")
            {
                SetRandomIdentifier(socket);
            }
            identifier = Identifier(socket);

            if (!_sockets.TryAdd(identifier, socket))
            {
                _sockets[identifier] = socket;
            }
            PrintIdentifier(socket, identifier);
        }

        private void SetRandomIdentifier(Socket apiary)
        {
            RequestAsync(apiary, $"setIdentifier {_identifierGenerator.GenerateIdentifier()}");
        }

        private void PrintIdentifier(Socket apiary, string identifier)
        {
            RequestAsync(apiary, $"print your-identifier-is-{identifier}");
        }

        private string Identifier(Socket apiary)
        {
            return RequestAsync(apiary, "identifier").Result.Replace("\"", "");
        }

        public async Task<string> RequestToApiaryAsync(string identifier, string requestText)
        {
            if (_sockets.TryGetValue(identifier, out var apiarySocket))
            {
                return await RequestAsync(apiarySocket, requestText);
            }
            return "";
        }

        private Task<string> RequestAsync(Socket socket, string requestText)
        {
            string response = "";
            var task = new Task<string>(() => response);
            var buffer = Encoding.UTF8.GetBytes($"{requestText}\n");
            socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);

            void Receive()
            {
                var buffer = new byte[BUFFER_SIZE];
                socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);

                void ReceiveCallback(IAsyncResult AR)
                {
                    try
                    {
                        var result = socket.EndReceive(AR);
                        StringBuilder builder = new StringBuilder();
                        builder.Append(Encoding.ASCII.GetString(buffer, 0, result));
                        while (socket.Available > 0)
                        {
                            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, new AsyncCallback(AppendToResult), null);

                            void AppendToResult(IAsyncResult AR)
                            {
                                result = socket.EndReceive(AR);
                                builder.Append(Encoding.ASCII.GetString(buffer, 0, result));
                            }
                        }
                        response = builder.ToString();
                        task.Start();
                    }
                    catch (ObjectDisposedException)
                    {
                        return;
                    }

                }

            }

            void SendCallback(IAsyncResult AR)
            {
                try
                {
                    var result = socket.EndSend(AR);
                    Receive();
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
            }

            return task;
        }
    }
}
