using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class MessageHandler
    {
        [Route("/Message[@type='Request' and @action='HeartBeat']")]
        public static Task<HeartBeatResponseMessage> HandleMessage(HeartBeatRequestMessage request)
        {
            Received(request);

            var response = new HeartBeatResponseMessage
            {
                Id = request.Id,
                POSData = request.POSData,
                Result = new Result { Status = Status.Success }
            };
            Sending(response);
            return Task.FromResult(response);
        }

        static void Received<T>(T m) where T:Message
            => Console.WriteLine($"Received {typeof(T).Name} => Action[{m.Action}]: Request Id {m.Id}");

        static void Sending<T>(T m) where T : Message
            => Console.WriteLine($"Sending{typeof(T).Name} => Action[{m.Action}]: Request Id {m.Id}");

    }
}
