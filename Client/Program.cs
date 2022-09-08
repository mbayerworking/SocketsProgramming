using Newtonsoft.Json.Linq;
using ScratchPad;
using Shared;
using System;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using HeartBeatRequestMessage = Shared.HeartBeatRequestMessage;

namespace Client
{


    class Program
    {
        
        static readonly ClientChannel<XmlMessageProtocol, XDocument> Channel = new ClientChannel<XmlMessageProtocol, XDocument >();
        static readonly XDocumentMessageDispatcher MessageDispatcher = new XDocumentMessageDispatcher();

        static async Task Main(string[] args)
        {
            MessageDispatcher.Register<Shared.HeartBeatResponseMessage>(MessageHandler.HeartBeatResponseHandler);

            Console.WriteLine("Press Enter to Connect");
            Console.ReadLine();
            
            var endPoint = new IPEndPoint(IPAddress.Loopback, 9000);

            //var channel = new ClientChannel<JsonMessageProtocol, JObject>();


            Channel.OnMessage(MessageDispatcher.DispatchAsync);

            await Channel.ConnectAsync(endPoint).ConfigureAwait(false);

            _ = Task.Run(() => HBLoop(10));

            //await Channel.SendAsync(myMessage).ConfigureAwait(false);

            

            Console.ReadLine();
        }

        static async Task HBLoop(int interval)
        {
            int requestedId = 1;

            while(true)
            {
                var hbRequest = new HeartBeatRequestMessage
                {
                    Id = $"<3{requestedId}<3",
                    POSData = new Shared.POSData { Id = "POS001" }
                };
                await Channel.SendAsync(hbRequest).ConfigureAwait(false);
                await Task.Delay(interval * 1000);
            }
        }
    }
}
