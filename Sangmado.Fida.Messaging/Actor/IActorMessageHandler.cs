using Sangmado.Fida.Messages;
using Sangmado.Fida.ServiceModel;

namespace Sangmado.Fida.Messaging
{
    public interface IActorMessageHandler
    {
        bool CanHandleMessage(MessageEnvelope envelope);
        void HandleMessage(ActorDescription remoteActor, MessageEnvelope envelope);
    }
}
