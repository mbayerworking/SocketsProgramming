using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class EchoServer
    {
        readonly XDocumentMessageDispatcher _messageDispatcher = new XDocumentMessageDispatcher();

        public EchoServer()
        {
            _messageDispatcher.Register<HeartBeatRequestMessage, HeartBeatResponseMessage>(Server.MessageHandler.HandleMessage);
        }

        public void Start(int port = 9000)
        {
            var endPoint = new IPEndPoint(IPAddress.Loopback, port);
            var socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(endPoint);
            socket.Listen(128);

            _ = Task.Run(() => DoEcho(socket));
        }

        private async Task DoEcho(Socket socket)
        {
            do
            {
                var clientSocket = await Task.Factory.FromAsync(
                    new Func<AsyncCallback, object, IAsyncResult>(socket.BeginAccept),
                    new Func<IAsyncResult, Socket>(socket.EndAccept),
                    null).ConfigureAwait(false);

                Console.WriteLine("ECHO SERVER :: CLIENT CONNECTED");

                using var stream = new NetworkStream(clientSocket, true);
                
                var buffer = new Byte[1024];
                do
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
                    if (bytesRead == 0)
                        break;
                    await stream.WriteAsync(buffer, 0, bytesRead).ConfigureAwait(false);
                } while (true);
                
            } while (true);
        }
    }
}
