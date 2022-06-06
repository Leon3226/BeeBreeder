using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BeeBreeder.Management.Sockets
{
    public class SocketConnectedEventArgs : EventArgs
    {
        public Socket Socket;
    }
}
