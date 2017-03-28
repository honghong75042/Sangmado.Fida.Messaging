using System;
using Sangmado.Inka.Logging;
using Sangmado.Inka.Logging.NLogIntegration;
using Sangmado.Fida.Messaging;
using Sangmado.Fida.ServiceModel;

namespace Sangmado.Fida.ServiceModelTestActor
{
    class Program
    {
        static void Main(string[] args)
        {
            CompositeLogger.Use();
            ILog log = Logger.Get<Program>();

            var messageEncoder = new JsonMessageEncoder() { CompressionEnabled = true };
            var messageDecoder = new JsonMessageDecoder() { CompressionEnabled = true };
            var encoder = new ActorMessageEncoder(messageEncoder);
            var decoder = new ActorMessageDecoder(messageDecoder);

            var configruation = new ActorConfiguration(encoder, decoder);
            configruation.Build();

            var actor = new Actor(configruation);
            actor.DataReceived += (s, e) =>
            {
                var text = decoder.Decode<string>(e.Data, e.DataOffset, e.DataLength);
                log.DebugFormat(text);
            };
            actor.Bootup();

            while (true)
            {
                try
                {
                    string text = Console.ReadLine();
                    if (text.ToLowerInvariant() == "quit" || text.ToLowerInvariant() == "exit")
                        break;

                    var message = encoder.Encode(text);
                    actor.Send("master", message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
