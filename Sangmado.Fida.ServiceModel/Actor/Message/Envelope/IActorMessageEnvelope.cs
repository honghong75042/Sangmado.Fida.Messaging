using System;

namespace Sangmado.Fida.ServiceModel
{
    public interface IActorMessageEnvelope
    {
        string MessageID { get; set; }
        DateTime MessageTime { get; set; }

        string MessageType { get; set; }
        byte[] MessageData { get; set; }
    }
}
