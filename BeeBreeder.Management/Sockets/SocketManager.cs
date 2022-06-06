using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Sockets
{
    public class SocketManager
    {
        Socket _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public event EventHandler<SocketConnectedEventArgs> Connected = (sender, e) => { };
        List<Socket> _clientSockets = new List<Socket>();

        public SocketManager(IPEndPoint endPoint)
        {
            _listenSocket.Bind(endPoint);
        }

        public void BeginAccept()
        {
            _listenSocket.Listen(1);
            _listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = _listenSocket.EndAccept(AR);
                _clientSockets.Add(socket);
                Connected.Invoke(this, new SocketConnectedEventArgs() { Socket = socket });
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            Console.WriteLine("Client connected, waiting for request...");
            _listenSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            byte[] buffer = new byte[256];
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                _clientSockets.Remove(current);
                return;
            }
        }

        private void CloseAllSockets()
        {
            foreach (Socket socket in _clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            _listenSocket.Close();
        }
    }
}
