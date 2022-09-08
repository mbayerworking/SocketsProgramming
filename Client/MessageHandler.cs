using Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public static class MessageHandler
    {
        [Route("/Message[@type='Response' and @action='HeartBeat']")]
        public static Task HeartBeatResponseHandler(HeartBeatResponseMessage response)
        {
            Console.WriteLine($"Received {response.Action}: {response?.Result?.Status}, {response?.Id}");
            return Task.CompletedTask;
        }
    }
}
